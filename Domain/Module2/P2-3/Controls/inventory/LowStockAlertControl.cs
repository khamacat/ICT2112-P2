using ProRental.Domain.Enums;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Module2.P2_3;

namespace ProRental.Domain.Module2.P2_3.Controls;

public class LowStockAlertControl : iAlertControl, iStockObserver
{
    private readonly IAlertMapper _alertMapper;
    private readonly iInventoryQueryControl _inventoryQueryControl;

    public LowStockAlertControl(IAlertMapper alertMapper, iInventoryQueryControl inventoryQueryControl)
    {
        _alertMapper = alertMapper ?? throw new ArgumentNullException(nameof(alertMapper));
        _inventoryQueryControl = inventoryQueryControl ?? throw new ArgumentNullException(nameof(inventoryQueryControl));
    }

    public bool CreateAlert(Alert alert)
    {
        if (alert is null || alert.GetProductId() <= 0)
        {
            return false;
        }

        try
        {
            _alertMapper.Insert(alert);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<Alert> GetAlertsByStaff(int staffId)
    {
        return _alertMapper.FindAll()?
            .Where(a => a.GetStaffId() == staffId)
            .ToList() ?? new List<Alert>();
    }

    public List<Alert> GetAlertsByProduct(int productId)
    {
        return _alertMapper.FindByProductId(productId)?.ToList() ?? new List<Alert>();
    }

    public bool SendAlertToStaff(int alertId, int staffId)
    {
        var alert = _alertMapper.FindById(alertId);
        if (alert is null)
        {
            return false;
        }
        alert.SetStaffId(staffId);
        try
        {
            _alertMapper.Update(alert);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateAlertStatus(int alertId, AlertStatus status)
    {
        var alert = _alertMapper.FindById(alertId);
        if (alert is null)
        {
            return false;
        }
        alert.SetAlertStatus(status);
        if (status == AlertStatus.RESOLVED)
        {
            alert.SetResolvedAt(DateTime.UtcNow);
        }

        try
        {
            _alertMapper.Update(alert);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ResolveAlert(int productId)
    {
        var alerts = _alertMapper.FindByProductId(productId) ?? new List<Alert>();
        var openAlerts = alerts.Where(a => a.GetAlertStatus() != AlertStatus.RESOLVED).ToList();

        foreach (var alert in openAlerts)
        {
            alert.SetAlertStatus(AlertStatus.RESOLVED);
            alert.SetResolvedAt(DateTime.UtcNow);
            _alertMapper.Update(alert);
        }

        return true;
    }

    public bool CheckLowStock(int productId, int threshold)
    {
        if (productId <= 0 || threshold < 0)
        {
            return false;
        }

        var currentStock = _inventoryQueryControl.CheckProductQuantityByStatus(productId, InventoryStatus.AVAILABLE);
        if (currentStock > threshold)
        {
            return false;
        }

        var alert = new Alert();

        alert.SetProductId(productId);
        alert.SetMinThreshold(threshold);
        alert.SetCurrentStock(currentStock);
        alert.SetAlertStatus(AlertStatus.OPEN);
        alert.SetMessage($"Product {productId} is low in stock ({currentStock} <= {threshold}).");
        alert.SetCreatedAt(DateTime.UtcNow);

        return CreateAlert(alert);
    }

    public void Update(int productId)
    {
        const int defaultThreshold = 5;
        _ = CheckLowStock(productId, defaultThreshold);
    }
}
