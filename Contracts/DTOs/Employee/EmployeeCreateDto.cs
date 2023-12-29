namespace Contracts.DTOs.Employee;
public class EmployeeCreateDto
{
    required public TimeSpan StartTime { get; set; }
    required public TimeSpan EndTime { get; set; }
}