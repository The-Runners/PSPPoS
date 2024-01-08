namespace Contracts.DTOs;

public class ServiceEmployeeCreateDto
{
    public required Guid EmployeeId { get; set; }

    public required Guid ServiceId { get; set; }
}