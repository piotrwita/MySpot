using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.Repositiories;
using MySpot.Infrastructure.DAL.Repositiories;

namespace MySpot.Infrastructure.DAL;

internal static class Extension
{
    //podpiecie pod baze danych (parametry polaczenia)
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        const string connectionString = "Host=localhost;Database=MySpot;Username=postgres";

        services
            //parametry polaczenia z silnikiem bazy danych
            .AddDbContext<MySpotDbContext>(x => x.UseNpgsql(connectionString))
            .AddScoped<IWeeklyParkingSpotRepository, PostgresWeeklyParkingSpotRepository>()
            .AddHostedService<DatabaseInitializer>();

        //Npgsql obsługuje również odczytywanie i zapisywanie DateTimeOffset do znacznika czasu ze strefą czasową, ale tylko z przesunięciem=0
        //Strefa czasowa była konwertowana na lokalną sygnaturę czasową podczas odczytu.
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }
}
