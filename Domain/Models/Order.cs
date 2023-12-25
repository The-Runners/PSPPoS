using Domain.Enums;

namespace Domain.Models;

public class Order
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; set; }
    public OrderStatus Status { get; init; }
    public decimal Discount { get; set; }
    public decimal Price { get; set; }
}
