﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.Repositiories;
using MySpot.Infrastructure.DAL.Repositiories;

namespace MySpot.Infrastructure.DAL;

internal static class Extension
{
    private const string SectionName = "postgres";

    //podpiecie pod baze danych (parametry polaczenia)
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);
        services.Configure<PostgresOptions>(section);
        var options = configuration.GetOptions<PostgresOptions>(SectionName);

        services
            //parametry polaczenia z silnikiem bazy danych
            .AddDbContext<MySpotDbContext>(x => x.UseNpgsql(options.ConnectionString))
            .AddScoped<IWeeklyParkingSpotRepository, PostgresWeeklyParkingSpotRepository>()
            .AddHostedService<DatabaseInitializer>();

        //Npgsql obsługuje również odczytywanie i zapisywanie DateTimeOffset do znacznika czasu ze strefą czasową, ale tylko z przesunięciem=0
        //Strefa czasowa była konwertowana na lokalną sygnaturę czasową podczas odczytu.
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetSection(sectionName);
        section.Bind(options);

        return options;
    }
}
