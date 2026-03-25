using ProRental.Domain.Enums;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iAlertControl
{
    bool CreateAlert(Alert alert);
    List<Alert>? GetAllAlerts();
    List<Alert> GetAlertsByProduct(int productId);
    List<Alert> GetOpenAlerts();
    bool UpdateAlertStatus(int alertId, AlertStatus status);
    bool ResolveAlert(int alertId);
    bool CheckLowStock(int productId, int threshold);
    bool AutoResolveAlertsIfStockAboveThreshold(int productId, int currentStock);
}
