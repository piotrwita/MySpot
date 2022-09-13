﻿using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Repositiories;

//wzorzec repozytorium
//stanowi abstrakcje nad dostepem do danych
//wystawia jako interface zbior metod mowiacych o tym co mozna z danymi zrobic
//jak dane pobrac zapisac usunac itd
public interface IWeeklyParkingSpotRepository
{
    WeeklyParkingSpot Get(ParkingSpotId id);
    IEnumerable<WeeklyParkingSpot> GetAll ();
    void Add(WeeklyParkingSpot weeklyParkingSpot);
    void Update(WeeklyParkingSpot weeklyParkingSpot);
    void Delete(WeeklyParkingSpot weeklyParkingSpot);
}
