using ModelsEmployee = Domain.Models.Employee;

namespace Contracts.DTOs.Service;

public class ServiceModelDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required TimeSpan Duration { get; set; }

    public required decimal Price { get; set; }

    public IEnumerable<ModelsEmployee>? Employees { get; set; }
}