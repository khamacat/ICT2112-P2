namespace ProRental.Data.Module2.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Gateway interface for the ClearanceLog child table.
/// Each row's PK (ClearanceLogId) must match an existing TransactionLogID.
/// </summary>
public interface IClearanceLogGateway
{
    Clearancelog Insert(Clearancelog log);
    List<Clearancelog> GetAll();
    Clearancelog? GetById(int clearanceLogId);
    bool ExistsByClearanceBatchId(int clearanceBatchId);
}