using Domain.Models;

namespace Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>
{
    public ProductRepository(AppDbContext context) : base(context) { }
}