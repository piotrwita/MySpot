using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Policies;

//wzorzec strategii
public interface IReservationPolicy
{
    /// <summary>
    /// Czy dana polityka może zostać zaaplikowana
    /// po przekazanym parametrze można stwierdzić, czy ta konkretna polityka zostala stworzona dla tego konkretnego stanowiska pracy
    /// </summary>
    /// <returns></returns>
    bool CanBeApplied(JobTitle jobTitle);

    bool CanReserve(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, EmployeeName employeeName);
}
