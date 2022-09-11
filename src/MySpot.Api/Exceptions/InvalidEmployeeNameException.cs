namespace MySpot.Api.Exceptions;

public class InvalidEmployeeNameException : CustomException
{
    public InvalidEmployeeNameException() : base("Employee name is invalid.")
    {
    }
}
