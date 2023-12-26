using Contracts;
using Domain.Models;
using Infrastructure;

namespace WebApi.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("customer").WithTags("Customers");

        group.MapGet(string.Empty, (AppDbContext ctx, int offset = 0, int limit = 100) =>
        {
            var customerViewModels = ctx
                .Customers
                .Skip(offset)
                .Take(limit)
                .Select(c => c.ToViewModel());

            return Results.Ok(customerViewModels);
        });

        group.MapGet("{id}", (AppDbContext ctx, Guid id) =>
        {
            var result = ctx.Customers.FirstOrDefault(x => x.Id == id);

            if (result is null)
            {
                return Results.NotFound($"Customer with id: `{id}` does not exist.");
            }
            else
            {
                return Results.Ok(result.ToViewModel());
            }
        });

        group.MapPost(string.Empty, (AppDbContext ctx) =>
        {
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow,
                LoyaltyDiscount = 0
            };

            ctx.Customers.Add(customer);
            ctx.SaveChanges();

            return Results.Ok(customer.ToViewModel());
        });

        group.MapPut("{id}", (AppDbContext ctx, Guid id, CustomerPutModel customerPutModel) =>
        {
            var errors = new Dictionary<string, string[]>();

            var customer = ctx.Customers.FirstOrDefault(x => x.Id == id);

            if (customer is null)
            {
                errors["Customer Id error"] = [$"Customer with id {id} does not exist."];
            }

            if (customerPutModel.LoyaltyDiscount is < 0 or > 1)
            {
                errors["Customer Loyalty Discount error"] = ["Loyalty discount must be in range [0, 1]."];
            }

            if (errors.Any())
                return Results.ValidationProblem(errors);

            customer.LoyaltyDiscount = customerPutModel.LoyaltyDiscount;
            ctx.SaveChanges();

            return Results.Ok(customer.ToViewModel());
        });
    }
}
