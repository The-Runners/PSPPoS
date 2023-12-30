using Domain.Enums;

namespace Domain.Filters;

public class OrderFilter
{
    public Guid? EmployeeId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    
    public IEnumerable<OrderStatus>? OrderStatuses { get; set; }
}
