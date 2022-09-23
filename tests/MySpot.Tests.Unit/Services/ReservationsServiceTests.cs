using MySpot.Application.Commands;
using MySpot.Core.Abstractions;
using MySpot.Core.Policies;
using MySpot.Core.Repositiories;
using MySpot.Core.Services;
using MySpot.Infrastructure.DAL.Repositiories;
using MySpot.Tests.Unit.Shared;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    [Fact]
    public async Task given_reservation_not_taken_date_create_reservation_should_succeed()
    {
        // ARRANGE
        var weeklyParkingSpot = (await _weeklyParkingSpotRepository.GetAllAsync()).First();
        var command = new ReserveParkingSpotForVehicle(weeklyParkingSpot.Id, Guid.NewGuid(), 1,
            _clock.Current().AddMinutes(5), "John Doe", "XYZ123");

        // ACT
        var reservationId = await _reservationsService.ReserveForVehicleAsync(command);

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

        var parkingReservationService = new ParkingReservationService(new IReservationPolicy[]
        {
            new BossReservationPolicy(),
            new ManagerReservationPolicy(),
            new RegularEmployeeReservationPolicy(_clock)
        }, _clock);

        _reservationsService = new ReservationsService(_clock, _weeklyParkingSpotRepository, parkingReservationService);
    }

    #endregion
}
