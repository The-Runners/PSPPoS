using Domain.Enums;

namespace Contracts.DTOs.Order;

public class OrderEditDto
{
    public required Guid Id { get; init; }

    public Guid? CustomerId { get; set; }

    public Guid? EmployeeId { get; set; }

    public OrderStatus? Status { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Tip { get; set; }
}