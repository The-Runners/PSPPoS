using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IOrderService
{
    Task AddProductsToOrder(OrderProductsDto products);

    Task RemoveProductsFromOrder(OrderProductsDto products);

    Task<decimal> CalculateOrderPrice(Order order);

    Task<OrderFinalDto> GenerateFinalOrderModel(Order order);

    Task<Order> CreateEmptyOrder(EmptyOrderCreateDto orderDto);

    Task<Either<DomainException, Order>> Edit(Guid orderId, OrderEditDto orderDto);
}