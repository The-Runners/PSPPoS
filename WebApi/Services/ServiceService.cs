﻿using Contracts.DTOs.Service;
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

    public async Task<Service?> Get(Guid serviceId)
    { 
        return await _serviceRepository.GetById(serviceId);
    }

    public async Task<Service> Create(ServiceCreateDto serviceDto)
    {
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

    public async Task<Service> Update(ServiceModelDto serviceDto) 
    {
        var service = new Service
        {
            Id = serviceDto.Id,
            Name = serviceDto.Name,
            Duration = serviceDto.Duration,
            Price = serviceDto.Price,
            Employees = serviceDto.Employees
        };
        return await _serviceRepository.Update(service);
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

        for (int i = 1; i < allAvailableTimeSlots.Count; i++)
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