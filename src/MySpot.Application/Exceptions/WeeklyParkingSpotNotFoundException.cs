using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class WeeklyParkingSpotNotFoundException : CustomException
{
    public Guid? Id { get; }

    public WeeklyParkingSpotNotFoundException(Guid id)
        : base($"Weekly parking spot with id: {id} not was found.")
    {
        Id = id;
    }

    public WeeklyParkingSpotNotFoundException() 
        : base($"Weekly parking spot with id not was found.")
    { 
    }
}
 