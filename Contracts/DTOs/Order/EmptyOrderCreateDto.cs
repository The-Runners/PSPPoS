﻿namespace Contracts.DTOs;

public class EmptyOrderCreateDto
{
    public Guid? CustomerId { get; init; }

    public required Guid EmployeeId { get; set; }
}