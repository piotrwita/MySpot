namespace MySpot.Api.Exceptions;

public class InvalidReservatedDateException : CustomException
{
    public DateTime Date { get; }

    public InvalidReservatedDateException(DateTime date) 
        : base($"Reservation date: {date:d} is invalid.") //:d short date
    {
        Date = date;
    }
}
