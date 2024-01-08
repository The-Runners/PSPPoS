using Contracts.DTOs;
using Contracts.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/employee").WithTags("Employees");

        group.MapGet(string.Empty, ListEmployees);

        group.MapGet("{id}", (
            [FromServices] IEmployeeService employeeService,
            Guid id) => employeeService
            .GetById(id)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPost(string.Empty, async (
            [FromServices] IEmployeeService employeeService,
            EmployeeCreateDto employeeDto) => await employeeService
            .Create(employeeDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPut("{id}", async (
            [FromServices] IEmployeeService employeeService,
            Guid id,
            EmployeeEditDto employeeDto) => await employeeService
            .Edit(id, employeeDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapDelete("{id}", async (
            [FromServices] IEmployeeService employeeService,
            Guid id) => await employeeService
            .Delete(id)
            .ToHttpResult());

        group.MapGet("{id}/show-available-times", async (
            [FromServices] IEmployeeService employeeService,
            [FromBody] TimeSlot timePeriod,
            Guid id) => await employeeService
            .GetAvailableTimeSlots(id, timePeriod)
            .ToHttpResult());
    }

    private static async Task<IResult> ListEmployees(
        [FromServices] IEmployeeService employeeService,
        int offset = 0,
        int limit = 100)
    {
        var employees = await employeeService.ListAsync(offset, limit);
        return Results.Ok(employees.Select(x => x.ToModelDto()));
    }
}