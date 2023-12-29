namespace Contracts.DTOs.Order;

public class OrderViewModel
{
    public required Guid Id { get; init; }
    public Guid? CustomerId { get; init; }
    public required Guid EmployeeId { get; init; }
    //public OrderStatus Status { get; init; }
    public decimal Discount { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal Tip { get; init; }
    public required IEnumerable<ProductForOrderDto> OrderProducts { get; init; }
}