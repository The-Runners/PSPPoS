namespace Domain.Models;

public class Employee
{
    public Guid Id { get; set; }
    public TimeOnly WorkStartTime { get; set; }
    public TimeOnly WorkEndTime { get; set; }
}