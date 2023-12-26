namespace Contracts.DTOs.Service;

public class ServiceCreateDto
{
    required public string Name { get; set; }

    required public TimeOnly Duration { get; set; }
}