namespace Contracts.DTOs;

public class CustomerModelDto
{
    public required Guid Id { get; init; }

    public required decimal LoyaltyDiscount { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }
}