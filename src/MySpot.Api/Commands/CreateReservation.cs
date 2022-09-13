namespace MySpot.Api.Commands;
    //init only
    //konstruktor od razu
    //toString wszystkie dane
    //with klonuje i zmienia wartosc
    //przypisanie do zmiennych (Tuple) var(1,2,3) = record
    //dane bez metod .. czest zastosowanie przy dto lub viewmodel
public record CreateReservation(Guid ParkingSpotId, Guid ReservationId, DateTime Date,
    string EmployeeName, string LicensePlate);

