namespace Contracts.DTOs.Reservation;

public class ReservationPostDto
{
    required public Guid OrderId { get; set; }

    required public Guid ServiceId { get; set; }

    required public DateTime TimeSlot { get; set; }
}