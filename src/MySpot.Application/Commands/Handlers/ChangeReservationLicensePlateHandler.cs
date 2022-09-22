using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositiories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

internal class ChangeReservationLicensePlateHandler : ICommandHandler<ChangeReservationLicensePlate>
{
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public ChangeReservationLicensePlateHandler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
    {
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
    }

    public async Task HandleAsync(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            throw new WeeklyParkingSpotNotFoundException();
                
        }

        var reservationId = new ReservationId(command.ReservationId);
        var reservation = weeklyParkingSpot.Reservations
            .OfType<VehicleReservation>()
            .SingleOrDefault(x => x.Id == reservationId);

        if (reservation is null)
        {
            throw new ReservationNotFoundException(command.ReservationId);
        }

        reservation.ChangeLicensePlate(command.LicensePlate);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot); 
    }

    private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
    {
        var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();

        return weeklyParkingSpots
            .SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == reservationId));
    }
}