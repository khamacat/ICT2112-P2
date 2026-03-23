using ProRental.Domain.Enums;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iAlertControl
{
    bool CreateAlert(Alert alert);
    List<Alert>? GetAllAlerts();
    List<Alert> GetAlertsByStaff(int staffId);
    List<Alert> GetAlertsByProduct(int productId);
    bool SendAlertToStaff(int alertId, int staffId);
    bool UpdateAlertStatus(int alertId, AlertStatus status);
    bool ResolveAlert(int alertId);
    bool CheckLowStock(int productId, int threshold);
}
