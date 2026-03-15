using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class RouteLeg
{
    public int LegId { get; private set; }

    public int RouteId { get; private set; }

    public int? Sequence { get; private set; }

    public string? StartPoint { get; private set; }

    public string? EndPoint { get; private set; }

    public double? DistanceKm { get; private set; }

    public bool? IsFirstMile { get; private set; }

    public bool? IsLastMile { get; private set; }

    public int? TransportId { get; private set; }

    public virtual ICollection<LegCarbon> LegCarbons { get; private set; } = new List<LegCarbon>();

    public virtual DeliveryRoute Route { get; private set; } = null!;

    public virtual Transport? Transport { get; private set; }
}
