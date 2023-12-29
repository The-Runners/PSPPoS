using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IServiceRepository : IGenericRepository<Service>
{
    public Task<TimeSpan> GetServiceDuration(Guid serviceId);
}
