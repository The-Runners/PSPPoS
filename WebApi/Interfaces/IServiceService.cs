﻿using Contracts.DTOs;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<TimeSlot>> GetAvailableTimeSlots(Guid serviceId, TimeSlot timePeriod);

    Task<Service?> GetServiceById(Guid serviceId);

    Task<Service> Create(ServiceCreateDto serviceDto);

    Task<Service?> Edit(Guid serviceId, ServiceEditDto serviceDto);

    Task Delete(Guid serviceId);
}
