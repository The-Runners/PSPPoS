using Domain.Enums;

namespace Contracts.DTOs.Order;

public class OrderFinalDto
{
    public required Guid OrderId { get; init; }
    public Guid? CustomerId { get; init; }
    public required Guid EmployeeId { get; init; }
    public required OrderStatus Status { get; init; }
    public required decimal TotalPrice { get; init; }
    public required decimal Discount { get; init; }
    public required decimal Tip { get; init; }
    public List<ProductForOrderDto>? OrderProducts { get; init; }
    public Guid? ReservationId { get; init; }
    public Guid? ServiceId { get; init; }
    public DateTime? TimeSlot { get; init; }
    public TimeSpan? Duration { get; init; }
    public string? Name { get; init; }
}