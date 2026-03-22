using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface iInventoryCRUDControl
{
    bool CreateInventoryItem(int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate);
    int CreateBulkInventoryItems(int productId, int quantity);
    Inventoryitem? GetInventoryItemById(int inventoryItemId);
    bool UpdateInventoryItem(int inventoryItemId, int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate);
    bool DeleteInventoryItem(int inventoryItemId);
}
