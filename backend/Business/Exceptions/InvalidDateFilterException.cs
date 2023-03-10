namespace Business.Exceptions;

public class InvalidDateFilterException : Exception
{
    public InvalidDateFilterException(string? message) 
        : base(message)
    {
    }
}