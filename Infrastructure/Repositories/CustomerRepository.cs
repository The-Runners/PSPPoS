using Domain.Models;

namespace Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>
{
    public CustomerRepository(AppDbContext context) : base(context) { }
}