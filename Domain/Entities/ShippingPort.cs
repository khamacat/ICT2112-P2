using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class ShippingPort
{
    public int HubId { get; private set; }

    public string PortCode { get; private set; } = null!;

    public string PortName { get; private set; } = null!;

    public string? PortType { get; private set; }

    public int? VesselSize { get; private set; }

    public virtual TransportationHub Hub { get; private set; } = null!;
}
