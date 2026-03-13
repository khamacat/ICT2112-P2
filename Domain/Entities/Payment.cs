using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Payment
{
    public string Paymentid { get; set; } = null!;

    public int Orderid { get; set; }

    public int Transactionid { get; set; }

    public decimal Amount { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
