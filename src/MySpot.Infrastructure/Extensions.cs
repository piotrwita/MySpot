using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
        //dokumentacja swagera zintegrowana z podejsciem minimalapi
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swagger =>
        {
            swagger.EnableAnnotations();
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MySpot Api",
                Version = "v1"
            });
        });

        return services;
    }

    public static WebApplication UseInfrastucture(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwagger();
        //inny ui swaggera
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = "docs";
            reDoc.DocumentTitle = "MySpot Api";
            reDoc.SpecUrl("/swagger/v1/swagger.json"); 
        });
        //app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetSection(sectionName);
        section.Bind(options);

        return options;
    }
}
