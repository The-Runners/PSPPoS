using Contracts.DTOs;

namespace WebApi.Interfaces;

public interface IReservationService
{
    Task CreateReservation(ReservationOrderDto reservationDto);

    Task CancelReservation(Guid reservationId);
}