﻿namespace Domain.Models;

public class Service
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public required TimeSpan Duration { get; set; }

    public required decimal Price { get; set; }
}