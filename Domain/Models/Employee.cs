namespace Domain.Models;

public class Employee
{
    public required Guid Id { get; set; }

    public required TimeSpan StartTime { get; set; }

    public required TimeSpan EndTime { get; set; }
    //TODO: add available services
}