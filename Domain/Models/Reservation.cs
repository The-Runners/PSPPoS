namespace Domain.Models;

public class Reservation
{
    public required Guid Id { get; init; }

    public required Guid OrderId { get; init; }

    public required Guid ServiceId { get; set; }

    public required DateTime TimeSlot { get; set; }
}