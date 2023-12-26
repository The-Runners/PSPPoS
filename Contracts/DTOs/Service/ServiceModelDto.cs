namespace Contracts.DTOs.Service;

public class ServiceModelDto
{
    required public Guid Id { get; init; }
    required public string Name { get; set; }
    required public TimeOnly Duration { get; set; }
}