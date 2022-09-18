using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Repositiories;

//wzorzec repozytorium
//stanowi abstrakcje nad dostepem do danych
//wystawia jako interface zbior metod mowiacych o tym co mozna z danymi zrobic
//jak dane pobrac zapisac usunac itd
public interface IWeeklyParkingSpotRepository
{
    Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id);
    Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week) => throw new NotImplementedException();
    Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync();
    Task AddAsync(WeeklyParkingSpot weeklyParkingSpot);
    Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot);
    Task DeleteAsync(WeeklyParkingSpot weeklyParkingSpot);
}
