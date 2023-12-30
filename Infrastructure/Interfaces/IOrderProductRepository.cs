using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IOrderProductRepository : IGenericRepository<OrderProduct>
{
    Task<IEnumerable<OrderProduct>?> GetAllProductsForOrderId(Guid orderId);

    Task<OrderProduct?> GetProductByOrderAndProductIds(Guid orderId, Guid productId);

    Task DeleteProductByOrderAndProductIds(Guid orderId, Guid productId);
}