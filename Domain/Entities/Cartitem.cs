using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Cartitem
{
    public int Cartitemid { get; set; }

    public int Cartid { get; set; }

    public int Productid { get; set; }

    public int Quantity { get; set; }

    public bool? Isselected { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
