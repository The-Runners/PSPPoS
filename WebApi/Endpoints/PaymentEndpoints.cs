using Contracts;
using Domain;
using Infrastructure;

namespace WebApi.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/payment").WithTags("Payments");

        group.MapGet("{id}", (AppDbContext ctx, Guid id) =>
        {
            var result = ctx.Payments.FirstOrDefault(x => x.Id == id);

            if (result is null)
            {
                return Results.NotFound($"Payment with id: `{id}` does not exist.");
            }
            else
            {
                return Results.Ok(result.ToViewModel());
            }

        });

        group.MapPost(string.Empty, (AppDbContext ctx, PaymentPostModel paymentPostModel) =>
        {
            if (paymentPostModel.Amount <= 0)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>()
                {
                    { "Payment amount error", [ "Payment amount must be positive." ] }
                });
            }

            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
                Type = paymentPostModel.Type,
                Amount = paymentPostModel.Amount,
                OrderId = paymentPostModel.OrderId,
            };

            ctx.Payments.Add(payment);

            ctx.SaveChanges();

            return Results.Ok(payment.ToViewModel());
        });

    }
}
