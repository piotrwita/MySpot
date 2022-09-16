using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositiories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositiories;

internal class PostgresWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly MySpotDbContext _dbContext;

    public PostgresWeeklyParkingSpotRepository(MySpotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //incluze zeby dociagnac pry okazji pelen model domenowy po kluczu obcym (eager loading)
    public WeeklyParkingSpot Get(ParkingSpotId id)
        => _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .SingleOrDefault(x => x.Id == id);

    public IEnumerable<WeeklyParkingSpot> GetAll()
        => _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .ToList();

    public void Add(WeeklyParkingSpot weeklyParkingSpot)
    {
        _dbContext.Add(weeklyParkingSpot);
        //bez tego nie zapisze do bazy
        _dbContext.SaveChanges();
    }

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
        _dbContext.Update(weeklyParkingSpot); 
        _dbContext.SaveChanges();
    }

    public void Delete(WeeklyParkingSpot weeklyParkingSpot)
    {
        _dbContext.Remove(weeklyParkingSpot);
        _dbContext.SaveChanges();
    }
}
