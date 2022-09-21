using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public abstract class Reservation
{
    public ReservationId Id { get; }

    public ParkingSpotId ParkingSpotId { get; private set; }

    public Capacity Capacity { get; private set; }

    public Date Date { get; private set; }

    public Reservation(ReservationId id, ParkingSpotId parkingSpotId, Capacity capacity, Date date)
    {
        Id = id;
        ParkingSpotId = parkingSpotId;
        Capacity = capacity;
        Date = date;
    }

    protected Reservation()
    {
    }
}
