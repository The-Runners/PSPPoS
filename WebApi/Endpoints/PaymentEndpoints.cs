using Contracts;
using Contracts.DTOs.Payment;
using Domain.Enums;
using Domain.Models;
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
            var errors = new Dictionary<string, string[]>();

            if (paymentPostModel.Amount <= 0)
                errors["Payment amount error"] = ["Payment amount must be positive."];

            var order = ctx.Orders.FirstOrDefault(x => x.Id == paymentPostModel.OrderId);

            if (order is null)
                errors["Order order"] = [$"Order with id {paymentPostModel.OrderId} does not exist"];

            var payments = ctx.Payments.Where(x => x.OrderId == order.Id).ToList();

            var paidSum = payments.Select(x => x.Amount).DefaultIfEmpty(0).Sum();

            if (paidSum == order.Price)
                errors["Order paid for error"] = ["Order is already paid for"];

            if (order.Price - paidSum < paymentPostModel.Amount)
                errors["Invalid payment amount error"] = ["Sum of payment amounts has to be equal to the order price."];

            if (errors.Any())
                return Results.ValidationProblem(errors);

            if (paidSum is 0 && paymentPostModel.Amount != order.Price)
            {
                order.Status = OrderStatus.PartiallyPaid;
            }

            else if (paymentPostModel.Amount + paidSum == order.Price)
            {
                order.Status = OrderStatus.Paid;
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
