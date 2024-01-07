using Contracts.DTOs.Service;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid serviceId, TimeSlot timePeriod);

    // Service CRUD operations
    Task<Service?> GetServiceById(Guid serviceId);

    Task<Service> Create(ServiceCreateDto serviceDto);

    Task<Service?> Edit(ServiceEditDto serviceDto);

    Task Delete(Guid serviceId);
}
