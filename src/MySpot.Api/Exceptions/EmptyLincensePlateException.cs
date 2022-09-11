namespace MySpot.Api.Exceptions;

public sealed class EmptyLincensePlateException : CustomException
{
    public EmptyLincensePlateException() : base("License plate is empty")
    {
    }
}
