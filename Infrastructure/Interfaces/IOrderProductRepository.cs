using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IOrderProductRepository : IGenericRepository<OrderProduct>
{
    Task<IEnumerable<OrderProduct>?> GetAllProductsForOrderId(Guid orderId);
}