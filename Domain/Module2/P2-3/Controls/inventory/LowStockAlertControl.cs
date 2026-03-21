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

    public bool CreateAlert(int productId, int minThreshold, int staffId = 0)
    {
        if (productId <= 0 || minThreshold < 0)
        {
            return false;
        }

        var alert = new Alert();
        alert.SetProductId(productId);
        alert.SetStaffId(staffId);
        alert.SetMinThreshold(minThreshold);
        alert.SetAlertStatus(AlertStatus.OPEN);
        alert.SetCreatedAt(DateTime.UtcNow);

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

    public List<Alert> GetAllAlerts()
    {
        return _alertMapper.FindAll()?.ToList() ?? new List<Alert>();
    }

    public Alert? GetAlertById(int alertId)
    {
        return _alertMapper.FindById(alertId);
    }

    public List<Alert> GetAlertsByThreshold(int threshold)
    {
        var allAlerts = _alertMapper.FindAll();
        if (allAlerts is null)
        {
            return new List<Alert>();
        }

        return allAlerts.Where(a => a.GetMinThreshold() == threshold).ToList();
    }

    public bool UpdateAlertThreshold(int alertId, int newThreshold)
    {
        if (alertId <= 0 || newThreshold < 0)
        {
            return false;
        }

        var alert = _alertMapper.FindById(alertId);
        if (alert is null)
        {
            return false;
        }

        alert.SetMinThreshold(newThreshold);

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

    public bool ResolveAlert(int alertId)
    {
        if (alertId <= 0)
        {
            return false;
        }

        var alert = _alertMapper.FindById(alertId);
        if (alert is null)
        {
            return false;
        }

        alert.SetAlertStatus(AlertStatus.RESOLVED);
        alert.SetResolvedAt(DateTime.UtcNow);

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

        // Create alert with no assigned staff (staffId defaults to 0)
        return CreateAlert(productId, threshold);
    }

    public void Update(int productId)
    {
        const int defaultThreshold = 5;
        _ = CheckLowStock(productId, defaultThreshold);
    }
}
