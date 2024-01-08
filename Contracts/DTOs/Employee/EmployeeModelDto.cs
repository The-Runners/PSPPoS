namespace Contracts.DTOs;

public class EmployeeModelDto
{
    public required Guid Id { get; set; }

    public required TimeSpan StartTime { get; set; }

    public required TimeSpan EndTime { get; set; }
}