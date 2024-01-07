namespace Contracts.DTOs.Product;

public class ProductCreateDto
{
    public required string Name { get; init; }

    public required decimal Price { get; init; }
}