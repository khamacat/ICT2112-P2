using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Ship : Transport
{
    private int _shipId;
    private int ShipId { get => _shipId; set => _shipId = value; }

    private string? _vesselType;
    private string? VesselType { get => _vesselType; set => _vesselType = value; }

    private string? _vesselNumber;
    private string? VesselNumber { get => _vesselNumber; set => _vesselNumber = value; }

    private string? _maxVesselSize;
    private string? MaxVesselSize { get => _maxVesselSize; set => _maxVesselSize = value; }
}
