using ProRental.Domain.Enums;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iInventoryQueryControl
{
    List<Inventoryitem> GetInventoryByProduct(int productId);
    List<Inventoryitem> GetInventoryByStatus(InventoryStatus status);
    int GetTotalStockCount(int productId);
    int CheckProductQuantityByStatus(int productId, InventoryStatus status);
    List<Inventoryitem> GetAllInventoryItems();
    List<Inventoryitem> SearchInventoryItems(string query);
    List<int> AllocateAvailableItems(int productId, int quantity);
}
