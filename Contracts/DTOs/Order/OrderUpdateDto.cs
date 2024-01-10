using Domain.Enums;

namespace Contracts.DTOs.Order;

public class OrderUpdateDto
{
    public Guid? CustomerId { get; init; }

    public required Guid EmployeeId { get; init; }

    public required OrderStatus Status { get; init; }

    public required decimal Discount { get; init; }

    public required decimal Tip { get; init; }

    public required IEnumerable<OrderProductSingleDto> Products { get; init; }
}