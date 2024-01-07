namespace Contracts.DTOs.Customer;

public class CustomerEditDto
{
    public required Guid Id { get; init; }

    // It is required only because Customer has only this one field to be updated
    public required decimal LoyaltyDiscount { get; init; }
}