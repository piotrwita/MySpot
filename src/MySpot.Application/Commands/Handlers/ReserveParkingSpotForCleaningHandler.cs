using MySpot.Application.Abstractions;
using MySpot.Core.Repositiories;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

internal class ReserveParkingSpotForCleaningHandler : ICommandHandler<ReserveParkingSpotForCleaning>
{
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IParkingReservationService _parkingReservationService;

    public ReserveParkingSpotForCleaningHandler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository, IParkingReservationService parkingReservationService)
    {
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        _parkingReservationService = parkingReservationService;
    }

    public async Task HandleAsync(ReserveParkingSpotForCleaning command)
    {
        var week = new Week(command.Date);
        var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetByWeekAsync(week);

        _parkingReservationService.ReserveParkingForCleaning(weeklyParkingSpots, new Date(command.Date));

        var tasks = weeklyParkingSpots.Select(x => _weeklyParkingSpotRepository.UpdateAsync(x));
        await Task.WhenAll(tasks);
    }
}
