using System;
using ModelsEmployee = Domain.Models.Employee;

namespace Contracts.DTOs.Service;

public class ServiceCreateDto // Need to create ServiceService to add services
{
    public required string Name { get; set; }

    public required TimeSpan Duration { get; set; }

    public required decimal Price { get; set; }

    public IEnumerable<ModelsEmployee>? Employees { get; set; }
}