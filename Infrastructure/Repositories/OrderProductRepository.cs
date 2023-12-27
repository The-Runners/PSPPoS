using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderProductRepository : GenericRepository<OrderProduct>, IOrderProductRepository
{
    private readonly DbSet<OrderProduct> _orderProducts;

    public OrderProductRepository(AppDbContext context) : base(context)
    {
        _orderProducts = context.Set<OrderProduct>();
    }

    public async Task<IEnumerable<OrderProduct>?> GetAllProductsForOrderId(Guid orderId)
    {
        var orderProducts = await _orderProducts.ToListAsync();
        return orderProducts.Where(o => o.OrderId == orderId);
    }
}