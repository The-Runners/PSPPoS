using Contracts.DTOs.Employee;

namespace Infrastructure.Interfaces;

public interface IEmployeeRepository
{
    Task<EmployeeModelDto?> GetEmployeeById(Guid id);

    Task<IEnumerable<EmployeeModelDto?>> GetAllEmployees();

    Task AddEmployee(EmployeeCreateDto employeeDto);

    Task UpdateEmployee(EmployeeModelDto employeeDto);

    Task DeleteEmployee(Guid id);
}