using Contracts.DTOs;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ReservationService : IReservationService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IEmployeeService _employeeService;
    private readonly IOrderService _orderService;

    public ReservationService(
        IServiceRepository serviceRepository,
        IReservationRepository reservationRepository,
        IEmployeeService employeeService,
        IOrderService orderService
    )
    {
        _serviceRepository = serviceRepository;
        _reservationRepository = reservationRepository;
        _employeeService = employeeService;
        _orderService = orderService;
    }

    public async Task<Either<DomainException, Reservation>> CreateReservation(ReservationOrderDto reservationOrderDto)
    {
        var reservationStart = reservationOrderDto.TimeSlot;
        var reservationEnd = await CalculateReservationEndTime(reservationOrderDto.ServiceId, reservationStart);

        var reservationSlot = new TimeSlot
        {
            StartTime = reservationStart,
            EndTime = reservationEnd,
        };

        if (await CanBookTimeSlot(reservationOrderDto.EmployeeId, reservationSlot))
        {
            var orderDto = new EmptyOrderCreateDto
            {
                EmployeeId = reservationOrderDto.EmployeeId,
                CustomerId = reservationOrderDto.CustomerId,
            };
            var order = await _orderService.CreateEmptyOrder(orderDto);

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ServiceId = reservationOrderDto.ServiceId,
                StartTime = reservationOrderDto.TimeSlot,
            };

            return await _reservationRepository.Add(reservation);
        }

        return new ValidationException($"Employee ${reservationOrderDto.EmployeeId} does not have" +
                                      $" available times in ${reservationStart} - ${reservationEnd}");
    }

    public async Task<Either<DomainException, Order>> CancelReservation(Guid reservationId)
    {
        /* We just delete the reservation - because an order may have multiple reservations
         * Available times are updated when requesting available times for employee
         */
        var reservation = await _reservationRepository.GetById(reservationId);
        if (reservation is null)
        {
            return new NotFoundException(nameof(Reservation), reservationId);
        }

        await _reservationRepository.Delete(reservationId);

        var orderEdit = new OrderEditDto
        {
            Status = OrderStatus.Cancelled,
        };
        return await _orderService.Edit(reservation.OrderId, orderEdit);
    }

    public async Task<IEnumerable<Reservation>> ListAsync(int offset, int limit)
    {
        return await _reservationRepository.ListAsync(offset, limit);
    }

    public async Task<Either<DomainException, Reservation>> GetById(Guid reservationId)
    {
        var result = await _reservationRepository.GetById(reservationId);
        return result is null ? new NotFoundException(nameof(Reservation), reservationId) : result;
    }

    private async Task<bool> CanBookTimeSlot(Guid employeeId, TimeSlot bookTime)
    {
        var availableTimes = await _employeeService.GetAvailableTimeSlots(employeeId, bookTime);
        return availableTimes.Count() == 1;
    }

    private async Task<DateTime> CalculateReservationEndTime(Guid serviceId, DateTime reservationStart) 
    {
        var reservationDuration = await _serviceRepository.GetServiceDuration(serviceId);
        var reservationEnd = reservationStart.Add(reservationDuration);
        return reservationEnd;
    }
}