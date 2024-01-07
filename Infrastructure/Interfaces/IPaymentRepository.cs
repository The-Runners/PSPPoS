using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    ValueTask<IEnumerable<Payment>> GetOrderPaymentsAsync(Guid OrderId);
}
