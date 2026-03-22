using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iClearanceItemQuery
{
    Clearanceitem GetClearanceItemById(int clearanceItemId);
    List<Clearanceitem> GetClearanceItemsByBatch(int batchId);
    List<Clearanceitem> GetClearanceItemsByStatus(string status);
}
