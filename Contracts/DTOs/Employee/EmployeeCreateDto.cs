namespace Contracts.DTOs.Employee;

public class EmployeeCreateDto
{
    required public TimeOnly WorkStartTime { get; set; }

    required public TimeOnly WorkEndTime { get; set; }
}