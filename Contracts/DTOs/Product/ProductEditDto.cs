namespace Contracts.DTOs;

public class ProductEditDto
{
    public required Guid Id { get; init; }

    public string? Name { get; init; }

    public decimal? Price { get; init; }
}