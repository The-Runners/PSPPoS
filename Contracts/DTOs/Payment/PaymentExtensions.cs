namespace Contracts.DTOs;

public static class PaymentModelExtensions
{
    public static PaymentModelDto ToViewModel(this Domain.Models.Payment payment) => new()
    {
        Id = payment.Id,
        Amount = payment.Amount,
        OrderId = payment.OrderId,
        Type = payment.Type,
    };
}