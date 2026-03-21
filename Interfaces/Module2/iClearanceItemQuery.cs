using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Module2;

public interface iClearanceItemQuery
{
    Clearanceitem GetClearanceItemById(int clearanceItemId);
    List<Clearanceitem> GetClearanceItemsByBatch(int batchId);
    List<Clearanceitem> GetClearanceItemsByStatus(string status);
}
