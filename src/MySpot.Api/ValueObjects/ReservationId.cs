using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;

public sealed record ReservationId
{
    public Guid Value { get; }

    public ReservationId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEmployeeNameException();
        }

        Value = value;
    }

    //public static ReservationId Create() => new(Guid.NewGuid());

    public static implicit operator Guid(ReservationId id) => id.Value;
    public static implicit operator ReservationId(Guid id) => new(id);
}