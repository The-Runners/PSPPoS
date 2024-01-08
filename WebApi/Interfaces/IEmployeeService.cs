using Contracts.DTOs;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod);

    Task<Employee> Create(EmployeeCreateDto employeeDto);

    Task<Employee?> GetById(Guid employeeId);

    Task<Employee?> Edit(Guid employeeId, EmployeeEditDto employeeDto);

    Task Delete(Guid employeeId);
}
