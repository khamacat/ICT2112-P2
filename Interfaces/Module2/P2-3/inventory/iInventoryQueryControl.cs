using ProRental.Domain.Enums;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iInventoryQueryControl
{
    Inventoryitem? GetInventoryItemById(int inventoryItemId);
    List<Inventoryitem> GetInventoryByProduct(int productId);
    List<Inventoryitem> GetInventoryByStatus(InventoryStatus status);
    int CheckProductQuantityByStatus(int productId, InventoryStatus status);
    List<Inventoryitem> GetAllInventoryItems();
    List<Inventoryitem> SearchInventoryItems(string query);
    List<int> AllocateAvailableItems(int productId, int quantity);
}
