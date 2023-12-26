namespace Contracts.DTOs.Service;

public class ServiceModelDto
{
    required public Guid Id { get; set; }

    required public string Name { get; set; }

    required public TimeOnly Duration { get; set; }
}