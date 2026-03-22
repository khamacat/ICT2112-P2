using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class RouteLeg
{
    private int _legId;
    private int LegId { get => _legId; set => _legId = value; }

    private int _routeId;
    private int RouteId { get => _routeId; set => _routeId = value; }

    private int? _sequence;
    private int? Sequence { get => _sequence; set => _sequence = value; }

    private string? _startPoint;
    private string? StartPoint { get => _startPoint; set => _startPoint = value; }

    private string? _endPoint;
    private string? EndPoint { get => _endPoint; set => _endPoint = value; }

    private double? _distanceKm;
    private double? DistanceKm { get => _distanceKm; set => _distanceKm = value; }

    private bool? _isFirstMile;
    private bool? IsFirstMile { get => _isFirstMile; set => _isFirstMile = value; }

    private bool? _isLastMile;
    private bool? IsLastMile { get => _isLastMile; set => _isLastMile = value; }

    private int? _transportId;
    private int? TransportId { get => _transportId; set => _transportId = value; }

    public virtual ICollection<LegCarbon> LegCarbons { get; private set; } = new List<LegCarbon>();

    public virtual DeliveryRoute Route { get; private set; } = null!;

    public virtual Transport? Transport { get; private set; }
}
