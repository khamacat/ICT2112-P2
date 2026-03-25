namespace ProRental.Data.Module2.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Gateway interface for the TransactionLog parent table.
/// Every log entry (rental, loan, return, clearance, PO) starts here.
/// </summary>
public interface ITransactionLogGateway
{
    /// <summary>
    /// Inserts a new TransactionLog row and returns the entity with the generated ID.
    /// </summary>
    Transactionlog Insert(Transactionlog log);

    /// <summary>
    /// Retrieves all transaction log entries, ordered by CreatedAt descending.
    /// </summary>
    List<Transactionlog> GetAll();

    /// <summary>
    /// Retrieves a single transaction log entry by ID. Returns null if not found.
    /// </summary>
    Transactionlog? GetById(int transactionLogId);

    /// <summary>
    /// Deletes a transaction log entry by ID. Child rows cascade-delete automatically.
    /// </summary>
    bool Delete(int transactionLogId);
}