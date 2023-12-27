using Contracts.DTOs.Reservation;

namespace WebApi.Interfaces
{
    public interface IReservationService
    {
        Task CreateReservation(ReservationOrderDto reservationDto);
    }
}
