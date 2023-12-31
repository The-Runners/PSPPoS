using Domain.Filters;
using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<List<Order>?> GetFilteredOrders(OrderFilter filter);
}
