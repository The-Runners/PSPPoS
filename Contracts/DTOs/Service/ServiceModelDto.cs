﻿namespace Contracts.DTOs;

public class ServiceModelDto
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public required TimeOnly Duration { get; set; }

    public required decimal Price { get; set; }
}