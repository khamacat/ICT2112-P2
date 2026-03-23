namespace ProRental.Data.Module2.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Gateway interface for the ReturnLog child table.
/// Each row's PK (ReturnLogId) must match an existing TransactionLogID.
/// </summary>
public interface IReturnLogGateway
{
    Returnlog Insert(Returnlog log);
    List<Returnlog> GetAll();
    Returnlog? GetById(int returnLogId);
    bool ExistsByReturnRequestId(int returnRequestId);
}