using Contracts.DTOs.Service;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid serviceId, TimeSlot timePeriod);

    public Task<Service?> Get(Guid serviceId);

    public Task<Service> Create(ServiceCreateDto serviceDto);

    public Task<Service> Update(ServiceModelDto serviceDto);

    public Task Delete(Guid serviceId);
}
