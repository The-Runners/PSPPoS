namespace Contracts.DTOs.Reservation;

public class ReservationOrderDto
{
    required public Guid CustomerId { get; set; }

    required public Guid EmployeeId { get; set; }

    required public Guid ServiceId { get; set; }

    required public DateTime TimeSlot { get; set; }

    required public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Tip { get; set; }
}