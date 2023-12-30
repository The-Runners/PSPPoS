using Domain.Enums;

namespace Domain.Models;

public class Payment
{
    public Guid Id { get; init; }

    public Guid OrderId { get; init; }

    public decimal Amount { get; init; }

    public PaymentType Type { get; init; }
}