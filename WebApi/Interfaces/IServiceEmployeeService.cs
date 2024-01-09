using Domain.Models;

namespace WebApi.Interfaces;

public interface IServiceEmployeeService
{
    Task<List<Employee>?> GetEmployeesByServiceId(Guid serviceId);
}