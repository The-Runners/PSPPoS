using Contracts.DTOs.Service;

namespace Infrastructure.Interfaces;

public interface IServiceRepository
{
    Task<ServiceModelDto?> GetServiceById(Guid id);

    Task<IEnumerable<ServiceModelDto?>> GetAllServices();

    Task AddService(ServiceCreateDto serviceDto);

    Task UpdateService(ServiceModelDto serviceDto);

    Task DeleteService(Guid id);
}