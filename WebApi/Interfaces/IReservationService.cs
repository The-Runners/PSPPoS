using Contracts.DTOs.Reservation;

namespace WebApi.Interfaces;

public interface IReservationService
{
    // Reservation CRUD operations
    Task CreateReservation(ReservationOrderDto reservationDto);

    Task CancelReservation(Guid reservationId);
}