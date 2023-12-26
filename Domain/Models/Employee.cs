namespace Domain.Models;

public class Employee
{
    required public Guid Id { get; set; }

    required public TimeOnly WorkStartTime { get; set; }

    required public TimeOnly WorkEndTime { get; set; }
}