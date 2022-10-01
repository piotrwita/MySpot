using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Core.Abstractions;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Exceptions;
using MySpot.Infrastructure.Logging;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("app");
        services.Configure<AppOptions>(section);

        services
            .AddPostgres(configuration)
            .AddSingleton<IClock, Clock>()
            .AddSingleton<ExceptionMiddleware>()
            .AddSecurity()
            .AddAuth(configuration)
            .AddHttpContextAccessor();
        //.AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>();

        var infrastructureAssembly = typeof(Clock).Assembly;

        //przekanuj wskazane assembly
        services.Scan(s => s.FromAssemblies(infrastructureAssembly) 
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))) 
            .AsImplementedInterfaces() 
            .WithScopedLifetime());

        //decorator ma strukture cebuli dlatego logowanie na koniec bo kolejnosc dodawania ma znaczeczenie tutaj
        services.AddCustomLogging();

        return services;
    }

    public static WebApplication UseInfrastucture(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
