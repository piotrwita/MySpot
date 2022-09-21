using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;
using System;
using Xunit;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    [Theory]
    [InlineData("2022-08-09")]
    [InlineData("2022-08-17")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        // ARRANGE 
        var invalidDate = new Date(DateTime.Parse(dateString));
        var reservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "Joe Doe", "XYZ123", 1, invalidDate);

        // ACT
        //sprawdzam czy dostane błąd - taki jest cel testu
        var exception = Record.Exception(() => _weeklyParkingSpot.AddResevation(reservation, _now));

        // ASSERT 
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Fact]
    public void given_reservation_for_already_reserved_parking_spot_add_reservation_should_fail()
    {
        // ARRANGE
        var reservationDate = _now.AddDays(1);
        var reservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "Joe Doe", "XYZ123", 2, reservationDate);
        var nextReservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "Joe Doe", "XYZ123", 1, reservationDate);
        _weeklyParkingSpot.AddResevation(reservation, _now);

        // ACT
        //sprawdzam czy dostane błąd - taki jest cel testu
        var exception = Record.Exception(() => _weeklyParkingSpot.AddResevation(nextReservation, reservationDate));

        // ASSERT 
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotCapacityExceededException>();
    }

    [Fact]
    public void given_reservation_not_reserved_parking_spot_reservation_should_succeed()
    {
        // ARRANGE
        var reservationDate = _now.AddDays(1);
        var reservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "Joe Doe", "XYZ123", 1, reservationDate);

        // ACT 
        _weeklyParkingSpot.AddResevation(reservation, _now);

        // ASSERT 
        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
    }

    #region Arrange

    private readonly Date _now;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(new DateTime(2022, 08, 10));
        _weeklyParkingSpot = WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_now), "P1");
    }

    #endregion
}
