using Contracts.DTOs.Service;

namespace Infrastructure.Interfaces;

public interface IServiceRepository
{
    Task<ServiceModelDto?> GetById(Guid id);
    IEnumerable<ServiceModelDto?> GetAll();
    Task Add(ServiceCreateDto serviceDto);
    Task Update(ServiceModelDto serviceDto);
    Task Delete(Guid id);
}