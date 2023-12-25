namespace Domain.Models;

public class Customer
{
    public Guid Id { get; init; }
    public decimal LoyaltyDiscount { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
}