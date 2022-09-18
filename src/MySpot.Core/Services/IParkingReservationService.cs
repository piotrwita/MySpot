﻿using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Services;

public interface IParkingReservationService
{
    void ReserveSpotForVehicle(IEnumerable<WeeklyParkingSpot> allParkingSpots, JobTitle jobTitle,
        WeeklyParkingSpot parkingSpotToReserve, Reservation reservation);
}
