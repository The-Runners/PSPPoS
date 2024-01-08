using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ServiceEmployeeService : IServiceEmployeeService
{
    private readonly IServiceEmployeeRepository _serviceEmployeeRepository;
    private readonly IGenericRepository<Employee> _employeeRepository;

    public ServiceEmployeeService(
        IServiceEmployeeRepository serviceEmployeeRepository,
        IGenericRepository<Employee> employeeRepository)
    {
        _serviceEmployeeRepository = serviceEmployeeRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Either<DomainException, List<Employee>>> GetEmployeesByServiceId(Guid serviceId)
    {
        var filteredServiceEmployees = await _serviceEmployeeRepository
            .GetServiceEmployeesByServiceId(serviceId);
        if (filteredServiceEmployees is null)
        {
            return new NotFoundException($"{nameof(filteredServiceEmployees)} with serviceId", serviceId);
        }

        var employees = new List<Employee>();
        foreach (var serviceEmployee in filteredServiceEmployees)
        {
            var employee = await _employeeRepository.GetById(serviceEmployee.EmployeeId);
            if (employee is not null)
            {
                employees.Add(employee);
            }
        }

        return employees;
    }
}