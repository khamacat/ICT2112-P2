using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class LegCarbon
{
    private int _legId;
    private int LegId { get => _legId; set => _legId = value; }

    private double? _distanceKm;
    private double? DistanceKm { get => _distanceKm; set => _distanceKm = value; }

    private double? _weightKg;
    private double? WeightKg { get => _weightKg; set => _weightKg = value; }

    private double? _carbonKg;
    private double? CarbonKg { get => _carbonKg; set => _carbonKg = value; }

    private double? _carbonRate;
    private double? CarbonRate { get => _carbonRate; set => _carbonRate = value; }

    private int? _carbonResultId;
    private int? CarbonResultId { get => _carbonResultId; set => _carbonResultId = value; }

    private int? _routeLegId;
    private int? RouteLegId { get => _routeLegId; set => _routeLegId = value; }

    public virtual CarbonResult? CarbonResult { get; private set; }

    public virtual RouteLeg? RouteLeg { get; private set; }
}
