using Domain.Enums;
using Domain.Filters;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly DbSet<Order> _orderRepository;

    public OrderRepository(AppDbContext context) : base(context)
    {
        _orderRepository = context.Set<Order>();
    }

    public async Task<List<Order>?> GetFilteredOrders(OrderFilter filter)
    {

        var orders = await _orderRepository.ToListAsync();

        if (filter.StartDate.HasValue)
        {
            orders = orders.Where(r => r.CreatedAt >= filter.StartDate.Value).ToList();
        }

        if (filter.EndDate.HasValue)
        {
            orders = orders.Where(r => r.CreatedAt <= filter.EndDate.Value).ToList();
        }

        if(filter.EmployeeId.HasValue)
        {
            orders = orders.Where(r => r.EmployeeId == filter.EmployeeId.Value).ToList();
        }

        if (filter.OrderStatuses != null && filter.OrderStatuses.Any())
        {
            foreach (OrderStatus status in filter.OrderStatuses) 
            {
                orders.RemoveAll(order => !filter.OrderStatuses.Contains(order.Status));
            }
        }

        return orders;
    }
}