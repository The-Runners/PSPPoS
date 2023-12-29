using Contracts.DTOs.Employee;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IEmployeeService
{
    Task<Employee> Create(EmployeeCreateDto employeeDto);

    Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid employeeId, TimeSlot timePeriod);
}
