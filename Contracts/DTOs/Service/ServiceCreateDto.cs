using Domain.Models;

namespace Contracts.DTOs;

public class ServiceCreateDto
{
    public required string Name { get; set; }

    public required TimeSpan Duration { get; set; }

    public required decimal Price { get; set; }

    public IEnumerable<Employee>? Employees { get; set; }
}