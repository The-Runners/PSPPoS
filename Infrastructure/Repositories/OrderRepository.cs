using Domain.Models;

namespace Infrastructure.Repositories;
public class OrderRepository : GenericRepository<Order>
{
    public OrderRepository(AppDbContext context) : base(context) { }
}