using Contracts.DTOs;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Models;
using Infrastructure.Interfaces;
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

    public async Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod)
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

        var orders = await _orderRepository.GetFilteredOrders(orderFilter);

        IEnumerable<Reservation>? reservations = new List<Reservation>();
        if (orders != null)
        {
            foreach (Order order in orders)
            {
                var newReservations = await _reservationRepository.GetReservationByOrderId(order.Id);
                if (newReservations != null)
                {
                    reservations = reservations.Append(newReservations);
                }
            }
        }

        reservations = reservations.OrderBy(r => r.StartTime).ToList();

        var availableTimeSlots = new List<TimeSlot>();
        var availableStart = timePeriod.StartTime;

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

    public async Task<Employee> Create(EmployeeCreateDto employeeDto)
    {
        if (!IsStartTimeValid(employeeDto.StartTime, employeeDto.EndTime))
        {
            throw new InvalidTimeException("Start time is later then the end time.");
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            StartTime = employeeDto.StartTime,
            EndTime = employeeDto.EndTime,
        };
        return await _employeeRepository.Add(employee);
    }

    private static bool IsStartTimeValid(TimeSpan startTime, TimeSpan endTime)
    {
        return startTime < endTime;
    }

    public async Task<Employee?> GetById(Guid employeeId)
    {
        return await _employeeRepository.GetById(employeeId);
    }

    public async Task<Employee?> Edit(EmployeeEditDto employeeDto)
    {
        var employeeFromDb = await _employeeRepository.GetById(employeeDto.Id);
        if (employeeFromDb is null)
        {
            return null;
        }

        var employee = new Employee
        {
            Id = employeeDto.Id,
            StartTime = employeeDto.StartTime ?? employeeFromDb.StartTime,
            EndTime = employeeDto.EndTime ?? employeeFromDb.EndTime,
        };

        return await _employeeRepository.Update(employee);
    }

    public async Task Delete(Guid employeeId)
    {
        await _employeeRepository.Delete(employeeId);
    }
}
