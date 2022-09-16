using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL;

public sealed class MySpotDbContext : DbContext
{
    //odwzorowanie encji/klas c# na tabele trzymane po stronie bazy danych
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<WeeklyParkingSpot> WeeklyParkingSpots { get; set; }

    public MySpotDbContext(DbContextOptions<MySpotDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    //aplikujemy nasze konfiguracje encji
    //metoda ta pozwala nam na analogiczna konfguracje typow
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //implmentuje wszystkie konfiguraje z assembly po interfejsie 
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly); 
    }
}
