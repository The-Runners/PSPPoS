namespace Contracts.DTOs;

public class EmployeeCreateDto
{
    public required TimeSpan StartTime { get; set; }

    public required TimeSpan EndTime { get; set; }
}