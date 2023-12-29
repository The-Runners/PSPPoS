﻿using Contracts.DTOs.Reservation;
using Domain.Filters;
using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<List<ReservationModelDto>?> GetFilteredReservations(ReservationFilter filter);

    Task<Reservation?> GetReservationByOrderId(Guid orderId);
}