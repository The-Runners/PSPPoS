using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository(AppDbContext dbContext)
: GenericRepository<Payment>(dbContext)
, IPaymentRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async ValueTask<IEnumerable<Payment>> GetOrderPaymentsAsync(Guid OrderId) =>
        await _dbContext
            .Payments
            .Where(x => x.OrderId == OrderId)
            .ToListAsync();
}