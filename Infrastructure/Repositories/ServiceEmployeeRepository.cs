using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceEmployeeRepository : GenericRepository<ServiceEmployee>, IServiceEmployeeRepository
{
    private readonly DbSet<ServiceEmployee> _serviceEmployees;

    public ServiceEmployeeRepository(AppDbContext context) : base(context)
    {
        _serviceEmployees = context.Set<ServiceEmployee>();
    }

    public async Task<IEnumerable<ServiceEmployee>?> GetServiceEmployeesByServiceId(Guid serviceId)
    {
        var serviceEmployees = await _serviceEmployees.ToListAsync();
        return serviceEmployees.Where(x => x.ServiceId == serviceId).ToList();
    }
}