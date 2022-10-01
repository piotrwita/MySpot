using MySpot.Application.DTO;

namespace MySpot.Application.Security;

//prosta abstrakcja odpowiedzialna za przekazanie tokena dalej
public interface ITokenStorage
{
    void Set(JwtDto jwt);
    JwtDto Get();
}
