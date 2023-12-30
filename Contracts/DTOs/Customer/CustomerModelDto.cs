namespace Contracts.DTOs.Customer;

public class CustomerModelDto // Need to create CustomerService to add customers
{
    public Guid Id { get; init; }

    public decimal LoyaltyDiscount { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}