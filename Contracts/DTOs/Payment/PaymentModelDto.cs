using Domain.Enums;

namespace Contracts.DTOs.Payment;

public class PaymentModelDto
{
    public Guid Id { get; init; }

    public Guid OrderId { get; init; }

    public decimal Amount { get; init; }

    public PaymentType Type { get; init; }
}