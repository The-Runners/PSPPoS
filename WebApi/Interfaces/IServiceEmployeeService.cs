using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IServiceEmployeeService
{
    Task<Either<DomainException, List<Employee>>> GetEmployeesByServiceId(Guid serviceId);
}