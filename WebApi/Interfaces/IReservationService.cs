using Contracts.DTOs.Reservation;

namespace WebApi.Interfaces;

public interface IReservationService
{
    Task CreateReservation(ReservationOrderDto reservationDto);

    Task CancelReservation(Guid reservationId);

    Task<DateTime> CalculateReservationEndTime(Guid serviceId, DateTime reservationStart);

    Task<ReservationServiceDto> GenerateReservationServiceModel(Guid orderId);
}