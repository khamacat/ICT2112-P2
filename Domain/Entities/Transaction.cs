using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Transaction
{
    public int Transactionid { get; set; }

    public int Orderid { get; set; }

    public decimal Amount { get; set; }

    public string? Providertransactionid { get; set; }

    public DateTime Createdat { get; set; }

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
