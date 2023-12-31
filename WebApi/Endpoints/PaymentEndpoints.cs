using Contracts.DTOs.Payment;
using Domain.Enums;
using Domain.Models;
using Infrastructure;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/payment").WithTags("Payments");

        group.MapGet("{id}", async (IPaymentService paymentService, Guid id) =>
        {
            var result = await paymentService.GetPaymentAsync(id);
            return result.Map(x => x.ToViewModel()).ToHttpResult();
        });

        group.MapPost(string.Empty, async (IPaymentService paymentService, PaymentCreateDto paymentPostModel) =>
        {
            var result = await paymentService.AddPaymentAsync(paymentPostModel);
            return result.Map(x => x.ToViewModel()).ToHttpResult();
        });
    }
}
