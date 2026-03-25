namespace ProRental.Interfaces;

/// <summary>
/// Stub interface for TransactionLogService (implemented by another team).
/// AnalyticsControl depends on this to read transaction logs for analytics generation.
/// </summary>
public interface ITransactionLogService
{
    Task<IEnumerable<TransactionLogDto>> GetAllTransactionLogsAsync();
    Task<IEnumerable<TransactionLogDto>> GetTransactionLogsByDateRangeAsync(DateTime start, DateTime end);
}

/// <summary>
/// DTO representing a unified view of any transaction log entry.
/// Populated from TransactionLog joined with its specific log table.
/// </summary>
public class TransactionLogDto
{
    public int LogID { get; set; }
    public string LogType { get; set; } = string.Empty;   // RENTAL_ORDER, LOAN, RETURN, PURCHASE_ORDER, CLEARANCE
    public DateTime CreatedAt { get; set; }
    public int? SupplierID { get; set; }
    public string? SupplierName { get; set; }
    public int? ProductID { get; set; }
    public List<string> ProductNames { get; set; } = new();
    public string? Summary { get; set; }                   // human-readable one-liner
}
