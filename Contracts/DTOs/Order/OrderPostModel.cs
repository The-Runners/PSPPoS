namespace Contracts.DTOs.Order;

public class OrderPostModel
{
    public Guid? CustomerId { get; init; }

    public required Guid EmployeeId { get; set; }
}