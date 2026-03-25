using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public abstract partial class Transport
{
    private int _transportId;
    private int TransportId { get => _transportId; set => _transportId = value; }

    private double? _maxLoadKg;
    private double? MaxLoadKg { get => _maxLoadKg; set => _maxLoadKg = value; }

    private double? _vehicleSizeM2;
    private double? VehicleSizeM2 { get => _vehicleSizeM2; set => _vehicleSizeM2 = value; }

    private bool? _isAvailable;
    private bool? IsAvailable { get => _isAvailable; set => _isAvailable = value; }
}
