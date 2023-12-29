using Contracts.DTOs.Employee;
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
    private readonly IOrderRepository _orderRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationService _reservationService;

    public EmployeeService(
        IGenericRepository<Employee> employeeRepository,
        IOrderRepository orderRepository,
        IReservationRepository reservationRepository,
        IReservationService reservationService)
    {
        _employeeRepository = employeeRepository;
        _orderRepository = orderRepository;
        _reservationRepository = reservationRepository;
        _reservationService = reservationService;
    }

    public async Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod)
    {

        var orderFilter = new OrderFilter
        {
            OrderStatuses = new List<OrderStatus> { OrderStatus.Paid, OrderStatus.PartiallyPaid, OrderStatus.Created },
            EndDate = timePeriod.EndTime,
            EmployeeId = employeeId
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

        reservations = reservations.OrderBy(r => r.StartDateTime).ToList();

        List<TimeSlot> availableTimeSlots = new List<TimeSlot>();
        DateTime availableStart = timePeriod.StartTime;

        foreach (Reservation reservation in reservations)
        {
            var reservationStart = reservation.StartDateTime;
            var reservationEnd = await _reservationService.CalculateReservationEndTime(employeeId, reservationStart);

            if (reservationStart > availableStart)
            {
                availableTimeSlots.Add(new TimeSlot
                {
                    StartTime = availableStart,
                    EndTime = reservationStart
                });
            }

            availableStart = reservationEnd;
        }

        if (availableStart < timePeriod.EndTime)
        {
            availableTimeSlots.Add(new TimeSlot
            {
                StartTime = availableStart,
                EndTime = timePeriod.EndTime
            });
        }

        return availableTimeSlots;
    }

    public async Task<Employee> Create(EmployeeCreateDto employeeDto)
    {
        if (CheckStartBeforeEnd(employeeDto.StartTime, employeeDto.EndTime))
        {
            throw new InvalidTimeException("Start time is later then the end time.");
        }

        Employee employee = new()
        {
            Id = Guid.NewGuid(),
            StartTime = employeeDto.StartTime,
            EndTime = employeeDto.EndTime
        };
        return await _employeeRepository.Add(employee);
    }

    private static bool CheckStartBeforeEnd(TimeSpan startTime, TimeSpan endTime)
    {
        return startTime < endTime;
    }

}
