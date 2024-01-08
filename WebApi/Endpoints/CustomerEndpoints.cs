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
            [FromServices] ICustomerService customerService,
            Guid id) => customerService
            .GetByIdAsync(id)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPost(string.Empty, async (
            [FromServices] ICustomerService customerService,
            CustomerCreateDto customerCreateDto) => await customerService
            .AddAsync(customerCreateDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPut("{id}", async (
            [FromServices] ICustomerService customerService,
            Guid id,
            CustomerUpdateDto customerUpdateDto) => await customerService
            .UpdateAsync(id, customerUpdateDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapDelete("{id}", async (
            [FromServices] ICustomerService customerService,
            Guid id) => await customerService
            .Delete(id)
            .ToHttpResult());
    }

    private static async Task<IResult> ListCustomersAsync(
        [FromServices] ICustomerService customerService,
        int offset = 0,
        int limit = 100)
    {
        var customers = await customerService.ListAsync(offset, limit);
        return Results.Ok(customers.Select(x => x.ToModelDto()));
    }
}