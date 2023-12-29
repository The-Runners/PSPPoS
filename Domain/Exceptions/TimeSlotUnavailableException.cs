namespace Domain.Exceptions;

public class TimeSlotUnavailableException : Exception
{
    public TimeSlotUnavailableException()
    {
    }

    public TimeSlotUnavailableException(string message)
        : base(message)
    {
    }

    public TimeSlotUnavailableException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

