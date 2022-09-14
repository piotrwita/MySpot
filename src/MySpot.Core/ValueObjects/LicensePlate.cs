using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record LicensePlate
{
    public string Value { get; }

    public LicensePlate(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EmptyLincensePlateException();
        }

        if (value.Length is < 5 or > 8)
        {
            throw new InvalidLincensePlateException(value);
        }

        Value = value;
    }

    //przeciazenie operatorow
    //statyczny polimorfizm
    //przeciazenie operatora ktory pozwala na castowanie z jednego typu na drugi
    //niejawne castowanie z typu licensePlate na string i odwrotnie
    public static implicit operator string(LicensePlate licensePlate) => licensePlate?.Value;
    public static implicit operator LicensePlate(string licensePlate) => new(licensePlate);
}
