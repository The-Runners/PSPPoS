using Contracts.DTOs;
using Domain.Models;

namespace Contracts.Extensions;

public static class ReservationExtensions
{
    public static ReservationModelDto ToModelDto(this Reservation reservation) => new()
    {
        Id = reservation.Id,
        OrderId = reservation.OrderId,
        ServiceId = reservation.ServiceId,
        StartTime = reservation.StartTime,
    };
}