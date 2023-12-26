namespace Contracts.DTOs.Employee;

public class EmployeeModelDto
{
    required public Guid Id { get; set; }
    required public TimeOnly WorkStartTime { get; set; }
    required public TimeOnly WorkEndTime { get; set; }
}