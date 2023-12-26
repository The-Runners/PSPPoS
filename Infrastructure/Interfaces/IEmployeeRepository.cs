using Contracts.DTOs.Employee;

namespace Infrastructure.Interfaces;

public interface IEmployeeRepository
{
    EmployeeModelDto GetById(Guid id);
    Task<IEnumerable<EmployeeModelDto>> GetAll();
    Task Add(EmployeeCreateDto employee);
    Task Update(EmployeeModelDto employee);
}