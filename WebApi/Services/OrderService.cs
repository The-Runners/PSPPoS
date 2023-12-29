﻿using AutoMapper;
using Contracts.DTOs.Order;
using Contracts.DTOs.OrderProduct;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services;

public class OrderService : IOrderService
{
    private readonly IGenericRepository<Order> _orderRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IGenericRepository<Service> _serviceRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IOrderProductService _orderProductService;
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public OrderService(
        IGenericRepository<Order> orderRepository,
        IOrderProductRepository orderProductRepository,
        IReservationRepository reservationRepository,
        IGenericRepository<Service> serviceRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Customer> customerRepository,
        IMapper mapper,
        IOrderProductService orderProductService,
        IReservationService reservationService)
{
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
        _reservationRepository = reservationRepository;
        _serviceRepository = serviceRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _orderProductService = orderProductService;
        _reservationService = reservationService;
}

    public async Task<Order> CreateEmptyOrder(OrderPostModel orderDto)
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
        };

        return await _orderRepository.Add(order);
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
                throw new NullReferenceException("No OrderProduct found with given order and product IDs!");
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

    public async Task<OrderFinalDto> GenerateFinalOrderModel(Order order)
    {
        var totalPrice = await CalculateOrderPrice(order);
        var orderProducts = await _orderProductService.GenerateProductViewModels(order.Id);
        var reservationServiceDto = await _reservationService.GenerateReservationServiceModel(order.Id);
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
            TimeSlot = reservationServiceDto.TimeSlot,
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

    public async Task ApplyOrderDiscount(Guid orderId, decimal discount)
    {
        var order = await GetOrderById(orderId);
        order.Discount = discount;
        await _orderRepository.Update(order);
    }

    public async Task AddTip(Guid orderId, decimal tip)
    {
        var order = await GetOrderById(orderId);
        order.Tip = tip;
        await _orderRepository.Update(order);
    }

    private async Task<Order> GetOrderById(Guid orderId)
    {
        var order = await _orderRepository.GetById(orderId);
        if (order is null)
        {
            throw new NullReferenceException("Wrong orderID given to apply discount");
        }
        return order;
    }

    private async Task<decimal> GetOrderProductsPrice(IEnumerable<OrderProduct> orderProducts, decimal price)
    {
        foreach (var orderProduct in orderProducts)
        {
            var product = await _productRepository.GetById(orderProduct.ProductId);
            if (product is not null)
            {
                // TO-DO-MAYBE Add discount to specific order products
                price += product.Price * orderProduct.Amount;
            }
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
        if (order.CustomerId is not null)
        {
            var customer = await _customerRepository.GetById(order.CustomerId);
            return customer is not null ? customer.LoyaltyDiscount + order.Discount : order.Discount;
        }

        return order.Discount;
    }
}