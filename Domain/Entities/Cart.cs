using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Cart
{
    public int Cartid { get; set; }

    public int? Customerid { get; set; }

    public int? Sessionid { get; set; }

    public DateTime? Rentalstart { get; set; }

    public DateTime? Rentalend { get; set; }

    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public virtual Customer? Customer { get; set; }

    public virtual Session? Session { get; set; }
}
