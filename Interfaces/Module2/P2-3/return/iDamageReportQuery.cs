using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iDamageReportQuery
{
    Damagereport? GetDamageReportByReturnItem(int returnItemId);
    List<Damagereport> GetDamageReportsByReturnRequest(int returnRequestId);
    byte[] DownloadDamageReport(int damageReportId);
}