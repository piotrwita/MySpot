namespace MySpot.Api.Exceptions;

public sealed class InvalidLincensePlateException : CustomException
{
    public string LicensePlate { get; }

    public InvalidLincensePlateException(string licensePlate) 
        : base($"License plate: {licensePlate} is invalid.")
    {
        LicensePlate = licensePlate;
    }
}
