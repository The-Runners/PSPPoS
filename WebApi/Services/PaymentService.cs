using Contracts.DTOs.Payment;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using WebApi.Interfaces;

namespace WebApi.Services;

public class PaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
    }

    public async ValueTask<Either<DomainException, Payment>> AddPaymentAsync(
        PaymentCreateDto model)
    {
        var order = await _orderRepository.GetById(model.OrderId);

        if (order is null)
            return new NotFoundException(nameof(Order), model.OrderId);

        if (order.Status is not OrderStatus.Ordered)
            return new ValidationException($"{nameof(Order)} status must be {nameof(OrderStatus.Ordered)} to make payments.");

        if (model.Amount <= 0)
            return new ValidationException($"{nameof(Payment)} amount must be positive.");

        var payments = await _paymentRepository.GetOrderPaymentsAsync(model.OrderId);
        var paidSum = payments.Select(x => x.Amount).DefaultIfEmpty(0).Sum();

        if (paidSum == order.Price)
            return new ValidationException("Order is already paid for.");

        if (order.Price - paidSum < model.Amount)
            return new ValidationException("Sum of payments has to equal order price.");

        var payment = new Payment()
        {
            Id = Guid.NewGuid(),
            Type = model.Type,
            Amount = model.Amount,
            OrderId = model.OrderId,
        };

        await _paymentRepository.Add(payment);

        return payment;
    }

    public async ValueTask<Either<DomainException, Payment>> GetPaymentAsync(Guid id)
    {
        var result = await _paymentRepository.GetById(id);
        return result is null ? new NotFoundException(nameof(Payment), id) : result;
    }
}
