using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class DeliveryRoute
{
    private int _routeId;
    private int RouteId { get => _routeId; set => _routeId = value; }

    private string _originAddress = null!;
    private string OriginAddress { get => _originAddress; set => _originAddress = value; }

    private string _destinationAddress = null!;
    private string DestinationAddress { get => _destinationAddress; set => _destinationAddress = value; }

    private double _totalDistanceKm;
    private double TotalDistanceKm { get => _totalDistanceKm; set => _totalDistanceKm = value; }

    private bool _isValid;
    private bool IsValid { get => _isValid; set => _isValid = value; }

    public virtual ICollection<RouteLeg> RouteLegs { get; private set; } = new List<RouteLeg>();

    public virtual ICollection<ShippingOption> ShippingOptions { get; private set; } = new List<ShippingOption>();
}
