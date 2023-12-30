﻿namespace Domain.Exceptions;

public class InvalidTimeException : Exception
{
    public InvalidTimeException()
    {
    }

    public InvalidTimeException(string message)
        : base(message)
    {
    }

    public InvalidTimeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}