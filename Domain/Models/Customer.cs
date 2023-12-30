using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Customer
{
    public required Guid Id { get; init; }

    public required decimal LoyaltyDiscount { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset CreatedAt { get; init; }
}