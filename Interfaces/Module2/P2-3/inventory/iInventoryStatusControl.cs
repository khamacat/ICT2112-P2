using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface iInventoryStatusControl
{
    /// <summary>
    /// Updates inventory status and automatically syncs product availability status.
    /// If available quantity becomes 0, product is marked as unavailable.
    /// If available quantity becomes > 0, product is marked as available.
    bool UpdateInventoryStatus(int inventoryItemId, InventoryStatus status);
}
