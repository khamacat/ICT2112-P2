using ProRental.Domain.Enums;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class LowStockAlertControl : iAlertControl, iStockObserver
{
    private readonly IAlertMapper _alertMapper;
    private readonly IProductStatusControl _productStatusControl;

    public LowStockAlertControl(IAlertMapper alertMapper, IProductStatusControl productStatusControl)
    {
        _alertMapper = alertMapper ?? throw new ArgumentNullException(nameof(alertMapper));
        _productStatusControl = productStatusControl ?? throw new ArgumentNullException(nameof(productStatusControl));
    }

    public bool CreateAlert(Alert alert)
    {
        if (alert is null)
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

    public bool CheckLowStock(int productId, int availableCount)
    {
        if (productId <= 0)
        {
            return false;
        }

        // Get the product's configured threshold value from IProductStatusControl
        int minThreshold = _productStatusControl.GetThresholdQuantityForProduct(productId);
        
        // Check if stock is below threshold
        if (availableCount > minThreshold)
        {
            return false;
        }

        // Build and create the alert
        var alert = new Alert();
        alert.SetProductId(productId);
        alert.SetStaffId(0); // No assigned staff
        alert.SetMinThreshold(minThreshold);
        alert.SetAlertStatus(AlertStatus.OPEN);
        alert.SetCreatedAt(DateTime.UtcNow);

        return CreateAlert(alert);
    }

    public void Update(int productId, int availableCount)
    {
        _ = CheckLowStock(productId, availableCount);
    }
}
