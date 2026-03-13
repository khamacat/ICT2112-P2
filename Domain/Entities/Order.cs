using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Order
{
    public int Orderid { get; set; }

    public int Customerid { get; set; }

    public int Checkoutid { get; set; }

    public DateTime Orderdate { get; set; }

    public decimal Totalamount { get; set; }

    public virtual ICollection<BatchOrder> BatchOrders { get; set; } = new List<BatchOrder>();

    public virtual Checkout Checkout { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CustomerChoice> CustomerChoices { get; set; } = new List<CustomerChoice>();

    public virtual ICollection<Deliverymethod> Deliverymethods { get; set; } = new List<Deliverymethod>();

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<Loanlist> Loanlists { get; set; } = new List<Loanlist>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Orderstatushistory> Orderstatushistories { get; set; } = new List<Orderstatushistory>();

    public virtual ICollection<Packagingprofile> Packagingprofiles { get; set; } = new List<Packagingprofile>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual ICollection<Rentalorderlog> Rentalorderlogs { get; set; } = new List<Rentalorderlog>();

    public virtual ICollection<Returnrequest> Returnrequests { get; set; } = new List<Returnrequest>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
