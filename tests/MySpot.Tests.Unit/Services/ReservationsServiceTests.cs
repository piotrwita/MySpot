using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositiories;
using MySpot.Infrastructure.Repositiories;
using MySpot.Tests.Unit.Shared;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    [Fact]
    public void given_reservation_not_taken_date_create_reservation_should_succeed()
    {
        // ARRANGE
        var weeklyParkingSpot = _weeklyParkingSpotRepository.GetAll().First();
        var command = new CreateReservation(weeklyParkingSpot.Id, Guid.NewGuid(),
            _clock.Current().AddMinutes(5), "John Doe", "XYZ123");

        // ACT
        var reservationId = _reservationsService.Create(command);

        // ASSERT
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }

    #region Arrange

    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IReservationsService _reservationsService;

    public ReservationsServiceTests()
    {
        _clock = new TestClock();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationsService = new ReservationsService(_clock, _weeklyParkingSpotRepository);
    }

    #endregion
}
