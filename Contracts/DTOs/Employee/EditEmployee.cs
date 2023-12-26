namespace Contracts.DTOs.Employee
{
    public class EditEmployee
    {
        required public Guid Id { get; set; }
        public TimeOnly? WorkStartTime { get; set; }
        public TimeOnly? WorkEndTime { get; set; }
    }
}
