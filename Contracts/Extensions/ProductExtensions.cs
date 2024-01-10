using Contracts.DTOs;
using Domain.Models;

namespace Contracts.Extensions;

public static class ProductExtensions
{
    public static ProductModelDto ToModelDto(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        Tax = product.Tax
    };
}