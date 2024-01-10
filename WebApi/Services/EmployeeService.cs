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
    private readonly IServiceEmployeeRepository _serviceEmployeeRepository;

    public EmployeeService(
        IGenericRepository<Employee> employeeRepository,
        IServiceRepository serviceRepository,
        IOrderRepository orderRepository,
        IReservationRepository reservationRepository,
        IServiceEmployeeRepository serviceEmployeeRepository)
    {
        _employeeRepository = employeeRepository;
        _serviceRepository = serviceRepository;
        _orderRepository = orderRepository;
        _reservationRepository = reservationRepository;
        _serviceEmployeeRepository = serviceEmployeeRepository;
    }

    public async Task<Either<DomainException, IEnumerable<TimeSlot>>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod)
    {
        if (!AreDatesInSameDay(timePeriod.StartTime, timePeriod.EndTime))
        {
            return new ValidationException("The time period is not within the same day");
        }

        //We get employee start time and end time
        var employee = await _employeeRepository.GetById(employeeId);
        if (employee is null)
        {
            return new NotFoundException("Was not able to find employee", employeeId);
        }

        var employeeStartTime = ReplaceTimeInDateTime(timePeriod.StartTime, employee.StartTime);
        var employeeEndTime = ReplaceTimeInDateTime(timePeriod.EndTime, employee.EndTime);
        if (employeeStartTime > timePeriod.StartTime
            || employeeStartTime > timePeriod.EndTime
            || employeeEndTime < timePeriod.StartTime
            || employeeEndTime < timePeriod.EndTime)
        {
            return new ValidationException(
                $"The given reservation start time is outside of the employee '{employeeId}' work hours.");
        }

        var orderFilter = CreateOrderFilter(employeeId, timePeriod);
        var orders = await _orderRepository.GetFilteredOrders(orderFilter);
        if (orders is null)
        {
            return new NotFoundException($"Was not able to find orders for {nameof(orders)}", employeeId);
        }

        var reservations = await GetOrderedReservationsForOrders(orders);
        var availableTimeSlots = new List<TimeSlot>();
        var availableStart = employeeStartTime;
        if (reservations is not null)
        {
            foreach (var reservation in reservations)
            {
                var reservationStart = reservation.StartTime;
                var service = await _serviceRepository.GetById(reservation.ServiceId);
                var reservationEnd = reservationStart.Add(service!.Duration);
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
        // Add the remaining time slot after the last reservation if any
        if (availableStart < employeeEndTime && availableStart < timePeriod.EndTime)
        {
            availableTimeSlots.Add(new TimeSlot
            {
                StartTime = availableStart,
                EndTime = timePeriod.EndTime < employeeEndTime ? timePeriod.EndTime : employeeEndTime, // Ensure it doesn't go beyond employee's end time
            });
        }

        return availableTimeSlots;
    }

    public async Task<List<Employee>?> GetEmployeesByServiceId(Guid serviceId)
    {
        var filteredServiceEmployees = await _serviceEmployeeRepository
            .GetServiceEmployeesByServiceId(serviceId);
        if (filteredServiceEmployees is null)
        {
            return null;
        }

        var employees = new List<Employee>();
        foreach (var serviceEmployee in filteredServiceEmployees)
        {
            var employee = await _employeeRepository.GetById(serviceEmployee.EmployeeId);
            if (employee is not null)
            {
                employees.Add(employee);
            }
        }

        return employees;
    }

    private DateTime ReplaceTimeInDateTime(DateTime baseDateTime, TimeSpan newTime)
    {
        return baseDateTime.Date.Add(newTime);
    }

    private bool AreDatesInSameDay(DateTime date1, DateTime date2)
    {
        return date1.Date == date2.Date;
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
