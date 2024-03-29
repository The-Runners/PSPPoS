﻿using Contracts.DTOs;
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
    private readonly IOrderService _orderService;

    public PaymentService(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        IOrderService orderService)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _orderService = orderService;
    }

    public async Task<Either<DomainException, Payment>> AddPaymentAsync(PaymentCreateDto model) =>
        await ValidatePaymentCreateDtoAsync(model).MatchAsync(
            error => Task.FromResult(Either<DomainException, Payment>.Left(error)),
            async () =>
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    Type = model.Type,
                    Amount = model.Amount,
                    OrderId = model.OrderId,
                };

                await _paymentRepository.Add(payment);

                return Either<DomainException, Payment>.Right(payment);
            });

    public async ValueTask<Either<DomainException, Payment>> GetPaymentAsync(Guid id)
    {
        var result = await _paymentRepository.GetById(id);
        return result is null ? new NotFoundException(nameof(Payment), id) : result;
    }

    private async Task<Option<DomainException>> ValidatePaymentCreateDtoAsync(PaymentCreateDto model)
    {
        var order = await _orderRepository.GetById(model.OrderId);

        if (order is null)
            return new NotFoundException(nameof(Order), model.OrderId);

        if (order.Status is not OrderStatus.Ordered)
            return new ValidationException($"{nameof(Order)} status must be {nameof(OrderStatus.Ordered)} to make payments.");

        if (model.Amount <= 0)
            return new ValidationException($"{nameof(Payment)} amount must be positive.");

        var paidAmount = await GetPaidAmountAsync(model.OrderId);

        var orderPrice = await _orderService.CalculateOrderPrice(order);
        if (paidAmount == orderPrice)
            return new ValidationException("Order is already paid for.");

        if (orderPrice - paidAmount < model.Amount)
            return new ValidationException("Sum of payments has to equal order price.");

        return Option<DomainException>.None;
    }

    private async ValueTask<decimal> GetPaidAmountAsync(Guid orderId)
    {
        var payments = await _paymentRepository.GetOrderPaymentsAsync(orderId);
        return payments.Select(x => x.Amount).DefaultIfEmpty(0).Sum();
    }
}
