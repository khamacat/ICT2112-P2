using System;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Module2;

namespace ProRental.Domain.Module2.P2_2.Entities;

public class SupplierCategoryChangeLog : ISupplierRegistryEntity
{
    public int LogID { get; set; }
    public int SupplierID { get; set; }
    public SupplierCategory PreviousCategory { get; set; }
    public SupplierCategory NewCategory { get; set; }
    public string ChangedReason { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }

    public SupplierCategoryChangeLog()
    {
        LogID = 0;
        SupplierID = 0;
        PreviousCategory = SupplierCategory.NEWUNTESTED;
        NewCategory = SupplierCategory.NEWUNTESTED;
        ChangedAt = DateTime.UtcNow;
    }

    public string getLogSummary()
    {
        return $"SupplierID={SupplierID}: {PreviousCategory} -> {NewCategory} @ {ChangedAt:u}: {ChangedReason}";
    }

    public void updateReason(string newReason)
    {
        ChangedReason = newReason;
    }

    // ISupplierRegistryEntity
    string ISupplierRegistryEntity.GetType() => "SupplierCategoryChangeLog";
}