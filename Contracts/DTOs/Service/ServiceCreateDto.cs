namespace Contracts.DTOs;

public class ServiceCreateDto
{
    public required string Name { get; set; }

    public required TimeOnly Duration { get; set; }

    public required decimal Price { get; set; }
}