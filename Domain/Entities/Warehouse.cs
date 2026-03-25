using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Warehouse : TransportationHub
{
    private string _warehouseCode = null!;
    private string WarehouseCode { get => _warehouseCode; set => _warehouseCode = value; }

    private int? _maxProductCapacity;
    private int? MaxProductCapacity { get => _maxProductCapacity; set => _maxProductCapacity = value; }

    private double? _totalWarehouseVolume;
    private double? TotalWarehouseVolume { get => _totalWarehouseVolume; set => _totalWarehouseVolume = value; }

    private double? _climateControlEmissionRate;
    private double? ClimateControlEmissionRate { get => _climateControlEmissionRate; set => _climateControlEmissionRate = value; }

    private double? _lightingEmissionRate;
    private double? LightingEmissionRate { get => _lightingEmissionRate; set => _lightingEmissionRate = value; }

    private double? _securitySystemEmissionRate;
    private double? SecuritySystemEmissionRate { get => _securitySystemEmissionRate; set => _securitySystemEmissionRate = value; }
}
