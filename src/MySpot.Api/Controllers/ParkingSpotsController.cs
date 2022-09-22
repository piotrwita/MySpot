using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers;

[ApiController]//automatyczne bindowanie ciala rządania z body (zamiast [FromBody])
[Route("parking-spots")]//("[controller]")]
//unikalnie w ramach każdego żądania http tworzony jest controller
public class ParkingSpotsController : ControllerBase
{
    private readonly ICommandHandler<ChangeReservationLicensePlate> _changeReservationLicensePlateHandler;
    private readonly ICommandHandler<DeleteReservation> _deleteReservationHandler;
    private readonly ICommandHandler<ReserveParkingSpotForCleaning> _reserveParkingSpotForCleaningHandler;
    private readonly ICommandHandler<ReserveParkingSpotForVehicle> _reserveParkingSpotForVehicleHandler;
    private readonly IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> _getWeeklyParkingSpotsHandler;

    public ParkingSpotsController(ICommandHandler<ChangeReservationLicensePlate> changeReservationLicensePlateHandler,
        ICommandHandler<DeleteReservation> deleteReservationHandler, 
        ICommandHandler<ReserveParkingSpotForCleaning> reserveParkingSpotForCleaningHandler, 
        ICommandHandler<ReserveParkingSpotForVehicle> reserveParkingSpotForVehicleHandler, 
        IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> getWeeklyParkingSpotsHandler)
    {
        _changeReservationLicensePlateHandler = changeReservationLicensePlateHandler;
        _deleteReservationHandler = deleteReservationHandler;
        _reserveParkingSpotForCleaningHandler = reserveParkingSpotForCleaningHandler;
        _reserveParkingSpotForVehicleHandler = reserveParkingSpotForVehicleHandler;
        _getWeeklyParkingSpotsHandler = getWeeklyParkingSpotsHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeeklyParkingSpotDto>>> Get([FromQuery] GetWeeklyParkingSpots query)
        => Ok(await _getWeeklyParkingSpotsHandler.HandleAsync(query)); 

    [HttpPost("{parkingSpotId:guid}/reservations/vehicle")]
    public async Task<ActionResult> Post(Guid parkingSpotId, ReserveParkingSpotForVehicle command)
    {
        await _reserveParkingSpotForVehicleHandler.HandleAsync(command with 
        { 
            ReservationId = Guid.NewGuid(),
            ParkingSpotId = parkingSpotId
        }); 

        return NoContent();
    }

    [HttpPost("reservations/cleaning")]
    public async Task<ActionResult> Post(ReserveParkingSpotForCleaning command)
    {
        await _reserveParkingSpotForCleaningHandler.HandleAsync(command);
        return NoContent();
    }

    [HttpPut("reservations/{reservationId:guid}")]
    public async Task<ActionResult> Put(Guid reservationId, ChangeReservationLicensePlate command)
    {
        await _changeReservationLicensePlateHandler.HandleAsync(command with { ReservationId = reservationId });        
        return NoContent();
    }

    [HttpDelete("reservations/{reservationId:guid}")]
    public async Task<ActionResult> Delete(Guid reservationId)
    {
        await _deleteReservationHandler.HandleAsync(new DeleteReservation(reservationId));
        return NoContent();
    }
}
