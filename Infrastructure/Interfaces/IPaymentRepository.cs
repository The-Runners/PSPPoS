using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interfaces;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    ValueTask<IEnumerable<Payment>> GetOrderPaymentsAsync(Guid OrderId);
}

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
