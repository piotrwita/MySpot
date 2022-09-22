using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;
//init only
//konstruktor od razu
//toString wszystkie dane
//with klonuje i zmienia wartosc
//przypisanie do zmiennych (Tuple) var(1,2,3) = record
//dane bez metod .. czest zastosowanie przy dto lub viewmodel
public record ReserveParkingSpotForVehicle(Guid ParkingSpotId, Guid ReservationId, int Capacity,
    DateTime Date, string EmployeeName, string LicensePlate) : ICommand;
