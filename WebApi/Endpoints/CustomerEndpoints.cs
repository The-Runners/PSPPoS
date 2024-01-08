using Contracts.DTOs;
using Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("customer").WithTags("Customers");

        group.MapGet(string.Empty, ListCustomersAsync);

        group.MapGet("{id}", (
            [FromServices] ICustomerService service,
            Guid id) => service
            .GetByIdAsync(id)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPost(string.Empty, async (
            [FromServices] ICustomerService service,
            CustomerCreateDto customerCreateDto) => await service
            .AddAsync(customerCreateDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPut("{id}", async (
            [FromServices] ICustomerService service,
            Guid id,
            CustomerUpdateDto customerUpdateDto) => await service
            .UpdateAsync(id, customerUpdateDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());
    }

    private static async Task<IResult> ListCustomersAsync(
        [FromServices] ICustomerService service,
        int offset = 0,
        int limit = 100)
    {
        var customers = await service.ListAsync(offset, limit);
        return Results.Ok(customers.Select(x => x.ToModelDto()));
    }
}