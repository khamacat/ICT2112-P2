using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Alert
{
    private int? _staffid;
    private AlertStatus _status;
    private AlertStatus Status { get => _status; set => _status = value; }

    private DateTime? _resolvedat;

    public int GetAlertId() => _alertid;
    public void SetAlertId(int alertId) => _alertid = alertId;

    public int GetProductId() => _productid;
    public void SetProductId(int productId) => _productid = productId;

    public AlertStatus GetAlertStatus() => _status;
    public void SetAlertStatus(AlertStatus status) => _status = status;
    public void UpdateStatus(AlertStatus newValue) => _status = newValue;

    public int GetMinThreshold() => _minthreshold;
    public void SetMinThreshold(int minThreshold) => _minthreshold = minThreshold;

    public int GetCurrentStock() => _currentstock;
    public void SetCurrentStock(int currentStock) => _currentstock = currentStock;

    public string GetMessage() => _message;
    public void SetMessage(string message) => _message = message;

    public int? GetStaffId() => _staffid;
    public void SetStaffId(int? staffId) => _staffid = staffId;

    public DateTime GetCreatedAt() => _createdat;
    public void SetCreatedAt(DateTime createdAt) => _createdat = createdAt;

    public DateTime? GetResolvedAt() => _resolvedat;
    public void SetResolvedAt(DateTime? resolvedAt) => _resolvedat = resolvedAt;
}