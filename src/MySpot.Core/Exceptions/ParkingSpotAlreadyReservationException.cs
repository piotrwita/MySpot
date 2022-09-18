namespace MySpot.Core.Exceptions;

public sealed class ParkingSpotAlreadyReservationException : CustomException
{
    public string Name { get; }
    public DateTime Date { get; }

    public ParkingSpotAlreadyReservationException(string name, DateTime date)
        : base($"Parking spot: {name} is already reservated at: {date:d}.") //:d short date
    {
        Name = name;
        Date = date;
    }
}
