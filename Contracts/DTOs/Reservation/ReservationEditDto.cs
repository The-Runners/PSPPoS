namespace Contracts.DTOs;

public class ReservationEditDto
{
    public required Guid Id { get; init; }

    public Guid? OrderId { get; init; }

    public Guid? ServiceId { get; init; }

    public DateTime? StartTime { get; init; }
}