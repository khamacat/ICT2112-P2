using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Clearancebatch
{
    public int Clearancebatchid { get; private set; }

    public string Batchname { get; private set; } = null!;

    public DateTime Createddate { get; private set; }

    public DateTime? Clearancedate { get; private set; }

    public virtual ICollection<Clearanceitem> Clearanceitems { get; private set; } = new List<Clearanceitem>();

    public virtual ICollection<Clearancelog> Clearancelogs { get; private set; } = new List<Clearancelog>();
}
