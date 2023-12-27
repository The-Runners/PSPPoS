namespace Contracts.DTOs.Order;

public class OrderProductViewModel
{
    public Guid ProductId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int Amount { get; init; }
    public decimal UnitPrice { get; init; }
}