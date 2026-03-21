using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Module2.P2_3;

public interface iInventoryCRUDControl
{
    bool CreateInventoryItem(int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate);
    Inventoryitem? GetInventoryItemById(int inventoryItemId);
    bool UpdateInventoryItem(int inventoryItemId, int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate);
    bool DeleteInventoryItem(int inventoryItemId);
}
