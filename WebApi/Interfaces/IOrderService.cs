using Contracts;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrder(OrderPostModel orderDto);

    Task<OrderViewModel> AddProductsToOrder(OrderPatchModel orderDto);
}