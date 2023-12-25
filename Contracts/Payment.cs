using Domain;
using Domain.Enums;

namespace Contracts;

public class PaymentPostModel
{
    public Guid OrderId { get; init; }
    public decimal Amount { get; init; }
    public PaymentType Type { get; init; }
}

public class PaymentViewModel
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public decimal Amount { get; init; }
    public PaymentType Type { get; init; }
}

public static class PaymentModelExtensions
{
    public static PaymentViewModel ToViewModel(this Payment payment) => new()
    {
        Id = payment.Id,
        Amount = payment.Amount,
        OrderId = payment.OrderId,
        Type = payment.Type,
    };
}