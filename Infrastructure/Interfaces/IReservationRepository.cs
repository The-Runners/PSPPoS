using Contracts.DTOs.Reservation;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IReservationRepository
{
    Task<ReservationModelDto?> GetReservationById(Guid id);

    Task<List<ReservationModelDto>?> GetFilteredReservations(ReservationFilter filter);

    Task AddReservation(ReservationCreateDto reservationDto);
}