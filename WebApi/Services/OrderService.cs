using Contracts.DTOs;
using Contracts.DTOs.Order;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using WebApi.Interfaces;

namespace WebApi.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IOrderProductService _orderProductService;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderProductRepository orderProductRepository,
        IReservationRepository reservationRepository,
        IServiceRepository serviceRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Customer> customerRepository,
        IOrderProductService orderProductService)
    {
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
        _reservationRepository = reservationRepository;
        _serviceRepository = serviceRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _orderProductService = orderProductService;
    }

    public async Task<Order> CreateEmptyOrder(EmptyOrderCreateDto orderDto)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = orderDto.CustomerId,
            EmployeeId = orderDto.EmployeeId,
            Status = OrderStatus.Created,
            Price = 0,
            Discount = 0,
            Tip = 0,
            CreatedAt = DateTime.UtcNow,
        };

        return await _orderRepository.Add(order);
    }

    public async Task<IEnumerable<Order>> GetAsync(int offset, int limit) =>
        await _orderRepository.ListAsync(offset, limit);

    public async Task<OrderFinalDto> UpdateOrderAsync(Guid id, OrderUpdateDto updateDto)
    {
        var order = await _orderRepository.GetById(id);

        if (order is null)
            throw new NotFoundException(nameof(Order), id);

        order.Tip = updateDto.Tip;
        order.Discount = updateDto.Discount;
        order.Status = updateDto.Status;
        order.CustomerId = updateDto.CustomerId;
        order.EmployeeId = updateDto.EmployeeId;

        await _orderProductRepository.RemoveOrderProductsAsync(id);
        await _orderProductRepository.AddRangeAsync(updateDto.Products.Select(x => new OrderProduct
        {
            Amount = x.Amount,
            OrderId = id,
            ProductId = x.ProductId,
        }));

        return await GenerateFinalOrderModel(order);
    }

    public async Task AddProductsToOrder(OrderProductsDto products)
    {
        foreach (var orderProductDto in products.OrderProducts)
        {
            var orderProduct = new OrderProduct
            {
                OrderId = products.OrderId,
                ProductId = orderProductDto.ProductId,
                Amount = orderProductDto.Amount,
            };
            await _orderProductRepository.Add(orderProduct);
        }
    }

    public async Task RemoveProductsFromOrder(OrderProductsDto products)
    {
        foreach (var orderProductDto in products.OrderProducts)
        {
            var orderProductFromDb = await _orderProductRepository
                    .GetProductByOrderAndProductIds(products.OrderId, orderProductDto.ProductId);
            if (orderProductFromDb is null)
            {
                throw new NotFoundException(nameof(orderProductFromDb), orderProductDto.ProductId);
            }

            if (orderProductFromDb.Amount > orderProductDto.Amount)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = products.OrderId,
                    ProductId = orderProductFromDb.ProductId,
                    Amount = orderProductFromDb.Amount - orderProductDto.Amount,
                };
                await _orderProductRepository.Update(orderProduct);
            }
            else if (orderProductFromDb.Amount <= orderProductDto.Amount)
            {
                await _orderProductRepository
                    .DeleteProductByOrderAndProductIds(products.OrderId, orderProductFromDb.ProductId);
            }
        }
    }

    public async Task<Either<DomainException, Order>> GetById(Guid orderId)
    {
        var result = await _orderRepository.GetById(orderId);
        return result is null ? new NotFoundException(nameof(Order), orderId) : result;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _orderRepository.GetById(id);
    }

    public async Task<OrderFinalDto> GenerateFinalOrderModel(Order order)
    {
        var totalPrice = await CalculateOrderPrice(order);
        var orderProducts = await _orderProductService.GenerateProductViewModels(order.Id);
        var reservationServiceDto = await GenerateReservationServiceModel(order.Id);
        var finalOrderModel = new OrderFinalDto
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            EmployeeId = order.EmployeeId,
            Status = order.Status,
            TotalPrice = totalPrice,
            Discount = order.Discount,
            Tip = order.Tip,
            OrderProducts = orderProducts,
            ReservationId = reservationServiceDto.ReservationId ?? Guid.Empty,
            ServiceId = reservationServiceDto.ServiceId ?? Guid.Empty,
            Name = reservationServiceDto.Name,
            StartTime = reservationServiceDto.StartTime,
            Duration = reservationServiceDto.Duration,
        };

        return finalOrderModel;
    }

    public async Task<decimal> CalculateOrderPrice(Order order)
    {
        decimal price = 0;
        var orderProducts = await _orderProductRepository.GetAllProductsForOrderId(order.Id);
        if (orderProducts is not null)
        {
            price += await GetOrderProductsPrice(orderProducts, price);
        }

        var reservation = await _reservationRepository.GetReservationByOrderId(order.Id);
        if (reservation is not null)
        {
            price += await GetServicePrice(reservation.ServiceId, price);
        }

        var discount = await GetFinalOrderDiscount(order);
        price -= price * discount;

        price += order.Tip;

        return price;
    }

    public async Task<Either<DomainException, Order>> Edit(Guid orderId, OrderEditDto orderDto) =>
        await GetById(orderId).BindAsync<DomainException, Order, Order>(async order =>
        {
            if (order is null)
            {
                return new NotFoundException(nameof(Order), orderId);
            }

            order.CustomerId = orderDto.CustomerId ?? order.CustomerId;
            order.EmployeeId = orderDto.EmployeeId ?? order.EmployeeId;
            order.Status = orderDto.Status ?? order.Status;
            order.Price = orderDto.Price ?? order.Price;
            order.Discount = orderDto.Discount ?? order.Discount;
            order.Tip = orderDto.Tip ?? order.Tip;

            return await _orderRepository.Update(order);
        });


    private async Task<decimal> GetOrderProductsPrice(IEnumerable<OrderProduct> orderProducts, decimal price)
    {
        foreach (var orderProduct in orderProducts)
        {
            var product = await _productRepository.GetById(orderProduct.ProductId);
            if (product is null)
            {
                throw new NotFoundException(nameof(product), orderProduct.ProductId);
            }

            // TO-DO-MAYBE Add discount to specific order products
            price += (1 + product.Tax) * product.Price * orderProduct.Amount;

        }

        return price;
    }

    private async Task<decimal> GetServicePrice(Guid serviceId, decimal price)
    {
        var service = await _serviceRepository.GetById(serviceId);
        if (service is null)
        {
            return price;
        }

        // TO-DO-MAYBE Add discount to specific services
        return price + service.Price;
    }

    private async Task<decimal> GetFinalOrderDiscount(Order order)
    {
        if (order.CustomerId is null) return order.Discount;

        var customer = await _customerRepository.GetById(order.CustomerId.Value);
        return customer is not null ? customer.LoyaltyDiscount + order.Discount : order.Discount;
    }

    private async Task<ReservationServiceDto> GenerateReservationServiceModel(Guid orderId)
    {
        var reservationServiceDto = new ReservationServiceDto();
        var reservation = await _reservationRepository.GetReservationByOrderId(orderId);
        if (reservation is null)
        {
            return reservationServiceDto;
        }

        reservationServiceDto.ReservationId = reservation.Id;
        reservationServiceDto.StartTime = reservation.StartTime;
        var service = await _serviceRepository.GetById(reservation.ServiceId);
        if (service is null)
        {
            return reservationServiceDto;
        }

        reservationServiceDto.ServiceId = service.Id;
        reservationServiceDto.Name = service.Name;
        reservationServiceDto.Duration = service.Duration;

        return reservationServiceDto;
    }
}