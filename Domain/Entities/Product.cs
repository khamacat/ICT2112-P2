using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Product
{
    public int Productid { get; private set; }

    public int Categoryid { get; private set; }

    public string Sku { get; private set; } = null!;

    public decimal Threshold { get; private set; }

    public DateTime Createdat { get; private set; }

    public DateTime Updatedat { get; private set; }

    public virtual ICollection<Alert> Alerts { get; private set; } = new List<Alert>();

    public virtual ICollection<Cartitem> Cartitems { get; private set; } = new List<Cartitem>();

    public virtual Category Category { get; private set; } = null!;

    public virtual ICollection<Inventoryitem> Inventoryitems { get; private set; } = new List<Inventoryitem>();

    public virtual ICollection<Lineitem> Lineitems { get; private set; } = new List<Lineitem>();

    public virtual ICollection<Orderitem> Orderitems { get; private set; } = new List<Orderitem>();

    public virtual Productdetail? Productdetail { get; private set; }

    public virtual ICollection<Productfootprint> Productfootprints { get; private set; } = new List<Productfootprint>();
}
