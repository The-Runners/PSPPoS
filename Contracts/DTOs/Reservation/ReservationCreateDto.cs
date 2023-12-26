namespace Contracts.DTOs.Reservation;

public class ReservationCreateDto
{
    required public Guid OrderId { get; set; }

    required public Guid ServiceId { get; set; }

    required public DateTime TimeSlot { get; set; }
}