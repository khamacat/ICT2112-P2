using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Warehouse
{
    public int HubId { get; private set; }

    public string WarehouseCode { get; private set; } = null!;

    public int? MaxProductCapacity { get; private set; }

    public double? TotalWarehouseVolume { get; private set; }

    public double? ClimateControlEmissionRate { get; private set; }

    public double? LightingEmissionRate { get; private set; }

    public double? SecuritySystemEmissionRate { get; private set; }

    public virtual TransportationHub Hub { get; private set; } = null!;
}
