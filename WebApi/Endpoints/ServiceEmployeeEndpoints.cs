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
        var group = app.MapGroup("/ServiceEmployee").WithTags("ServiceEmployees");

        group.MapGet(string.Empty, ListServiceEmployees);

        //todo: MOVE TO EMPLOYEE
        group.MapGet("/list-by-serviceId", ListServiceEmployeesByServiceId);

        //TODO: fix REUTRN SERVICE
        group.MapGet("/list-by-employeeId", ListServiceEmployeesByEmployeeId);

        group.MapPost("/add-service-employee", async (
            [FromServices] IServiceEmployeeService serviceEmployeeService,
            ServiceEmployeeCreateDto serviceEmployeeDto) => await serviceEmployeeService
            .AddServiceEmployeeAsync(serviceEmployeeDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapDelete("{serviceId}/{employeeId}", async (
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

    private static async Task<IResult> ListServiceEmployeesByServiceId(
        [FromServices] IServiceEmployeeService serviceEmployeeService,
        Guid serviceId,
        int offset = 0,
        int limit = 100)
    {
        var serviceEmployees = await serviceEmployeeService.GetEmployeesByServiceId(serviceId);
        return Results.Ok(serviceEmployees?.Select(x => x.ToModelDto()));
    }

    private static async Task<IResult> ListServiceEmployeesByEmployeeId(
        [FromServices] IServiceEmployeeService serviceEmployeeService,
        Guid employeeId,
        int offset = 0,
        int limit = 100)
    {
        var serviceEmployees = await serviceEmployeeService.GetEmployeesByEmployeeId(employeeId);
        return Results.Ok(serviceEmployees?.Select(x => x.ToModelDto()));
    }
}
