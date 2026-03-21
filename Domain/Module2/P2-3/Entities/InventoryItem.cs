using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Inventoryitem
{
    private InventoryStatus? _status;
    private InventoryStatus? Status { get => _status; set => _status = value; }

    public int GetInventoryItemId() => _inventoryid;
    public void SetInventoryItemId(int id) => _inventoryid = id;

    public int GetProductId() => _productid;
    public void SetProductId(int productId) => _productid = productId;

    public string GetSerialNumber() => _serialnumber;
    public void SetSerialNumber(string serialNumber) => _serialnumber = serialNumber;

    public InventoryStatus? GetStatus() => _status;
    public void SetStatus(InventoryStatus status) => _status = status;
    public void UpdateStatus(InventoryStatus newValue) => _status = newValue;

    public DateTime GetCreatedDate() => _createdat;
    public void SetCreatedDate(DateTime createdDate) => _createdat = createdDate;

    public DateTime GetUpdatedDate() => _updatedat;
    public void SetUpdatedDate(DateTime updatedDate) => _updatedat = updatedDate;

    public DateTime? GetExpiryDate() => _expirydate;
    public void SetExpiryDate(DateTime? expiryDate) => _expirydate = expiryDate;
}