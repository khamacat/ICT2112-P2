using ProRental.Domain.Enums;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Module2.P2_3;

public interface iAlertControl
{
    bool CreateAlert(int productId, int minThreshold, int staffId = 0);
    List<Alert> GetAllAlerts();
    List<Alert> GetAlertsByStaff(int staffId);
    List<Alert> GetAlertsByProduct(int productId);
    Alert? GetAlertById(int alertId);
    List<Alert> GetAlertsByThreshold(int threshold);
    bool SendAlertToStaff(int alertId, int staffId);
    bool UpdateAlertStatus(int alertId, AlertStatus status);
    bool UpdateAlertThreshold(int alertId, int newThreshold);
    bool ResolveAlert(int alertId);
    bool CheckLowStock(int productId, int threshold);
}
