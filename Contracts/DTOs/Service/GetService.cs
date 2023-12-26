namespace Contracts.DTOs.Service
{
    public class GetService
    {
        required public Guid Id { get; init; }
        required public string Name { get; set; }
        public string? Description { get; set; }
        required public TimeOnly Duration { get; set; }
    }
}
