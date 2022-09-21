﻿using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class WeeklyParkingSpot
{
    public const int MaxCapacity = 2;

    //bedziemy mieli dzieki hashset pewna unikalnosc zapewniona
    //nie bedzie sytuacji w ktorej bedziemy mieli zdublowana rezerwacje w ramach kolekcji
    private readonly HashSet<Reservation> _reservations = new();

    public ParkingSpotId Id { get; }
    public Week Week { get; }
    public ParkingSpotName Name { get; }
    public Capacity Capacity { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    private WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name, Capacity capacity)
    {
        Id = id;
        Week = week;
        Name = name;
        Capacity = capacity;
    }

    public static WeeklyParkingSpot Create(ParkingSpotId id, Week week, ParkingSpotName name)
        => new (id, week, name, MaxCapacity);

    internal void AddResevation(Reservation reservation, Date now)
    {
        var isInvalidDate = reservation.Date < Week.From ||
                            reservation.Date > Week.To ||
                            reservation.Date < now;

        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date.Value.Date);
        }

        var dateCapacity = _reservations
            .Where(x => x.Date == reservation.Date)
            .Sum(x => x.Capacity);

        if(dateCapacity + reservation.Capacity > Capacity)
        {
            throw new ParkingSpotCapacityExceededException(Id);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(ReservationId id)
        => _reservations.RemoveWhere(x => x.Id == id);

    public void RemoveReservations(IEnumerable<Reservation> reservations)
        => _reservations.RemoveWhere(x => reservations.Any(r => r.Id == x.Id));
}
