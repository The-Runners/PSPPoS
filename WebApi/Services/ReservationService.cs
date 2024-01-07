using Contracts.DTOs;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
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

    public async Task CreateReservation(ReservationOrderDto reservationOrderDto)
    {
        var reservationStart = reservationOrderDto.TimeSlot;
        var reservationEnd = await CalculateReservationEndTime(reservationOrderDto.ServiceId, reservationStart);

        var reservationSlot = new TimeSlot
        {
            StartTime = reservationStart,
            EndTime = reservationEnd
        };

        if (await CanBookTimeSlot(reservationOrderDto.EmployeeId, reservationSlot))
        {
            var orderDto = new EmptyOrderCreateDto
            {
                EmployeeId = reservationOrderDto.EmployeeId,
                CustomerId = reservationOrderDto.CustomerId
            };
            var order = await _orderService.CreateEmptyOrder(orderDto);

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ServiceId = reservationOrderDto.ServiceId,
                StartTime = reservationOrderDto.TimeSlot
            };

            await _reservationRepository.Add(reservation);

            //TODO: how do we process the created order
        }
        else 
        {
            throw new TimeSlotUnavailableException($"Employee ${reservationOrderDto.EmployeeId} does not have" +
                $" available times in ${reservationStart} - ${reservationEnd}");
        }
    }

    public async Task CancelReservation(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetById(reservationId);
        if (reservation is null)
        {
            throw new NullReferenceException("Reservation with the given id does not exist.");
        }
        /* We just delete the reservation - because an order may have multiple reservations
         * Available times are updated when requesting availble times for employee
         */
        await _reservationRepository.Delete(reservationId);

        var orderEdit = new OrderEditDto
        {
            Status = OrderStatus.Cancelled,
        };
        await _orderService.Edit(reservation.OrderId, orderEdit);
    }

    private async Task<bool> CanBookTimeSlot(Guid employeeId, TimeSlot bookTime)
    {
        var availableTimes = await _employeeService.GetAvailableTimeSlots(employeeId, bookTime);
        if (availableTimes.Count() == 1)
        {
            return true;
        }

        return false;
    }

    private async Task<DateTime> CalculateReservationEndTime(Guid serviceId, DateTime reservationStart) 
    {
        var reservationDuration = await _serviceRepository.GetServiceDuration(serviceId);
        var reservationEnd = reservationStart.Add(reservationDuration);
        return reservationEnd;
    }
}