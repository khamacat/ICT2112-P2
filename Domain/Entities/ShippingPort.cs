using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class ShippingPort : TransportationHub
{
    private string _portCode = null!;
    private string PortCode { get => _portCode; set => _portCode = value; }

    private string _portName = null!;
    private string PortName { get => _portName; set => _portName = value; }

    private string? _portType;
    private string? PortType { get => _portType; set => _portType = value; }

    private int? _vesselSize;
    private int? VesselSize { get => _vesselSize; set => _vesselSize = value; }
}
