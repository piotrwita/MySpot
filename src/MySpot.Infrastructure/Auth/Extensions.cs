using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.Security;
using MySpot.Infrastructure.DAL;
using System.Text;

namespace MySpot.Infrastructure.Auth;

internal static class Extensions
{
    private const string SectionName = "auth";

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetRequiredSection(SectionName));
        var options = configuration.GetOptions<AuthOptions>(SectionName);

        services
            .AddSingleton<IAuthenticator, Authenticator>()
            .AddSingleton<ITokenStorage, HttpContextTokenStorage>()
            .AddAuthentication(x =>
            {
                //schemat uwierzytelniania - ustawienie konkretnego schematu
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                //ustawienie ze odbiorcami naszych tokenow beda tylko ci uzytkownicy
                //reguly walidacji
                //atrybuty jakie atrybuty walidacji musza byc spelnione zeby ten token byl uznany za poprawny
                x.Audience = options.Audience;
                //sczegoly wyjatku - na prodzie raczej false
                x.IncludeErrorDetails = true;
                //dodatkowe parametry walidujace token
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    //przesuniecie w czasie po stronie serwera
                    //nie zezwalamy na zaden bufor dzialania tokena
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
                };
            });

        services.AddAuthorization();

        return services; 
    }
}