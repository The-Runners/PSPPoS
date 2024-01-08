using System.Reflection.Metadata.Ecma335;
using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IEmployeeService _employeeService;

    public ServiceService(
        IServiceRepository serviceRepository,
        IEmployeeService employeeService
    )
    {
        _serviceRepository = serviceRepository;
        _employeeService = employeeService;
    }

    public async Task<Service?> GetServiceById(Guid serviceId)
    { 
        return await _serviceRepository.GetById(serviceId);
    }

    public async Task<Service> Create(ServiceCreateDto serviceDto)
    {
        if (!IsPriceValid(serviceDto.Price) || !IsDurationValid(serviceDto.Duration))
        {
            throw new ValidationException("The given service to be created is invalid.");
        }

        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = serviceDto.Name,
            Duration = serviceDto.Duration,
            Price = serviceDto.Price,
            Employees = serviceDto.Employees 
        };

        return await _serviceRepository.Add(service);
    }

    public async Task<Service?> Edit(Guid serviceId, ServiceEditDto serviceDto)
    {
        var serviceFromDb = await _serviceRepository.GetById(serviceId);
        if (serviceFromDb is null)
        {
            return null;
        }

        if ((serviceDto.Price is not null && !IsPriceValid(serviceDto.Price)) 
            || (serviceDto.Duration is not null && !IsDurationValid(serviceDto.Duration)))
        {
            throw new ValidationException("The given service edit is invalid.");
        }

        var service = new Service
        {
            Id = serviceId,
            Name = serviceDto.Name ?? serviceFromDb.Name,
            Duration = serviceDto.Duration ?? serviceFromDb.Duration,
            Price = serviceDto.Price ?? serviceFromDb.Price,
            Employees = serviceDto.Employees ?? serviceFromDb.Employees,
        };

        return await _serviceRepository.Update(service);
    }

    private static bool IsPriceValid(decimal? price)
    {
        return price >= 0;
    }

    private static bool IsDurationValid(TimeSpan? duration)
    {
        return duration?.TotalMinutes > 0;
    }

    public async Task Delete(Guid serviceId)
    {
        await _serviceRepository.Delete(serviceId);
    }

    public async Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid serviceId, TimeSlot timePeriod)
    {
        var employees = await _serviceRepository.GetServiceEmployees(serviceId);

        if (employees == null)
        {
            return Enumerable.Empty<TimeSlot>();
        }

        var allAvailableTimeSlots = new List<TimeSlot>();

        foreach ( var employee in employees)
        {
            var employeeAvailableTimeSlots = await _employeeService.GetAvailableTimeSlots(employee.Id, timePeriod);
            allAvailableTimeSlots.AddRange(employeeAvailableTimeSlots);

        }
        // Sort time slots by start time, then by end time if start times are equal
        allAvailableTimeSlots = allAvailableTimeSlots.OrderBy(x => x.StartTime).ThenBy(x => x.EndTime).ToList();

        var mergedTimeSlots = new List<TimeSlot>();
        var current = allAvailableTimeSlots[0];

        for (var i = 1; i < allAvailableTimeSlots.Count; i++)
        {
            if (current.EndTime >= allAvailableTimeSlots[i].StartTime)
            {
                // Extend the current time slot to include the overlapping time slot
                if (current.EndTime.CompareTo(allAvailableTimeSlots[i].EndTime) < 0)
                {
                    current.EndTime = allAvailableTimeSlots[i].EndTime;
                }
            }
            else
            {
                // No overlap, add the current time slot to the merged list and move on to the next
                mergedTimeSlots.Add(current);
                current = allAvailableTimeSlots[i];
            }
        }

        // Add the last time slot
        mergedTimeSlots.Add(current);

        return mergedTimeSlots;
    }
}
