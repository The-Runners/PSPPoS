namespace Domain.Models;

public class Employee
{
    public required Guid Id { get; set; }

    public required TimeOnly WorkStartTime { get; set; }

    public required TimeOnly WorkEndTime { get; set; }
}