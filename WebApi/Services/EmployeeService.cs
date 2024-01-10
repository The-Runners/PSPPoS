using Contracts.DTOs;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using WebApi.Interfaces;

namespace WebApi.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IGenericRepository<Employee> _employeeRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IReservationRepository _reservationRepository;

    public EmployeeService(
        IGenericRepository<Employee> employeeRepository,
        IServiceRepository serviceRepository,
        IOrderRepository orderRepository,
        IReservationRepository reservationRepository)
    {
        _employeeRepository = employeeRepository;
        _serviceRepository = serviceRepository;
        _orderRepository = orderRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<Either<DomainException, IEnumerable<TimeSlot>>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod)
    {
        var orderFilter = CreateOrderFilter(employeeId, timePeriod);
        var orders = await _orderRepository.GetFilteredOrders(orderFilter);
        if (orders is null)
        {
            return new NotFoundException(nameof(orders), employeeId);
        }

        var reservations = await GetOrderedReservationsForOrders(orders);
        var availableTimeSlots = new List<TimeSlot>();
        var availableStart = timePeriod.StartTime;
        if (reservations is not null)
        {
            foreach (var reservation in reservations)
            {
                var reservationStart = reservation.StartTime;
                var reservationDuration = await _serviceRepository.GetServiceDuration(employeeId);
                var reservationEnd = reservationStart.Add(reservationDuration);
                if (reservationStart > availableStart)
                {
                    availableTimeSlots.Add(new TimeSlot
                    {
                        StartTime = availableStart,
                        EndTime = reservationStart,
                    });
                }

                availableStart = reservationEnd;
            }
        }
        if (availableStart < timePeriod.EndTime)
        {
            availableTimeSlots.Add(new TimeSlot
            {
                StartTime = availableStart,
                EndTime = timePeriod.EndTime,
            });
        }

        return availableTimeSlots;
    }

    public async Task<IEnumerable<Employee>> ListAsync(int offset, int limit)
    {
        return await _employeeRepository.ListAsync(offset, limit);
    }

    public async Task<Either<DomainException, Employee>> Create(EmployeeCreateDto employeeDto)
    {
        if (!IsStartTimeValid(employeeDto.StartTime, employeeDto.EndTime))
        {
            return new ValidationException("Employee start time is later than the end time.");
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            StartTime = employeeDto.StartTime.ToTimeSpan(),
            EndTime = employeeDto.EndTime.ToTimeSpan(),
        };
        return await _employeeRepository.Add(employee);
    }

    private static bool IsStartTimeValid(TimeOnly startTime, TimeOnly endTime)
    {
        return startTime < endTime;
    }

    public async Task<Either<DomainException, Employee>> GetById(Guid employeeId)
    {
        var result = await _employeeRepository.GetById(employeeId);
        return result is null ? new NotFoundException(nameof(Employee), employeeId) : result;
    }

    public async Task<Either<DomainException, Employee>> Edit(Guid employeeId, EmployeeEditDto employeeDto) =>
        await GetById(employeeId).BindAsync<DomainException, Employee, Employee>(async employee =>
        {
            if (employee is null)
            {
                return new NotFoundException(nameof(Employee), employeeId);
            }

            employee.StartTime = employeeDto.StartTime?.ToTimeSpan() ?? employee.StartTime;
            employee.EndTime = employeeDto.EndTime?.ToTimeSpan() ?? employee.EndTime;

            return await _employeeRepository.Update(employee);
        });

    public async Task<Either<DomainException, Unit>> Delete(Guid employeeId)
    {
        return await GetById(employeeId)
            .MapAsync(async _ => await _employeeRepository.Delete(employeeId))
            .Map(_ => Unit.Default);
    }

    private static OrderFilter CreateOrderFilter(Guid employeeId, TimeSlot timePeriod)
    {
        var orderFilter = new OrderFilter
        {
            OrderStatuses = new List<OrderStatus>
            {
                OrderStatus.Completed,
                OrderStatus.Ordered,
                OrderStatus.Created,
            },
            EndDate = timePeriod.EndTime,
            EmployeeId = employeeId,
        };

        return orderFilter;
    }

    private async Task<List<Reservation>?> GetOrderedReservationsForOrders(List<Order> orders)
    {
        var reservations = new List<Reservation>();
        foreach (var order in orders)
        {
            var reservation = await _reservationRepository.GetReservationByOrderId(order.Id);
            if (reservation is not null)
            {
                reservations.Add(reservation);
            }
        }

        if (reservations.Count > 0)
        {
            reservations = reservations.OrderBy(r => r.StartTime).ToList();
        }

        return reservations;
    }
}
