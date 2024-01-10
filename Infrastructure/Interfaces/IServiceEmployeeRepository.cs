using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IServiceEmployeeRepository : IGenericRepository<ServiceEmployee>
{
    Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByServiceId(Guid serviceId);

    Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByEmployeeId(Guid employeeId);

    Task<ServiceEmployee?> GetServiceEmployeeByCompoundKey(Guid serviceId, Guid employeeId);

    Task DeleteGivenServiceEmployee(ServiceEmployee serviceEmployee);
}