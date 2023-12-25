using Domain.Enums;

namespace Domain.Models;

public class Order
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; init; }
    public OrderStatus Status { get; init; }
}
