namespace Domain.Models;

public class Customer
{
    public required Guid Id { get; init; }

    public required decimal LoyaltyDiscount { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}