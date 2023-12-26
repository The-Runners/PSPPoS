namespace Domain.Models;

public class Reservation
{
    required public Guid Id { get; init; }

    required public Guid OrderId { get; init; }

    required public Guid ServiceId { get; set; }

    required public DateTime TimeSlot { get; set; }
}