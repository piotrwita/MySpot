using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySpot.Infrastructure.Auth;

internal sealed class Authenticator : IAuthenticator
{ 
    private readonly IClock _clock;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _expiry;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public Authenticator(IOptions<AuthOptions> options, IClock clock)
    { 
        _clock = clock;
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
        //dane uzywane do generowania tokenow
        _signingCredentials = new SigningCredentials(
            //przekazujemy dokladnie to samo co zdefiniowane w addauth
            //w api spoko poniewaz symetrics czyli w obie strony ten sam klucz
            //w mikro uzyjemy innego w ystemie rozproszonym
            //walidacja po stronie serwera oraz rownize generowanie nowych tokenow
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
            //jakiego typu algorytmu uzywamy - my uzywamy standardu
            SecurityAlgorithms.HmacSha256);

    }

    public JwtDto CreateToken(Guid userId, string role)
    {
        var now = _clock.Current();
        //zywotnosc tokena
        var expires = now.Add(_expiry);

        //jakie pary klucz wartosc maja byc zawarte w tokienie
        var claims = new List<Claim>
        {
            //subject jako userid
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            //dzieki temu claimowi mozemy dobrac sie do wlasciwosci user i zbindowac go na uniquename
            //(wtedy automatycznie przez famework bindowanie wlasnie dzieki unique name)
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            //sprawdzenie roli mozna dzieki temu sprawdzic przez httpcontext
            new(ClaimTypes.Role, role)
        };

        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        var accessToken = _jwtSecurityTokenHandler.WriteToken(jwt);

        return new JwtDto
        {
            AccessToken = accessToken
        };
    }
}
