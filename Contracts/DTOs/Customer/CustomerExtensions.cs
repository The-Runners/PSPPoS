namespace Contracts.DTOs.Customer;

public static class CustomerExtensions
{
    public static CustomerModelDto ToModelDto(this Domain.Models.Customer customer) => new()
    {
        Id = customer.Id,
        CreatedAt = customer.CreatedAt,
        LoyaltyDiscount = customer.LoyaltyDiscount,
    };
}