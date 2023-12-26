namespace Domain.Models;

public class OrderProduct
{
    public Guid ProductId { get; init; }

    public Guid OrderId { get; init; }

    public int Amount { get; set; }
}
