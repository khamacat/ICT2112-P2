using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iClearanceBatchQuery
{
    List<Clearancebatch> GetActiveBatches();
    Clearancebatch GetClearanceBatchById(int clearanceBatchId);
}
