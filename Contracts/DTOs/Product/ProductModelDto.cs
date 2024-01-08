namespace Contracts.DTOs;

public class ProductModelDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required decimal Price { get; init; }
}