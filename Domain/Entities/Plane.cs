using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Plane : Transport
{
    private int _planeId;
    private int PlaneId { get => _planeId; set => _planeId = value; }

    private string? _planeType;
    private string? PlaneType { get => _planeType; set => _planeType = value; }

    private string? _planeCallsign;
    private string? PlaneCallsign { get => _planeCallsign; set => _planeCallsign = value; }
}
