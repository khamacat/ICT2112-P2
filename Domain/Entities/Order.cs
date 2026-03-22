using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Order
{
    private int _orderid;
    private int Orderid { get => _orderid; set => _orderid = value; }

    private int _customerid;
    private int Customerid { get => _customerid; set => _customerid = value; }

    private int _checkoutid;
    private int Checkoutid { get => _checkoutid; set => _checkoutid = value; }

    private int? _transactionid;
    private int? Transactionid { get => _transactionid; set => _transactionid = value; }

    private DateTime _orderdate;
    private DateTime Orderdate { get => _orderdate; set => _orderdate = value; }

    private decimal _totalamount;
    private decimal Totalamount { get => _totalamount; set => _totalamount = value; }

    public virtual BatchOrder? BatchOrder { get; private set; }

    public virtual Checkout Checkout { get; private set; } = null!;

    public virtual Customer Customer { get; private set; } = null!;

    public virtual ICollection<CustomerChoice> CustomerChoices { get; private set; } = new List<CustomerChoice>();

    public virtual ICollection<Deposit> Deposits { get; private set; } = new List<Deposit>();

    public virtual ICollection<Loanlist> Loanlists { get; private set; } = new List<Loanlist>();

    public virtual ICollection<Ordercarbondatum> Ordercarbondata { get; private set; } = new List<Ordercarbondatum>();

    public virtual ICollection<Orderitem> Orderitems { get; private set; } = new List<Orderitem>();

    public virtual ICollection<Orderstatushistory> Orderstatushistories { get; private set; } = new List<Orderstatushistory>();

    public virtual ICollection<Packagingprofile> Packagingprofiles { get; private set; } = new List<Packagingprofile>();

    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public virtual ICollection<Refund> Refunds { get; private set; } = new List<Refund>();

    public virtual ICollection<Rentalorderlog> Rentalorderlogs { get; private set; } = new List<Rentalorderlog>();

    public virtual ICollection<Returnrequest> Returnrequests { get; private set; } = new List<Returnrequest>();

    public virtual ICollection<Shipment> Shipments { get; private set; } = new List<Shipment>();

    public virtual ICollection<ShippingOption> ShippingOptions { get; private set; } = new List<ShippingOption>();

    public virtual Transaction? Transaction { get; private set; }
}
