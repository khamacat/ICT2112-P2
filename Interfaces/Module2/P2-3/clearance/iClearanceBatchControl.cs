using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface iClearanceBatchControl
{
    bool CreateClearanceBatch(Clearancebatch clearanceBatch);
    List<Clearancebatch> GetAllBatches();
    bool UpdateClearanceBatch(Clearancebatch clearanceBatch);
    bool DeleteClearanceBatch(int clearanceBatchId);

    // Lifecycle
    bool ScheduleBatch(int clearanceBatchId);
    bool ActivateBatch(int clearanceBatchId);
    bool CloseBatch(int clearanceBatchId);

    // Validation methods have been encapsulated

}
