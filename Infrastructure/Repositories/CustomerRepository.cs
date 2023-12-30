using Domain.Models;

namespace Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>
{
    private readonly AppDbContext _context;
    private readonly DbSet<Customer> _customers;

    public CustomerRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _customers = _context.Set<Customer>();
    }

    public async Task<Customer?> UpdateCustomerLoyalty(Guid customerId, decimal loyalty)
    {
        var customer = await _customers.FindAsync(customerId);
        if (customer == null)
        {
            return;
        }

        customer.Loyalty = loyalty;

        _customers.Update(customer);

        await _context.SaveChangesAsync();

        return customer;
    }
}