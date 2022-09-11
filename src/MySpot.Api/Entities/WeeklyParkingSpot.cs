using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    //bedziemy mieli dzieki hashset pewna unikalnosc zapewniona
    //nie bedzie sytuacji w ktorej bedziemy mieli zdublowana rezerwacje w ramach kolekcji
    private readonly HashSet<Reservation> _reservations = new();

    public ParkingSpotId Id { get; }
    public Week Week { get; } 
    public ParkingSpotName Name { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
    { 
        Id = id;
        Week = week; 
        Name = name;
    }

    public void AddResevation(Reservation reservation, Date now)
    {
        var isInvalidDate = reservation.Date < Week.From ||
                            reservation.Date > Week.To ||
                            reservation.Date < now;

        if (isInvalidDate)
        {
            throw new InvalidReservatedDateException(reservation.Date.Value.Date); 
        }

        var reservationAlreadyExists = Reservations.Any(x => 
            x.Date.Value.Date == reservation.Date.Value.Date);

        if (reservationAlreadyExists)
        {
            throw new ParkingSpotAlreadyReservationException(Name, reservation.Date.Value.Date);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(ReservationId id)
        => _reservations.RemoveWhere(x => x.Id == id); 
}
