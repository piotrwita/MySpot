using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries;

public sealed class GetWeeklyParkingSpots : IQuery<IEnumerable<WeeklyParkingSpotDto>>
{
    //wymagalny parametr / jezeli null to wszystko jezeli nie to szukam po dacie
    public DateTime? Date { get; set; }  
}
