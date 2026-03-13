using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Orderitem
{
    public int Orderitemid { get; set; }

    public int Orderid { get; set; }

    public int Productid { get; set; }

    public int Quantity { get; set; }

    public decimal Unitprice { get; set; }

    public DateTime? Rentalstartdate { get; set; }

    public DateTime? Rentalenddate { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
