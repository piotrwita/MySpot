namespace MySpot.Application.Security;

public interface IPasswordManager
{
    //
    string Secure(string password);
    //porownanie hasla przeslanego stringiem z haslem zabezpieczonym na bazie
    bool Validate(string password, string securedPassword);
}
