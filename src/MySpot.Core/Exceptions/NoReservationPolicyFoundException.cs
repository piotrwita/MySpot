using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class NoReservationPolicyFoundException : CustomException
{
    public JobTitle JobTitle { get; }
    public NoReservationPolicyFoundException(JobTitle jobTitle) 
        : base($"No reservation policy for {jobTitle} has been found.")
    {
        JobTitle = jobTitle;
    }
}

public sealed class CannotReserveParkingSpotException : CustomException
{
    public ParkingSpotId ParkingSpotId { get; }
    public CannotReserveParkingSpotException(ParkingSpotId parkingSpotId)
        : base($"Cannot reserve parking spot with id: {parkingSpotId}.")
    {
        ParkingSpotId = parkingSpotId;
    }
}