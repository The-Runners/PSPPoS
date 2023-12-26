namespace Contracts.DTOs.Reservation;

public class ReservationModelDto
{
    required public Guid Id { get; set; }

    required public Guid OrderId { get; set; }

    required public Guid ServiceId { get; set; }

    required public DateTime TimeSlot { get; set; }
}