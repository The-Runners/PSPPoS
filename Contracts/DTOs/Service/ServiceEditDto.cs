namespace Contracts.DTOs;

public class ServiceEditDto
{
    public string? Name { get; init; }

    public TimeOnly? Duration { get; init; }

    public decimal? Price { get; init; }
}