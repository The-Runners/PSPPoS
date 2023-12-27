namespace Contracts.DTOs.Customer;

public class CustomerViewModel
{
    public Guid Id { get; init; }
    public decimal LoyaltyDiscount { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}