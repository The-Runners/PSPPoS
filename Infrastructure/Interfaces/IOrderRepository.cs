using Domain.Filters;
using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>?> GetFilteredOrders(OrderFilter filter);
}
