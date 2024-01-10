namespace Contracts.DTOs;

public class ProductForOrderDto
{
    public required Guid ProductId { get; init; }

    public required string Name { get; init; }

    public required int Amount { get; init; }

    public required decimal UnitPrice { get; init; }

    public required decimal Tax { get; init; }
}