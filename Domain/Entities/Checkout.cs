using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Checkout
{
    public int Checkoutid { get; set; }

    public int Customerid { get; set; }

    public int Cartid { get; set; }

    public string? Deliverymethodid { get; set; }

    public bool? Notifyoptin { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
