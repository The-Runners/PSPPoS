using Contracts.DTOs.Service;

namespace Infrastructure.Interfaces;

public interface IServiceRepository
{
    ServiceModelDto GetById(Guid id);
    Task<IEnumerable<ServiceModelDto>> GetAll();
    Task Add(ServiceCreateDto service);
    Task Update(ServiceModelDto service);
    Task Delete(Guid id);
}