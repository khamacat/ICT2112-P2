using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class DeliveryRoute
{
    public int RouteId { get; private set; }

    public string OriginAddress { get; private set; } = null!;

    public string DestinationAddress { get; private set; } = null!;

    public double? TotalDistanceKm { get; private set; }

    public bool? IsValid { get; private set; }

    public int? OriginHubId { get; private set; }

    public int? DestinationHubId { get; private set; }

    public virtual TransportationHub? DestinationHub { get; private set; }

    public virtual TransportationHub? OriginHub { get; private set; }

    public virtual ICollection<RouteLeg> RouteLegs { get; private set; } = new List<RouteLeg>();

    public virtual ICollection<ShippingOption> ShippingOptions { get; private set; } = new List<ShippingOption>();
}
