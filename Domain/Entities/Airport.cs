using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Airport
{
    public int HubId { get; private set; }

    public string AirportCode { get; private set; } = null!;

    public string AirportName { get; private set; } = null!;

    public int? Terminal { get; private set; }

    public int? AircraftSize { get; private set; }

    public virtual TransportationHub Hub { get; private set; } = null!;
}
