using Contracts.DTOs;
using Contracts.DTOs.Order;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAsync(int offset, int limit);

    Task AddProductsToOrder(OrderProductsDto products);

    Task RemoveProductsFromOrder(OrderProductsDto products);

    Task<decimal> CalculateOrderPrice(Order order);

    Task<Order?> GetByIdAsync(Guid id);

    Task<Either<DomainException, Order>> GetById(Guid orderId);

    Task<OrderFinalDto> GenerateFinalOrderModel(Order order);

    Task<Order> CreateEmptyOrder(EmptyOrderCreateDto orderDto);

    Task<Either<DomainException, Order>> Edit(Guid orderId, OrderEditDto orderDto);

    Task<OrderFinalDto> UpdateOrderAsync(Guid id, OrderUpdateDto updateDto);
}