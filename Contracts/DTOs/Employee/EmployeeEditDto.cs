namespace Contracts.DTOs;

public class EmployeeEditDto
{
    public required Guid Id { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }
}