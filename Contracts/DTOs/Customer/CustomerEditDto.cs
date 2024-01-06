namespace Contracts.DTOs.Customer;

public class CustomerEditDto
{
    public required Guid Id { get; init; }
    public decimal LoyaltyDiscount { get; init; }
}