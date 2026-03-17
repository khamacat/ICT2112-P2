using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Returnrequest
{
    public int Returnrequestid { get; private set; }

    public int Orderid { get; private set; }

    public int Customerid { get; private set; }

    public DateTime Requestdate { get; private set; }

    public DateTime? Completiondate { get; private set; }

    public virtual Customer Customer { get; private set; } = null!;

    public virtual Order Order { get; private set; } = null!;

    public virtual ICollection<Refund> Refunds { get; private set; } = new List<Refund>();

    public virtual ICollection<ReturnStage> ReturnStages { get; private set; } = new List<ReturnStage>();

    public virtual ICollection<Returnitem> Returnitems { get; private set; } = new List<Returnitem>();

    public virtual ICollection<Returnlog> Returnlogs { get; private set; } = new List<Returnlog>();
}
