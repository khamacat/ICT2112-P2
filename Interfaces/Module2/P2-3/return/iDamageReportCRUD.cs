namespace ProRental.Interfaces.Domain;

public interface iDamageReportCRUD
{
    bool SaveDamageReport(int returnItemId, string description, string severity, decimal? repairCost, string? imagePath);
    bool AppendRepairNote(int returnItemId, string note);
    bool DeleteDamageReport(int returnItemId);
}
