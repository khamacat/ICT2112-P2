using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class DeliveryBatch
{
    public int DeliveryBatchId { get; private set; }

    public double? BatchWeightKg { get; private set; }

    public string? DestinationAddress { get; private set; }

    public int? TotalOrders { get; private set; }

    public double? CarbonSavings { get; private set; }

    public int? HubId { get; private set; }

    public virtual ICollection<BatchOrder> BatchOrders { get; private set; } = new List<BatchOrder>();

    public virtual TransportationHub? Hub { get; private set; }

    public virtual ICollection<Shipment> Shipments { get; private set; } = new List<Shipment>();
}
