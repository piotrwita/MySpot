﻿using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class Reservation
{
    public Guid Id { get; }
    public Guid ParingSpotId { get; private set; }
    public string EmployeeName { get; private set; } 
    public string LicensePlate { get; private set; }
    public DateTime Date { get; private set; }

    public Reservation(Guid id, Guid paringSpotId, string employeeName, string licensePlate, DateTime date)
    {
        Id = id;
        ParingSpotId = paringSpotId;
        EmployeeName = employeeName;
        ChangeLicensePlate(licensePlate);
        Date = date;
    }

    public void ChangeLicensePlate(string licensePlate)
    {
        if(string.IsNullOrEmpty(licensePlate))
        {
            throw new EmptyLincensePlateException();
        }

        LicensePlate = licensePlate;
    }
}
