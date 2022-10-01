using Microsoft.AspNetCore.Http;
using MySpot.Application.DTO;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Auth;

//bedziemy chcieli w ramach unikalnego kontekstu http zapisac token jezeli jest dla danego uzytkownika dostepny 
internal sealed class HttpContextTokenStorage : ITokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public JwtDto Get()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt))
        {
            return jwt as JwtDto;
        }

        return null;
    }

    public void Set(JwtDto jwt) 
        => _httpContextAccessor.HttpContext?.Items.Add(TokenKey, jwt);
}