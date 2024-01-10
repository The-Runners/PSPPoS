using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderProductRepository : GenericRepository<OrderProduct>, IOrderProductRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<OrderProduct> _orderProducts;

    public OrderProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _orderProducts = _context.Set<OrderProduct>();
    }

    public async Task<IEnumerable<OrderProduct>?> GetAllProductsForOrderId(Guid orderId)
    {
        var orderProducts = await _orderProducts.ToListAsync();
        return orderProducts.Where(o => o.OrderId == orderId);
    }

    public async Task<OrderProduct?> GetProductByOrderAndProductIds(Guid orderId, Guid productId)
    {
        return await _orderProducts.FindAsync(orderId, productId);
    }

    public async Task DeleteProductByOrderAndProductIds(Guid orderId, Guid productId)
    {
        var orderProduct = await _orderProducts.FindAsync(orderId, productId);
        if (orderProduct is null)
        {
            return;
        }

        _orderProducts.Remove(orderProduct);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveOrderProductsAsync(Guid orderId) =>
        await _orderProducts
        .Where(x => x.OrderId == orderId)
        .ExecuteDeleteAsync();
}