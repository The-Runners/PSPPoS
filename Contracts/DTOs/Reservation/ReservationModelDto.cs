namespace Contracts.DTOs;

public class ReservationModelDto
{
    public required Guid Id { get; init; }

    public required Guid OrderId { get; init; }

    public required Guid ServiceId { get; init; }

    public required DateTime StartTime { get; init; }
}