using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class CarbonResult
{
    private int _carbonResultId;
    private int CarbonResultId { get => _carbonResultId; set => _carbonResultId = value; }

    private double? _totalCarbonKg;
    private double? TotalCarbonKg { get => _totalCarbonKg; set => _totalCarbonKg = value; }

    private DateTime? _createdAt;
    private DateTime? CreatedAt { get => _createdAt; set => _createdAt = value; }

    private bool? _validationPassed;
    private bool? ValidationPassed { get => _validationPassed; set => _validationPassed = value; }

    public virtual ICollection<LegCarbon> LegCarbons { get; private set; } = new List<LegCarbon>();
}
