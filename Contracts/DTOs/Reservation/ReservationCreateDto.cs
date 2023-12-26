namespace Contracts.DTOs.Reservation;

public class ReservationCreateDto
{
    public Guid OrderId { get; init; }
    public Guid ServiceId { get; init; }
}