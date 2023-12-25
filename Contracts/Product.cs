using Domain.Models;

namespace Contracts;

public class ProductPostModel
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
}

public class ProductViewModel
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
}

public static class ProductExtensions
{
    public static ProductViewModel ToViewModel(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
    };
}
