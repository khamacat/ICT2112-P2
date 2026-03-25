namespace ProRental.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Interface for the TransactionLoggingController to interact with
/// TransactionFilterControl. Provides methods for displaying and
/// filtering transaction logs in the UI.
/// 
/// Implemented by TransactionFilterControl.
/// </summary>
public interface ITransactionLoggingUI
{
    /// <summary>
    /// Retrieves all transaction logs across all types.
    /// Also triggers PO pull to ensure latest POs are logged.
    /// </summary>
    List<Transactionlog> GetAllLogs();

    /// <summary>
    /// Retrieves transaction logs filtered by the given strategy.
    /// </summary>
    /// <param name="filterType">Type of filter: "customer", "supplier", "order", "daterange"</param>
    /// <param name="filterValue">The filter value (ID or date range as "startDate|endDate")</param>
    List<Transactionlog> GetFilteredLogs(string filterType, string filterValue);

    /// <summary>
    /// Retrieves a single transaction log with full details (including child log data).
    /// Used when the user clicks on a log entry to see the expanded DetailsJSON.
    /// </summary>
    Transactionlog? GetLogDetails(int transactionLogId);
}