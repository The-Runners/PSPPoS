using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceRepository
{
    private readonly DbSet<Service> _services;

    public ServiceRepository(AppDbContext context) : base(context)
    {
        _services = context.Set<Service>();
    }

    public async Task<TimeSpan> GetServiceDuration(Guid serviceId)
    {
        var service = await _services.FindAsync(serviceId);
        if (service is null)
        {
            throw new InvalidOperationException("Service was not found");
        }

        return service.Duration;
    }
}