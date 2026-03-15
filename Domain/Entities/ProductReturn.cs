using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class ProductReturn
{
    public int ReturnId { get; private set; }

    public string? ReturnStatus { get; private set; }

    public double? TotalCarbon { get; private set; }

    public DateOnly? DateIn { get; private set; }

    public DateOnly? DateOn { get; private set; }
}
