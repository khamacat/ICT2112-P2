using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Inventoryitem
{
    private InventoryStatus _status;
    private InventoryStatus Status { get => _status; set => _status = value; }
    public void UpdateStatus(InventoryStatus newValue) => _status = newValue;
}