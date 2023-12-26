using Contracts.DTOs.Reservation;

namespace Infrastructure.Interfaces;

public interface IReservationRepository
{
    Task<IEnumerable<ReservationModelDto>> GetFilteredReservations();
    Task AddReservation(ReservationCreateDto reservation);
}