namespace Contracts.DTOs.Employee
{
    public class PostEmployee
    {
        required public TimeOnly WorkStartTime { get; set; }
        required public TimeOnly WorkEndTime { get; set; }
    }
}
