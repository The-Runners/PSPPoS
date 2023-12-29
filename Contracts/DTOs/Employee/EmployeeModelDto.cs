namespace Contracts.DTOs.Employee;

public class EmployeeModelDto
{
    required public Guid Id { get; set; }

    required public TimeSpan StartTime { get; set; }
    required public TimeSpan EndTime { get; set; }
}