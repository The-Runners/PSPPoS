using Domain.Models;

namespace Contracts;

public class CustomerViewModel
{
    public Guid Id { get; init; }
    public decimal LoyaltyDiscount { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}

public class CustomerPutModel
{
    public decimal LoyaltyDiscount { get; init; }
}

public static class CustomerExtensions
{
    public static CustomerViewModel ToViewModel(this Customer customer) => new()
    {
        Id = customer.Id,
        CreatedAt = customer.CreatedAt,
        LoyaltyDiscount = customer.LoyaltyDiscount,
    };
}
