using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class CustomerChoice
{
    private int _customerId;
    private int CustomerId { get => _customerId; set => _customerId = value; }

    private int _orderId;
    private int OrderId { get => _orderId; set => _orderId = value; }

    private DateTime? _createdAt;
    private DateTime? CreatedAt { get => _createdAt; set => _createdAt = value; }

    public virtual Customer Customer { get; private set; } = null!;

    public virtual Order Order { get; private set; } = null!;
}
