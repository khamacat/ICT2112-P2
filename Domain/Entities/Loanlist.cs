using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Loanlist
{
    public int Loanlistid { get; private set; }

    public int Orderid { get; private set; }

    public int Customerid { get; private set; }

    public DateTime Loandate { get; private set; }

    public DateTime Duedate { get; private set; }

    public DateTime? Returndate { get; private set; }

    public string? Remarks { get; private set; }

    public virtual Customer Customer { get; private set; } = null!;

    public virtual ICollection<Loanitem> Loanitems { get; private set; } = new List<Loanitem>();

    public virtual ICollection<Loanlog> Loanlogs { get; private set; } = new List<Loanlog>();

    public virtual Order Order { get; private set; } = null!;
}
