using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Order
{
    public required Guid Id { get; init; }

    public Guid? CustomerId { get; set; }

    public required Guid EmployeeId { get; set; }

    public required OrderStatus Status { get; set; }

    public required decimal Price { get; set; }

    public required decimal Discount { get; set; }

    public required decimal Tip { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
}
