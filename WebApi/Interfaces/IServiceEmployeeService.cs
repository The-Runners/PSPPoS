using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IServiceEmployeeService
{
    Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByEmployeeId(Guid employeeId);

    Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByServiceId(Guid serviceId);

    Task<IEnumerable<ServiceEmployee>> ListAsync(int offset, int limit);

    Task<Either<DomainException, ServiceEmployee>> AddServiceEmployeeAsync(ServiceEmployeeCreateDto serviceEmployeeDto);

    Task<Either<DomainException, Unit>> Delete(Guid serviceId, Guid employeeId);
}