namespace Domain.Models;

public class Service
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public required TimeSpan Duration { get; set; }

    public required decimal Price { get; set; }

    /* Employees field is nullable because bussiness logic -
     * when employee exits job, the service description is kept
     * until a new employee is found*/
    public IEnumerable<Employee>? Employees { get; set; }
}