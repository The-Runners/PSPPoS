namespace Contracts.DTOs;

public class ServiceEditDto
{
    public string? Name { get; init; }

    public TimeSpan? Duration { get; init; }

    public decimal? Price { get; init; }

    // Do we need this, or can we somehow extract it somewhere else
    public IEnumerable<Domain.Models.Employee>? Employees { get; init; }
}