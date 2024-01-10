using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceEmployeeRepository : GenericRepository<ServiceEmployee>, IServiceEmployeeRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<ServiceEmployee> _serviceEmployees;

    public ServiceEmployeeRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _serviceEmployees = _context.Set<ServiceEmployee>();
    }

    public async Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByServiceId(Guid serviceId)
    {
        var serviceEmployees = await _serviceEmployees.ToListAsync();
        return serviceEmployees.Where(x => x.ServiceId == serviceId).ToList();
    }

    public async Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByEmployeeId(Guid employeeId)
    {
        var serviceEmployees = await _serviceEmployees.ToListAsync();
        return serviceEmployees.Where(x => x.ServiceId == employeeId).ToList();
    }

    public async Task<ServiceEmployee?> GetServiceEmployeeByCompoundKey(Guid serviceId, Guid employeeId)
    {
        return await _serviceEmployees.FindAsync(serviceId, employeeId);
    }

    public async Task DeleteGivenServiceEmployee(ServiceEmployee serviceEmployee)
    {
        _serviceEmployees.Remove(serviceEmployee);
        await _context.SaveChangesAsync();
    }
}