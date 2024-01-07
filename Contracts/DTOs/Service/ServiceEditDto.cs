namespace Contracts.DTOs;

public class ServiceEditDto
{
    public required Guid Id { get; init; }

    public string? Name { get; init; }

    public TimeSpan? Duration { get; init; }

    public decimal? Price { get; init; }

    public IEnumerable<Domain.Models.Employee>? Employees { get; init; }
}