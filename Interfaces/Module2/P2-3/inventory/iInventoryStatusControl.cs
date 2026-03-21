using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Module2.P2_3;

public interface iInventoryStatusControl
{
    bool UpdateInventoryStatus(int inventoryItemId, InventoryStatus status);
}
