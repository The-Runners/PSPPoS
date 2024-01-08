using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
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

    public async Task<Either<DomainException, Service>> GetByIdAsync(Guid serviceId)
    { 
        var result = await _serviceRepository.GetById(serviceId);

        return result is null
            ? new NotFoundException(nameof(Customer), serviceId)
            : result;
    }

    public async Task<IEnumerable<Service>> ListAsync(int offset, int limit)
    {
        return await _serviceRepository.ListAsync(offset, limit);
    }

    public async Task<Either<DomainException, Service>> AddAsync(ServiceCreateDto serviceDto)
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

    public async Task<Either<DomainException, Service>> UpdateAsync(Guid serviceId, ServiceEditDto serviceDto) =>
        await GetByIdAsync(serviceId).BindAsync<DomainException, Service, Service>(async service =>
            {
                if ((serviceDto.Price is not null && !IsPriceValid(serviceDto.Price))
                    || (serviceDto.Duration is not null && !IsDurationValid(serviceDto.Duration)))
                {
                    throw new ValidationException("The given service edit is invalid.");
                }

                service.Name = serviceDto.Name ?? service.Name;
                service.Duration = serviceDto.Duration ?? service.Duration;
                service.Price = serviceDto.Price ?? service.Price;
                service.Employees = serviceDto.Employees ?? service.Employees;
                
                return await _serviceRepository.Update(service);
            });

    public async Task Delete(Guid serviceId)
    {
        await _serviceRepository.Delete(serviceId);
    }

    public async Task<Either<DomainException, IEnumerable<TimeSlot>>> GetAvailableTimeSlots(Guid serviceId, TimeSlot timePeriod)
    {
        var employees = await _serviceRepository.GetServiceEmployees(serviceId);

        if (employees == null)
        {
            return new NotFoundException("Employees in a service", serviceId);
        }

        var allAvailableTimeSlots = new List<TimeSlot>();

        foreach (var employee in employees)
        {
            var employeeAvailableTimeSlots = await _employeeService.GetAvailableTimeSlots(employee.Id, timePeriod);

            // Use Match to handle both cases
            employeeAvailableTimeSlots.Match(
                Right: timeSlots => allAvailableTimeSlots.AddRange(timeSlots),
                Left: _ => { } // Ignore the Left case
            );
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

    private static bool IsPriceValid(decimal? price)
    {
        return price >= 0;
    }

    private static bool IsDurationValid(TimeSpan? duration)
    {
        return duration?.TotalMinutes > 0;
    }
}
