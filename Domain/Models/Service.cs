namespace Domain.Models
{
    public class Service
    {
        required public Guid Id { get; init; }
        required public string Name { get; set; }
        public string? Description { get; set; }
        required public TimeOnly Duration { get; set; }

    }
}
