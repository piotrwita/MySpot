using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;

namespace MySpot.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ICommandHandler<>).Assembly;

        //przekanuj wskazane assembly
        services.Scan(s => s.FromAssemblies(applicationAssembly)
            //wez wszystkie klasy ktore implementuja ten interfejs
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            //zarejestruj je jako ten interfejs
            .AsImplementedInterfaces()
            //cykl zycia
            .WithScopedLifetime());

        return services;
    }
}
