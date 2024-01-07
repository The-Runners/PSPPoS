namespace Contracts.DTOs;

public class OrderProductSingleDto
{
    public required Guid ProductId { get; init; }

    public required int Amount { get; init; }
}