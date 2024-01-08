namespace Domain.Models;

public class ServiceEmployee
{
    public required Guid EmployeeId { get; init; }

    public required Guid ServiceId { get; init; }
}