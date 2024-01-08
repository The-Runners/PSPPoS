using Domain.Enums;

namespace Domain.Models;

public class Payment
{
    public required Guid Id { get; init; }

    public required Guid OrderId { get; init; }

    public required decimal Amount { get; init; }

    public required PaymentType Type { get; init; }
}