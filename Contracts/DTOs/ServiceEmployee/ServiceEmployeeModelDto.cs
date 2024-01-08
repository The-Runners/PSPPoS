namespace Contracts.DTOs;

public class ServiceEmployeeModelDto
{
    public required Guid EmployeeId { get; init; }

    public required Guid ServiceId { get; init; }
}