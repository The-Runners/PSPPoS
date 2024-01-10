using Contracts.DTOs;
using Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class ServiceEmployeeEndpoints
{
    public static void MapServiceEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/serviceEmployee").WithTags("ServiceEmployees");

        group.MapGet(string.Empty, ListServiceEmployees);

        group.MapGet("/list-by-employeeId/{id}", ListServiceEmployeesByEmployeeId);

        group.MapGet("/list-by-serviceId/{id}", ListServiceEmployeesByServiceId);

        group.MapPost("/assign-employee-to-service", async (
            [FromServices] IServiceEmployeeService serviceEmployeeService,
            ServiceEmployeeCreateDto serviceEmployeeDto) => await serviceEmployeeService
            .AddServiceEmployeeAsync(serviceEmployeeDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapDelete("/serviceId/{serviceId}/employeeId/{employeeId}", async (
            [FromServices] IServiceEmployeeService serviceEmployeeService,
            Guid serviceId,
            Guid employeeId) => await serviceEmployeeService
            .Delete(serviceId, employeeId)
            .ToHttpResult());
    }

    private static async Task<IResult> ListServiceEmployees(
        [FromServices] IServiceEmployeeService serviceEmployeeService,
        int offset = 0,
        int limit = 100)
    {
        var services = await serviceEmployeeService.ListAsync(offset, limit);
        return Results.Ok(services.Select(x => x.ToModelDto()));
    }

    private static async Task<IResult> ListServiceEmployeesByEmployeeId(
        [FromServices] IServiceEmployeeService serviceEmployeeService,
        Guid id,
        int offset = 0,
        int limit = 100)
    {
        var serviceEmployees = await serviceEmployeeService.GetServiceEmployeesByEmployeeId(id);
        return Results.Ok(serviceEmployees?.Select(x => x.ToModelDto()));
    }

    private static async Task<IResult> ListServiceEmployeesByServiceId(
        [FromServices] IServiceEmployeeService serviceEmployeeService,
        Guid id,
        int offset = 0,
        int limit = 100)
    {
        var serviceEmployees = await serviceEmployeeService.GetServiceEmployeesByServiceId(id);
        return Results.Ok(serviceEmployees?.Select(x => x.ToModelDto()));
    }
}
