using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IEmployeeService
{
    Task<Either<DomainException, IEnumerable<TimeSlot>>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod);

    Task<IEnumerable<Employee>> ListAsync(int offset, int limit);

    Task<Either<DomainException, Employee>> Create(EmployeeCreateDto employeeDto);

    Task<Either<DomainException, Employee>> GetById(Guid employeeId);

    Task<Either<DomainException, Employee>> Edit(Guid employeeId, EmployeeEditDto employeeDto);

    Task Delete(Guid employeeId);
}
