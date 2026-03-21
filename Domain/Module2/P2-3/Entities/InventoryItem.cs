using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Inventoryitem
{
    private InventoryStatus? _status;
    private InventoryStatus? Status { get => _status; set => _status = value; }

    public int GetInventoryItemId() => _inventoryid;
    internal void SetInventoryItemId(int id) => _inventoryid = id;

    public int GetProductId() => _productid;
    internal void SetProductId(int productId) => _productid = productId;

    public string GetSerialNumber() => _serialnumber;
    internal void SetSerialNumber(string serialNumber) => _serialnumber = serialNumber;

    public InventoryStatus? GetStatus() => _status;
    internal void SetStatus(InventoryStatus status) => _status = status;
    internal void UpdateStatus(InventoryStatus newValue) => _status = newValue;

    public DateTime GetCreatedDate() => _createdat;
    internal void SetCreatedDate(DateTime createdDate) => _createdat = createdDate;

    public DateTime GetUpdatedDate() => _updatedat;
    internal void SetUpdatedDate(DateTime updatedDate) => _updatedat = updatedDate;

    public DateTime? GetExpiryDate() => _expirydate;
    internal void SetExpiryDate(DateTime? expiryDate) => _expirydate = expiryDate;
}