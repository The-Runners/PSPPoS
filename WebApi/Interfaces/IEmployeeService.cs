using Contracts.DTOs.Employee;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod);

    // Employee CRUD operations
    Task<Employee> Create(EmployeeCreateDto employeeDto);

    Task<Employee?> GetById(Guid employeeId);

    Task<Employee?> Edit(EmployeeEditDto employeeDto);

    Task Delete(Guid employeeId);
}
