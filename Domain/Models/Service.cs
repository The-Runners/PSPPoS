﻿namespace Domain.Models;

public class Service
{
    required public Guid Id { get; init; }

    required public string Name { get; set; }

    required public TimeOnly Duration { get; set; }

    required public decimal Price { get; set; }
}