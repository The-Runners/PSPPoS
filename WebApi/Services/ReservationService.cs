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
    private readonly IGenericRepository<Employee> _employeeRepository;
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IEmployeeService _employeeService;
    private readonly IOrderService _orderService;

    public ReservationService(
        IServiceRepository serviceRepository,
        IReservationRepository reservationRepository,
        IEmployeeService employeeService,
        IOrderService orderService,
        IGenericRepository<Employee> employeeRepository,
        IGenericRepository<Customer> customerRepository)
    {
        _serviceRepository = serviceRepository;
        _reservationRepository = reservationRepository;
        _employeeService = employeeService;
        _orderService = orderService;
        _employeeRepository = employeeRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Either<DomainException, Reservation>> CreateReservation(ReservationOrderDto reservationOrderDto)
    {
        var reservationStart = reservationOrderDto.TimeSlot;
        var serviceFromDb = await _serviceRepository.GetById(reservationOrderDto.ServiceId);
        if (serviceFromDb is null)
        {
            return new NotFoundException(nameof(Service), reservationOrderDto.ServiceId);
        }

        var reservationEnd = reservationStart.Add(serviceFromDb.Duration);
        var reservationSlot = new TimeSlot
        {
            StartTime = reservationStart,
            EndTime = reservationEnd,
        };

        if (await CanBookTimeSlot(reservationOrderDto.EmployeeId, reservationSlot))
        {
            var employee = await _employeeRepository.GetById(reservationOrderDto.EmployeeId);
            if (employee is null)
            {
                return new NotFoundException(nameof(Employee), reservationOrderDto.EmployeeId);
            }

            var customer = await _customerRepository.GetById(reservationOrderDto.CustomerId);
            if (customer is null)
            {
                return new NotFoundException(nameof(Customer), reservationOrderDto.CustomerId);
            }

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
                                      $" available times from ${reservationStart} - ${reservationEnd}. Reservation" +
                                      $" not available.");
    }

    public async Task<Either<DomainException, Order>> CancelReservation(Guid reservationId) =>
        await GetById(reservationId).BindAsync<DomainException, Reservation, Order>(async reservation =>
        {
            if (reservation is null)
            {
                return new NotFoundException(nameof(Reservation), reservationId);
            }

            /* We just delete the reservation - because an order may have multiple reservations
             * Available times are updated when requesting available times for employee
             */
            await _reservationRepository.Delete(reservationId);

            var orderEdit = new OrderEditDto
            {
                Status = OrderStatus.Cancelled,
            };
            return await _orderService.Edit(reservation.OrderId, orderEdit);
        });

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
        var availableTimesResult = await _employeeService.GetAvailableTimeSlots(employeeId, bookTime);
        var check = false;
        availableTimesResult.Match(
            Right: timeSlots =>
            {
                check = CheckAllAvailableTimeSlotsIfBookedTimeFits(timeSlots, bookTime);
            },
            Left: _ => {}
        );
        return check;
    }

    private bool CheckAllAvailableTimeSlotsIfBookedTimeFits(IEnumerable<TimeSlot> timeSlots, TimeSlot bookTime)
    {
        var check = false;
        if (timeSlots.Any())
        {
            foreach (var availableTimeSlot in timeSlots)
            {
                if (IsBookTimeFitIntoAvailableTimeSlot(bookTime, availableTimeSlot))
                {
                    check = true;
                }
            }
        }
        return check;
    }

    private bool IsBookTimeFitIntoAvailableTimeSlot(TimeSlot bookTime, TimeSlot availableTime) 
    {
        return (bookTime.StartTime >= availableTime.StartTime &&
            bookTime.EndTime <= availableTime.EndTime) ? true : false;
    }
}