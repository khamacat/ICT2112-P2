using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Clearanceitem
{
    public int Clearanceitemid { get; private set; }

    public int Clearancebatchid { get; private set; }

    public int Inventoryitemid { get; private set; }

    public decimal? Finalprice { get; private set; }

    public decimal? Recommendedprice { get; private set; }

    public DateTime? Saledate { get; private set; }

    public virtual Clearancebatch Clearancebatch { get; private set; } = null!;

    public virtual Inventoryitem Inventoryitem { get; private set; } = null!;
}
