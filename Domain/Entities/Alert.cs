using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Alert
{
    public int Alertid { get; private set; }

    public int Productid { get; private set; }

    public int Minthreshold { get; private set; }

    public int Currentstock { get; private set; }

    public string Message { get; private set; } = null!;

    public DateTime Createdat { get; private set; }

    public DateTime Updatedat { get; private set; }

    public virtual Product Product { get; private set; } = null!;
}
