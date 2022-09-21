using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public sealed class CleaningReservation : Reservation
{
    public CleaningReservation(ReservationId id, ParkingSpotId parkingSpotId, Date date) 
        : base(id, parkingSpotId, 2, date)
    { 
    }

    //bezparametrowy kontruktor jest wymagany wzgledem EFCore
    //dzieki temu nie musimy posiadac konstruktorow publicznych
    private CleaningReservation()
    {
    }
}
