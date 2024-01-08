using Contracts.DTOs;
using Contracts.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/service").WithTags("Services");

        group.MapGet(string.Empty, ListServices);

        group.MapGet("{id}", (
            [FromServices] IServiceService serviceService,
            Guid id) => serviceService
            .GetByIdAsync(id)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPost(string.Empty, async (
            [FromServices] IServiceService serviceService,
            ServiceCreateDto serviceDto) => await serviceService
            .AddAsync(serviceDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPut("{id}", async (
            [FromServices] IServiceService serviceService,
            Guid id,
            ServiceEditDto serviceDto) => await serviceService
            .UpdateAsync(id, serviceDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapGet("{id}/show-available-times", async (
            [FromServices] IServiceService serviceService,
            [FromBody] TimeSlot timePeriod,
            Guid id) => await serviceService
            .GetAvailableTimeSlots(id, timePeriod)
            .ToHttpResult());
    }

    private static async Task<IResult> ListServices(
        [FromServices] IServiceService serviceService,
        int offset = 0,
        int limit = 100)
    {
        var services = await serviceService.ListAsync(offset, limit);
        return Results.Ok(services.Select(x => x.ToModelDto()));
    }
}