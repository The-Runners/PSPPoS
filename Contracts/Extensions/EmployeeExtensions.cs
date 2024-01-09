using Contracts.DTOs;
using Domain.Models;

namespace Contracts.Extensions;

public static class EmployeeExtensions
{
    public static EmployeeModelDto ToModelDto(this Employee employee) => new()
    {
        Id = employee.Id,
        StartTime = TimeOnly.FromTimeSpan(employee.StartTime),
        EndTime = TimeOnly.FromTimeSpan(employee.EndTime),
    };
}