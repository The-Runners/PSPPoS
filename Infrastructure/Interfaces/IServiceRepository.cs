using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IServiceRepository : IGenericRepository<Service>
{
    Task<TimeSpan> GetServiceDuration(Guid serviceId);
}
