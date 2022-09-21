using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record ParkingSpotId
{
    public Guid Value { get; }

    public ParkingSpotId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEmployeeNameException();
        }

        Value = value;
    }

    //public static ParkingSpotId Create() => new(Guid.NewGuid());

    public static implicit operator Guid(ParkingSpotId id) => id.Value;
    public static implicit operator ParkingSpotId(Guid id) => new(id);
}

public sealed record Capacity
{
    public int Value { get; }

    public Capacity(int value)
    {
        if (value is < 0 or > 4)
        {
            throw new InvalidCapacityException(value);
        }

        Value = value;
    }

    //public static ParkingSpotId Create() => new(Guid.NewGuid());

    public static implicit operator int(Capacity capacity) => capacity.Value;
    public static implicit operator Capacity(int value) => new(value);
}