﻿namespace MySpot.Core.Exceptions;

public class InvalidReservationDateException : CustomException
{
    public DateTime Date { get; }

    public InvalidReservationDateException(DateTime date) 
        : base($"Reservation date: {date:d} is invalid.") //:d short date
    {
        Date = date;
    }
}
