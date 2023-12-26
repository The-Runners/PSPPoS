using Domain.Enums;

namespace Domain.Models;

public class Order
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Discount { get; set; }
    public decimal Price { get; set; }
    public decimal Tip { get; set; }
}
