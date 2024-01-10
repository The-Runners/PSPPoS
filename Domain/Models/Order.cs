using Domain.Enums;

namespace Domain.Models;

public class Order
{
    public required Guid Id { get; init; }

    public Guid? CustomerId { get; set; }

    public required Guid EmployeeId { get; set; }

    public required OrderStatus Status { get; set; }

    public required decimal Price { get; set; }

    public required decimal Discount { get; set; }

    public required decimal Tip { get; set; }

    public DateTime CreatedAt { get; set; }
}
