using ProRental.Domain.Enums;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Module2.P2_3;

public interface iInventoryQueryControl
{
    List<Inventoryitem> GetInventoryByProduct(int productId);
    List<Inventoryitem> GetInventoryByStatus(InventoryStatus status);
    int GetTotalStockCount(int productId);
    bool ValidateInventoryItem(Inventoryitem inventoryItem);
    bool CheckInventoryConflicts(Inventoryitem inventoryItem);
    int CheckProductQuantityByStatus(int productId, InventoryStatus status);
}
