using Contracts.DTOs.Reservation;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IReservationRepository
{
    Task<List<ReservationModelDto>?> GetFilteredReservations(ReservationFilter filter);
}