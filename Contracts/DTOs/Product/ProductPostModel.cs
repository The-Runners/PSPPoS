namespace Contracts.DTOs.Product;

public class ProductPostModel
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
}