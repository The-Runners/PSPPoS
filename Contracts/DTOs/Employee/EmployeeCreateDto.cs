namespace Contracts.DTOs;

public class EmployeeCreateDto
{
    public required TimeOnly StartTime { get; set; }

    public required TimeOnly EndTime { get; set; }
}