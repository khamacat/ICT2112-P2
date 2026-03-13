using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Session
{
    public int Sessionid { get; set; }

    public int Userid { get; set; }

    public string Role { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime Expiresat { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual User User { get; set; } = null!;
}
