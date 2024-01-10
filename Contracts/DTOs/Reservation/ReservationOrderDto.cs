namespace Contracts.DTOs;

public class ReservationOrderDto
{
    public required Guid CustomerId { get; set; }

    public required Guid EmployeeId { get; set; }

    public required Guid ServiceId { get; set; }

    public required DateTime TimeSlot { get; set; }
}