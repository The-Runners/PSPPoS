namespace Contracts.DTOs.Order;

public class ProductForOrderDto
{
    public Guid ProductId { get; init; }
    public required string Name { get; init; }
    public int Amount { get; init; }
    public decimal UnitPrice { get; init; }
}