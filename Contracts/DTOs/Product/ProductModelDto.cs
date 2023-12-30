namespace Contracts.DTOs.Product;

public class ProductModelDto // Need to create ProductService to view products
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required decimal Price { get; init; }
}