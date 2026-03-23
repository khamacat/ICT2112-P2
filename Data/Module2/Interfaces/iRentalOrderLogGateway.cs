namespace ProRental.Data.Module2.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Gateway interface for the RentalOrderLog child table.
/// Each row's PK (RentalOrderLogId) must match an existing TransactionLogID.
/// </summary>
public interface IRentalOrderLogGateway
{
    /// <summary>
    /// Inserts a rental order log entry. The entity's rentalorderlogid must already
    /// match an existing TransactionLogID (created via ITransactionLogGateway.Insert).
    /// </summary>
    Rentalorderlog Insert(Rentalorderlog log);

    /// <summary>
    /// Retrieves all rental order log entries, ordered by CreatedAt descending.
    /// </summary>
    List<Rentalorderlog> GetAll();

    /// <summary>
    /// Retrieves a single rental order log by its ID.
    /// </summary>
    Rentalorderlog? GetById(int rentalOrderLogId);

    /// <summary>
    /// Checks whether a rental order log already exists for the given OrderId.
    /// Used to prevent duplicate logging.
    /// </summary>
    bool ExistsByOrderId(int orderId);
}