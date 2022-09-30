namespace MySpot.Core.Exceptions;

public sealed class EmptyLincensePlateException : CustomException
{
    public EmptyLincensePlateException() : base("License plate is empty")
    {
    }
} 