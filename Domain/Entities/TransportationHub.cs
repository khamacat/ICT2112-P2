using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class TransportationHub
{
    public int HubId { get; private set; }

    public double Longitude { get; private set; }

    public double Latitude { get; private set; }

    public string CountryCode { get; private set; } = null!;

    public string Address { get; private set; } = null!;

    public string? OperationalStatus { get; private set; }

    public string? OperationTime { get; private set; }

    public virtual Airport? Airport { get; private set; }

    public virtual ICollection<DeliveryBatch> DeliveryBatches { get; private set; } = new List<DeliveryBatch>();

    public virtual ICollection<DeliveryRoute> DeliveryRouteDestinationHubs { get; private set; } = new List<DeliveryRoute>();

    public virtual ICollection<DeliveryRoute> DeliveryRouteOriginHubs { get; private set; } = new List<DeliveryRoute>();

    public virtual ShippingPort? ShippingPort { get; private set; }

    public virtual Warehouse? Warehouse { get; private set; }
}
