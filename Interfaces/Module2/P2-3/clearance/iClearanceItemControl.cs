using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iClearanceItemControl
{
    bool CreateClearanceItem(Clearanceitem clearanceItem);
    bool UpdateClearanceItem(Clearanceitem clearanceItem);
    bool DeleteClearanceItem(int clearanceItemId);
    bool AddItemToBatch(int batchId, int inventoryItemId);
    bool AddItemsToBatch(int batchId, List<int> inventoryItemIds);
    bool RemoveItemFromBatch(int clearanceItemId);
    bool UpdateClearanceItemStatus(int clearanceItemId, string status);
    decimal CalculateClearancePrice(int clearanceItemId);
    bool RecordSale(int clearanceItemId, decimal finalPrice, int staffId);
    List<Inventoryitem> GetEligibleItemsForClearance();

}
