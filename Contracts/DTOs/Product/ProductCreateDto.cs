namespace Contracts.DTOs.Product;

public class ProductCreateDto // Need to create ProductService to add products
{
    public required string Name { get; init; }

    public required decimal Price { get; init; }
}