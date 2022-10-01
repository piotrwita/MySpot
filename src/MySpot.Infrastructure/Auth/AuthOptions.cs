namespace MySpot.Infrastructure.Auth;

public class AuthOptions
{
    //dostawca podmiot wystawiajacy token - tu np mysspot
    public string Issuer { get; set; }
    //odbiorca, czyli do kogo ten token jest dedykowany
    public string Audience { get; set; }
    //unikalny klucz trzymany po stronie serwera - nikt go z zewnatrz nie moze znac
    public string SigningKey { get; set; }
    //czas zycia tokena
    public TimeSpan? Expiry { get; set; }
}
