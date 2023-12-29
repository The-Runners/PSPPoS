namespace Domain.Filters;

public class ReservationFilter
{
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? OrderId { get; set; }
}