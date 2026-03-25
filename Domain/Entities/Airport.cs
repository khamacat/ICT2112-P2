using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Airport : TransportationHub
{
    private string _airportCode = null!;
    private string AirportCode { get => _airportCode; set => _airportCode = value; }

    private string _airportName = null!;
    private string AirportName { get => _airportName; set => _airportName = value; }

    private int? _terminal;
    private int? Terminal { get => _terminal; set => _terminal = value; }

    private int? _aircraftSize;
    private int? AircraftSize { get => _aircraftSize; set => _aircraftSize = value; }
}
