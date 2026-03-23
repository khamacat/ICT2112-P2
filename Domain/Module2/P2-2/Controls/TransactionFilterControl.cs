namespace ProRental.Domain.Module2.P2_2.Controls;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Interfaces;
using ProRental.Data.Module2.Interfaces;
using ProRental.Domain.Module2.P2_2.Strategy;

/// <summary>
/// Control class responsible for READING and FILTERING transaction logs.
/// - Serves the TransactionLoggingController (via ITransactionLoggingUI)
/// - Serves the Analytics teammate (via ITransactionLogService)
/// - Uses IFilterStrategy pattern for flexible filtering
/// - Calls TransactionLogControl to pull PO data before displaying
/// 
/// Implements: ITransactionLoggingUI, ITransactionLogService
/// </summary>
public class TransactionFilterControl : ITransactionLoggingUI /* TODO: also implement ITransactionLogService */
{
    private readonly ITransactionLogGateway _transactionLogGateway;
    private readonly IRentalOrderLogGateway _rentalOrderLogGateway;
    private readonly ILoanLogGateway _loanLogGateway;
    private readonly IReturnLogGateway _returnLogGateway;
    private readonly IClearanceLogGateway _clearanceLogGateway;
    private readonly IPurchaseOrderLogGateway _purchaseOrderLogGateway;
    private readonly TransactionLogControl _transactionLogControl;

    public TransactionFilterControl(
        ITransactionLogGateway transactionLogGateway,
        IRentalOrderLogGateway rentalOrderLogGateway,
        ILoanLogGateway loanLogGateway,
        IReturnLogGateway returnLogGateway,
        IClearanceLogGateway clearanceLogGateway,
        IPurchaseOrderLogGateway purchaseOrderLogGateway,
        TransactionLogControl transactionLogControl)
    {
        _transactionLogGateway = transactionLogGateway;
        _rentalOrderLogGateway = rentalOrderLogGateway;
        _loanLogGateway = loanLogGateway;
        _returnLogGateway = returnLogGateway;
        _clearanceLogGateway = clearanceLogGateway;
        _purchaseOrderLogGateway = purchaseOrderLogGateway;
        _transactionLogControl = transactionLogControl;
    }

    // ── ITransactionLoggingUI ───────────────────────────────────

    public List<Transactionlog> GetAllLogs()
    {
        // Pull latest PO data before displaying
        _transactionLogControl.PullAndLogPurchaseOrders();

        // Get all transaction logs with their child log data loaded
        return LoadAllLogsWithChildren();
    }

    public List<Transactionlog> GetFilteredLogs(string filterType, string filterValue)
    {
        // Pull latest PO data before filtering
        _transactionLogControl.PullAndLogPurchaseOrders();

        // Create the appropriate strategy based on filter type
        IFilterStrategy strategy = CreateStrategy(filterType, filterValue);

        // Validate the filter input
        if (!strategy.validate())
        {
            // If validation fails, return all logs (no filter applied)
            return LoadAllLogsWithChildren();
        }

        // Load all logs and apply the filter
        var allLogs = LoadAllLogsWithChildren();
        return strategy.filter(allLogs);
    }

    public Transactionlog? GetLogDetails(int transactionLogId)
    {
        // Load the specific log with its child data
        var log = _transactionLogGateway.GetById(transactionLogId);
        if (log == null) return null;

        // Load the child log based on the log type
        LoadChildLog(log);
        return log;
    }

    // ── Private Helpers ─────────────────────────────────────────

    /// <summary>
    /// Creates the appropriate filter strategy based on the filter type string.
    /// </summary>
    private IFilterStrategy CreateStrategy(string filterType, string filterValue)
    {
        return filterType.ToLower() switch
        {
            "customer" => new FilterByCustomerId(filterValue),
            "supplier" => new FilterBySupplierId(filterValue),
            "order" => new FilterByOrderId(filterValue),
            "daterange" => new FilterByDateRange(filterValue),
            _ => new FilterByDateRange(DateTime.MinValue, DateTime.MaxValue) // no-op filter
        };
    }

    /// <summary>
    /// Loads all TransactionLog entries with their associated child log data.
    /// </summary>
    private List<Transactionlog> LoadAllLogsWithChildren()
    {
        var logs = _transactionLogGateway.GetAll();

        foreach (var log in logs)
        {
            LoadChildLog(log);
        }

        return logs;
    }

    /// <summary>
    /// Loads the child log entity for a given TransactionLog entry.
    /// </summary>
    private void LoadChildLog(Transactionlog log)
    {
        if (log.Rentalorderlog == null && log.Loanlog == null && log.Returnlog == null
            && log.Clearancelog == null && log.Purchaseorderlog == null)
        {
            var rental = _rentalOrderLogGateway.GetById(log.transactionlogid);
            var loan = _loanLogGateway.GetById(log.transactionlogid);
            var returnLog = _returnLogGateway.GetById(log.transactionlogid);
            var clearance = _clearanceLogGateway.GetById(log.transactionlogid);
            var po = _purchaseOrderLogGateway.GetById(log.transactionlogid);
        }
    }
}