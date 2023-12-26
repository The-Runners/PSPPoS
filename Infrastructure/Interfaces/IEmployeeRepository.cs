using Contracts.DTOs.Employee;

namespace Infrastructure.Interfaces;

public interface IEmployeeRepository
{
    Task<EmployeeModelDto?> GetById(Guid id);
    IEnumerable<EmployeeModelDto?> GetAll();
    Task Add(EmployeeCreateDto employeeDto);
    Task Update(EmployeeModelDto employeeDto);
    Task Delete(Guid id);
}