using ProRental.Domain.Enums;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class LowStockAlertControl : iAlertControl, iStockObserver
{
    private readonly IAlertMapper _alertMapper;
    private readonly IProductActions _productActions;

    public LowStockAlertControl(IAlertMapper alertMapper, IProductActions productActions)
    {
        _alertMapper = alertMapper ?? throw new ArgumentNullException(nameof(alertMapper));
        _productActions = productActions ?? throw new ArgumentNullException(nameof(productActions));
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

    public List<Alert> GetAllAlerts()
    {
        return _alertMapper.FindAll()?.ToList() ?? new List<Alert>();
    }

    public List<Alert> GetAlertsByProduct(int productId)
    {
        return _alertMapper.FindByProductId(productId)?.ToList() ?? new List<Alert>();
    }

    public bool UpdateAlertStatus(int alertId, AlertStatus status)
    {
        var alert = _alertMapper.FindById(alertId);
        if (alert is null)
        {
            return false;
        }
        alert.UpdateStatus(status);

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
        int minThreshold = _productActions.GetThresholdQuantityForProduct(productId);
        
        // Check if stock is below threshold
        if (availableCount > minThreshold)
        {
            return false;
        }

        // spam prevention step, prevent multiple alerts for the same product
        var existingAlerts = GetAlertsByProduct(productId);

        if (existingAlerts.Any(a => a.GetAlertStatus() == AlertStatus.OPEN))
            return false;

        // Build and create the alert
        var alert = Alert.Create(
            productId,
            minThreshold,
            AlertStatus.OPEN,
            $"Automatic Alert: Stock has fallen below the minimum threshold of {minThreshold}.",
            availableCount
        );

        return CreateAlert(alert);
    }

    /// <summary>
    /// Gets all alerts with status OPEN. Used for displaying current active alerts.
    /// </summary>
    public List<Alert> GetOpenAlerts()
    {
        return _alertMapper.FindAll()?
            .Where(a => a.GetAlertStatus() == AlertStatus.OPEN)
            .ToList() ?? new List<Alert>();
    }

    /// <summary>
    /// Automatically resolves all OPEN and ACKNOWLEDGED alerts for a product if the current stock
    /// has gone above the product's minimum threshold.
    /// </summary>
    public bool AutoResolveAlertsIfStockAboveThreshold(int productId, int currentStock)
    {
        if (productId <= 0 || currentStock < 0)
        {
            return false;
        }

        try
        {
            // Get the product's configured threshold value
            int minThreshold = _productActions.GetThresholdQuantityForProduct(productId);

            // If stock is NOT above threshold, nothing to resolve
            if (currentStock <= minThreshold)
            {
                return false;
            }

            // Get all non-RESOLVED alerts for this product
            var alertsToResolve = GetAlertsByProduct(productId)?
                .Where(a => a.GetAlertStatus() != AlertStatus.RESOLVED)
                .ToList() ?? new List<Alert>();

            if (!alertsToResolve.Any())
            {
                return true; // No alerts to resolve, but operation is successful
            }

            // Resolve each alert
            bool allResolved = true;
            foreach (var alert in alertsToResolve)
            {
                if (!UpdateAlertStatus(alert.GetAlertId(), AlertStatus.RESOLVED))
                {
                    allResolved = false;
                }
            }

            return allResolved;
        }
        catch
        {
            return false;
        }
    }

    public void Update(int productId, int availableCount)
    {
        // Check if stock has fallen below threshold and create alert if needed
        _ = CheckLowStock(productId, availableCount);
        
        // Check if stock has risen above threshold and auto-resolve any open alerts
        _ = AutoResolveAlertsIfStockAboveThreshold(productId, availableCount);
    }
}
