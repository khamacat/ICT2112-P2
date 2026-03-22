using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iDamageReportCRUD
{
    bool SubmitDamageReport(int returnItemId, Damagereport damageReport);
}