using Domain.Enums;

namespace Contracts.DTOs;

public class PaymentCreateDto
{
    public required Guid OrderId { get; init; }

    public required decimal Amount { get; init; }

    public required PaymentType Type { get; init; }
}