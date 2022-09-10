using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers
{
    [ApiController]//automatyczne bindowanie ciala rządania z body (zamiast [FromBody])
    [Route("[controller]")]
    public class ReservationsController : ControllerBase
    {
        private static readonly List<string> ParkingSpotNames = new()
        {
            "P1",
            "P2",
            "P3",
            "P4",
            "P5"
        };

        private static int Id = 1;
        private static readonly List<Reservation> Reservations = new();


        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> Get() => Ok(Reservations);

        [HttpGet("{id:int}")]
        public ActionResult<Reservation> Get(int id)
        {
            var reservation = Reservations.SingleOrDefault(x => x.Id == id);
            if (reservation is null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult Post(Reservation reservation)
        {
            if(ParkingSpotNames.All(x => x != reservation.ParkingSpotName))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest();
            }

            reservation.Date = DateTime.UtcNow.AddDays(1).Date;

            var reservationAlreadyExists = Reservations.Any(x =>
                x.ParkingSpotName == reservation.ParkingSpotName &&
                x.Date.Date == reservation.Date.Date);

            if(reservationAlreadyExists)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest();
            }


            reservation.Id = Id;
            Id++;
            Reservations.Add(reservation);

            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, null);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Reservation reservation)
        {
            var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
            if (existingReservation is null)
            {
                return NotFound();
            }

            existingReservation.LicencePlate = reservation.LicencePlate;

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
            if (existingReservation is null)
            {
                return NotFound();
            }

            Reservations.Remove(existingReservation);

            return NoContent();
        }
    }
}
