using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Module2.P2_3;

public interface iInventoryCRUDControl
{
    bool CreateInventoryItem(Inventoryitem inventoryItem);
    Inventoryitem? GetInventoryItemById(int inventoryItemId);
    bool UpdateInventoryItem(Inventoryitem inventoryItem);
    bool DeleteInventoryItem(int inventoryItemId);
}
