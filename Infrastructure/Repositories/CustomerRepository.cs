using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>
{
    private readonly DbSet<Customer> _customers;

    public CustomerRepository(AppDbContext context) : base(context)
    {
        _customers = context.Set<Customer>();
    }
}