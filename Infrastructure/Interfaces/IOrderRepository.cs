﻿using Domain.Filters;
using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>?> GetFilteredOrders(OrderFilter filter);
}
