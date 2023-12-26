namespace Contracts.DTOs.Reservation;

public class ReservationModelDto
{
    required public Guid Id { get; init; }
    required public Guid OrderId { get; init; }
    required public Guid ServiceId { get; init; }
}