using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Exceptions;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPostgres(configuration)
            .AddSingleton<IClock, Clock>()
            .AddSingleton<ExceptionMiddleware>();
        //.AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>();

        return services;
    }

    public static WebApplication UseInfrastucture(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.MapControllers();

        return app;
    }
}
