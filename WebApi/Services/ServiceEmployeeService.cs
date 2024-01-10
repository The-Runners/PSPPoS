using Contracts.DTOs;
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
    private readonly IServiceRepository _serviceRepository;

    public ServiceEmployeeService(
        IServiceEmployeeRepository serviceEmployeeRepository,
        IGenericRepository<Employee> employeeRepository,
        IServiceRepository serviceRepository)
    {
        _serviceEmployeeRepository = serviceEmployeeRepository;
        _employeeRepository = employeeRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByEmployeeId(Guid employeeId)
    {
        return await _serviceEmployeeRepository
            .GetServiceEmployeesByEmployeeId(employeeId);
    }

    public async Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByServiceId(Guid serviceId)
    {
        return await _serviceEmployeeRepository
            .GetServiceEmployeesByServiceId(serviceId);
    }

    public async Task<Either<DomainException, ServiceEmployee>> GetByCompoundKeyAsync(Guid serviceId, Guid employeeId)
    {
        var result = await _serviceEmployeeRepository
            .GetServiceEmployeeByCompoundKey(serviceId, employeeId);

        return result is null
            ? new NotFoundException(nameof(ServiceEmployee), new {serviceId, employeeId})
            : result;
    }

    public async Task<IEnumerable<ServiceEmployee>> ListAsync(int offset, int limit)
    {
        return await _serviceEmployeeRepository.ListAsync(offset, limit);
    }

    public async Task<Either<DomainException, ServiceEmployee>> AddServiceEmployeeAsync(ServiceEmployeeCreateDto serviceEmployeeDto)
    {
        var employee = await _employeeRepository.GetById(serviceEmployeeDto.EmployeeId);
        var service = await _serviceRepository.GetById(serviceEmployeeDto.ServiceId);
        if (employee is null)
        {
            return new NotFoundException(nameof(Employee), serviceEmployeeDto.EmployeeId);
        }
        if (service is null)
        {
            return new NotFoundException(nameof(Service), serviceEmployeeDto.ServiceId);
        }

        var serviceEmployee = new ServiceEmployee
        {
            EmployeeId = serviceEmployeeDto.EmployeeId,
            ServiceId = serviceEmployeeDto.ServiceId,
        };
        return await _serviceEmployeeRepository.Add(serviceEmployee);
    }

    public async Task<Either<DomainException, Unit>> Delete(Guid serviceId, Guid employeeId) =>
        await GetByCompoundKeyAsync(serviceId, employeeId)
            .MapAsync(async serviceEmployee => await _serviceEmployeeRepository.DeleteGivenServiceEmployee(serviceEmployee))
            .Map(_ => Unit.Default);

}