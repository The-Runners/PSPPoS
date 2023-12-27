namespace Domain.Models;

public class Product
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required decimal Price { get; init; }
}
