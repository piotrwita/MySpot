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
    public Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id)
        => _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .SingleOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week)
        => await _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .Where(x => x.Week == week)
            .ToListAsync();

    public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
        => await _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .ToListAsync();

    public async Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        await _dbContext.AddAsync(weeklyParkingSpot);
        //bez tego nie zapisze do bazy
        await _dbContext.SaveChangesAsync(); 
    }

    public Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _dbContext.Update(weeklyParkingSpot);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _dbContext.Remove(weeklyParkingSpot);
        return Task.CompletedTask;
    }
}
