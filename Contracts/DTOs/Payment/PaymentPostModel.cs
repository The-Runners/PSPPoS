using Domain.Enums;

namespace Contracts.DTOs.Payment;

public class PaymentPostModel
{
    public Guid OrderId { get; init; }
    public decimal Amount { get; init; }
    public PaymentType Type { get; init; }
}