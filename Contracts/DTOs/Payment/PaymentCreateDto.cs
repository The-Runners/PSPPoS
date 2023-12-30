using Domain.Enums;

namespace Contracts.DTOs.Payment;

public class PaymentCreateDto
{
    public Guid OrderId { get; init; }

    public decimal Amount { get; init; }

    public PaymentType Type { get; init; }
}