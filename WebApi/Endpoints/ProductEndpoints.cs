using Contracts.DTOs;
using Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("product").WithTags("Products");

        group.MapGet(string.Empty, ListProducts);

        group.MapGet("{id}", (
            [FromServices] IProductService productService,
            Guid id) => productService
            .GetProductById(id)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPost(string.Empty, async (
            [FromServices] IProductService productService,
            ProductCreateDto productDto) => await productService
            .Create(productDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPut("{id}", async (
            [FromServices] IProductService productService,
            Guid id,
            ProductEditDto productDto) => await productService
            .Edit(id, productDto)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapDelete("{id}", async (
            [FromServices] IProductService productService,
            Guid id) => await productService
            .Delete(id)
            .ToHttpResult());
    }

    private static async Task<IResult> ListProducts(
        [FromServices] IProductService productService,
        int offset = 0,
        int limit = 100)
    {
        var customers = await productService.ListAsync(offset, limit);
        return Results.Ok(customers.Select(x => x.ToModelDto()));
    }
}