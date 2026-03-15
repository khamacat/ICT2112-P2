using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class ReturnStage
{
    public int StageId { get; private set; }

    public int ReturnId { get; private set; }

    public double? EnergyKwh { get; private set; }

    public double? LabourHours { get; private set; }

    public double? MaterialsKg { get; private set; }

    public double? CleaningSuppliesQty { get; private set; }

    public double? WaterLitres { get; private set; }

    public double? PackagingKg { get; private set; }

    public decimal? SurchargeRate { get; private set; }

    public virtual ICollection<CarbonEmission> CarbonEmissions { get; private set; } = new List<CarbonEmission>();

    public virtual Returnrequest Return { get; private set; } = null!;
}
