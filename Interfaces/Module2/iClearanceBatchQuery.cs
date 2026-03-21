using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Module2;

public interface iClearanceBatchQuery
{
    List<Clearancebatch> GetActiveBatches();
    Clearancebatch GetClearanceBatchById(int clearanceBatchId);
}
