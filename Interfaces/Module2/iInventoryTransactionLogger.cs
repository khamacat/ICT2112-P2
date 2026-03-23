namespace ProRental.Interfaces;

/// <summary>
/// Push interface for P2-3 (Inventory module) to log loans, returns, and clearance.
/// Implemented by TransactionLogControl.
/// 
/// Usage by P2-3:
///   _inventoryLogger.pushLoanData(loanListId, rentalOrderLogId, ...);
///   _inventoryLogger.pushReturnData(returnRequestId, rentalOrderLogId, ...);
///   _inventoryLogger.pushClearanceLogData(clearanceBatchId, batchName, ...);
/// </summary>
public interface IInventoryTransactionLogger
{
    void pushLoanData(int loanListId, int rentalOrderLogId, string status,
                      DateTime? loanDate, DateTime? returnDate, DateTime? dueDate,
                      string? detailsJson);

    void pushReturnData(int returnRequestId, int rentalOrderLogId, string? customerId,
                        string status, DateTime? requestDate, DateTime? completionDate,
                        string? imageUrl, string? detailsJson);

    void pushClearanceLogData(int clearanceBatchId, string? batchName,
                              DateTime? clearanceDate, string status,
                              string? detailsJson);
}