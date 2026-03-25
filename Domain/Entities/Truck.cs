using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Truck : Transport
{
    private int _truckId;
    private int TruckId { get => _truckId; set => _truckId = value; }

    private string? _truckType;
    private string? TruckType { get => _truckType; set => _truckType = value; }

    private string? _licensePlate;
    private string? LicensePlate { get => _licensePlate; set => _licensePlate = value; }
}
