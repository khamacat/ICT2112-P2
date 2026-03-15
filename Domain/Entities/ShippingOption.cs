using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class ShippingOption
{
    public int OptionId { get; private set; }

    public string? DisplayName { get; private set; }

    public decimal? Cost { get; private set; }

    public double? Carbonfootprintkg { get; private set; }

    public int? DeliveryDays { get; private set; }

    public int? OrderId { get; private set; }

    public int? RouteId { get; private set; }

    public virtual Order? Order { get; private set; }

    public virtual DeliveryRoute? Route { get; private set; }
}
