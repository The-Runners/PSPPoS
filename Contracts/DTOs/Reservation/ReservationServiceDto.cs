namespace Contracts.DTOs;

public class ReservationServiceDto
{
    public Guid? ReservationId { get; set; }

    public Guid? ServiceId { get; set; }

    public DateTime? StartTime { get; set; }

    public TimeSpan? Duration { get; set; }

    public string? Name { get; set; }
}