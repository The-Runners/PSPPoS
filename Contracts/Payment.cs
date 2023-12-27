using Contracts.DTOs.Payment;
using Domain.Models;

namespace Contracts;

public static class PaymentModelExtensions
{
    public static PaymentViewModel ToViewModel(this Payment payment) => new()
    {
        Id = payment.Id,
        Amount = payment.Amount,
        OrderId = payment.OrderId,
        Type = payment.Type,
    };
}