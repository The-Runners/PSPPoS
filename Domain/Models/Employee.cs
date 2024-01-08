namespace Domain.Models;

public class Employee
{
    public required Guid Id { get; set; }

    public required TimeOnly StartTime { get; set; }

    public required TimeOnly EndTime { get; set; }
}