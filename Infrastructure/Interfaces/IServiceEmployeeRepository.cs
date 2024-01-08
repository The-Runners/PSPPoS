using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IServiceEmployeeRepository : IGenericRepository<ServiceEmployee>
{
    Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByServiceId(Guid serviceId);
}