using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Inventoryitem
{
    public int Inventoryid { get; private set; }

    public int Productid { get; private set; }

    public string Serialnumber { get; private set; } = null!;

    public DateTime Createdat { get; private set; }

    public DateTime Updatedat { get; private set; }

    public DateTime? Expirydate { get; private set; }

    public virtual Clearanceitem? Clearanceitem { get; private set; }

    public virtual ICollection<Loanitem> Loanitems { get; private set; } = new List<Loanitem>();

    public virtual Product Product { get; private set; } = null!;

    public virtual ICollection<Returnitem> Returnitems { get; private set; } = new List<Returnitem>();
}
