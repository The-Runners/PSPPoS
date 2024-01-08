using Contracts.DTOs;
using Domain.Models;

namespace Contracts.Extensions;

public static class CustomerExtensions
{
    public static CustomerModelDto ToModelDto(this Customer customer) => new()
    {
        Id = customer.Id,
        CreatedAt = customer.CreatedAt,
        LoyaltyDiscount = customer.LoyaltyDiscount,
    };
}