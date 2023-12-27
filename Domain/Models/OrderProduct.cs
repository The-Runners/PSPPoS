namespace Domain.Models;

public class OrderProduct
{
    public required Guid ProductId { get; init; }

    public required Guid OrderId { get; init; }

    public required int Amount { get; set; }
}
