using System.Text.Json;
using ProRental.Interfaces; 
using ProRental.Interfaces.Data; 
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class TransactionLogEnricher : ILoanLogEnricher, IReturnLogEnricher, IClearanceLogEnricher
{
    private readonly IInventoryTransactionLogger _externalLogger;
    
    // STRICT ISP: Only Read-Only Mappers are injected here!
    private readonly ILoanListRead _loanListRead;
    private readonly ILoanItemRead _loanItemRead;
    private readonly IReturnRequestRead _returnRequestRead;
    private readonly IReturnItemRead _returnItemRead;
    private readonly IClearanceBatchRead _clearanceBatchRead;
    private readonly IClearanceItemRead _clearanceItemRead;
    
    // Domain Queries (Safe, no circular dependency)
    private readonly iInventoryQueryControl _inventoryQuery;
    private readonly IProductQuery _productQuery;

    public TransactionLogEnricher(
        IInventoryTransactionLogger externalLogger,
        ILoanListRead loanListRead,
        ILoanItemRead loanItemRead,
        IReturnRequestRead returnRequestRead,
        IReturnItemRead returnItemRead,
        IClearanceBatchRead clearanceBatchRead,
        IClearanceItemRead clearanceItemRead,
        iInventoryQueryControl inventoryQuery,
        IProductQuery productQuery)
    {
        _externalLogger = externalLogger;
        _loanListRead = loanListRead;
        _loanItemRead = loanItemRead;
        _returnRequestRead = returnRequestRead;
        _returnItemRead = returnItemRead;
        _clearanceBatchRead = clearanceBatchRead;
        _clearanceItemRead = clearanceItemRead;
        _inventoryQuery = inventoryQuery;
        _productQuery = productQuery;
    }

    // ── FEATURE 1: LOAN LOGGING ──────────────────────────────────────────────
    public void LogLoanProcess(int loanListId, int orderId)
    {
        var loanList = _loanListRead.FindById(loanListId); 
        if (loanList == null) return;

        var specificLoanItems = _loanItemRead.FindByLoanListId(loanListId); 
        if (specificLoanItems == null) return;

        var logItems = new List<LoanLogItemFormat>();
        foreach (var item in specificLoanItems)
        {
            int invId = item.GetInventoryItemId();
            var invItem = _inventoryQuery.GetInventoryItemById(invId);
            string serialNum = invItem?.GetSerialNumber() ?? "UNKNOWN";
            
            string productName = "UNKNOWN";
            if (invItem != null)
            {
                var product = _productQuery.GetProductById(invItem.GetProductId());
                productName = product?.GetProductdetail()?.GetName() ?? "UNKNOWN";
            }

            logItems.Add(new LoanLogItemFormat(item.GetLoanItemId(), serialNum, productName, item.GetRemarks()));
        }

        var logDetails = new LoanLogDetailsFormat(loanList.GetRemarks() ?? "Loan Processed via System", logItems);
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        string detailsJson = JsonSerializer.Serialize(logDetails, jsonOptions);

        _externalLogger.pushLoanData(loanListId, orderId, loanList.GetStatus().ToString(), loanList.GetLoanDate(), loanList.GetReturnDate(), loanList.GetDueDate(), detailsJson);
    }

    // ── FEATURE 2: RETURN LOGGING ────────────────────────────────────────────
    public void LogReturnProcess(int returnRequestId, int orderId)
    {
        var returnReq = _returnRequestRead.FindById(returnRequestId); 
        if (returnReq == null) return;

        var returnItems = _returnItemRead.FindByReturnRequest(returnRequestId); 
        if (returnItems == null) return;

        var logItems = new List<ReturnLogItemFormat>();
        foreach (var item in returnItems)
        {
            int invId = item.GetInventoryItemId();
            var invItem = _inventoryQuery.GetInventoryItemById(invId);
            string serialNum = invItem?.GetSerialNumber() ?? "UNKNOWN";
            
            string productName = "UNKNOWN";
            if (invItem != null)
            {
                var product = _productQuery.GetProductById(invItem.GetProductId());
                productName = product?.GetProductdetail()?.GetName() ?? "UNKNOWN";
            }

            logItems.Add(new ReturnLogItemFormat(item.GetReturnItemId(), serialNum, productName, item.GetStatus().ToString(), item.GetCompletionDate()?.ToString("yyyy-MM-dd"), item.GetImageUrl()));
        }

        string remarks = returnReq.GetStatus() == ProRental.Domain.Enums.ReturnRequestStatus.PROCESSING ? "Return Initiated" : "Return Completed";
        var logDetails = new ReturnLogDetailsFormat(remarks, logItems);
        
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        string detailsJson = JsonSerializer.Serialize(logDetails, jsonOptions);

        _externalLogger.pushReturnData(returnRequestId, orderId, returnReq.GetCustomerId().ToString(), returnReq.GetStatus().ToString(), returnReq.GetRequestDate(), returnReq.GetCompletionDate(), null, detailsJson);
    }

    // ── FEATURE 3: CLEARANCE LOGGING ─────────────────────────────────────────
    public void LogClearanceProcess(int clearanceBatchId)
    {
        var batch = _clearanceBatchRead.FindById(clearanceBatchId); 
        if (batch == null) return;

        var clearanceItems = _clearanceItemRead.FindByBatchId(clearanceBatchId); 
        if (clearanceItems == null) return;

        var logItems = new List<ClearanceLogItemFormat>();
        foreach (var item in clearanceItems)
        {
            int invId = item.GetInventoryItemId();
            var invItem = _inventoryQuery.GetInventoryItemById(invId);
            string serialNum = invItem?.GetSerialNumber() ?? "UNKNOWN";

            string productName = "UNKNOWN";
            decimal originalPrice = 0m;
            
            if (invItem != null)
            {
                var product = _productQuery.GetProductById(invItem.GetProductId());
				var productDetail = product?.GetProductdetail();

                if (productDetail != null)
                {
                    // Now the compiler knows for a fact this local variable is safe
                    productName = productDetail.GetName();
                    originalPrice = productDetail.GetPrice();
                }
            }

            decimal clearancePrice = item.GetFinalPrice() ?? item.GetRecommendedPrice() ?? 0m;
            logItems.Add(new ClearanceLogItemFormat(item.GetClearanceItemId(), serialNum, productName, "Standard Clearance", originalPrice, clearancePrice));
        }

        var logDetails = new ClearanceLogDetailsFormat(batch.GetStatus().ToString(), clearanceItems.Count(), logItems);
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        string detailsJson = JsonSerializer.Serialize(logDetails, jsonOptions);

        _externalLogger.pushClearanceLogData(clearanceBatchId, batch.GetBatchName(), batch.GetClearanceDate(), batch.GetStatus().ToString(), detailsJson);
    }
}

// ── FORMATTING RECORDS ──
internal record LoanLogItemFormat(int loanItemId, string serialNumber, string productName, string? remarks);
internal record LoanLogDetailsFormat(string remarks, List<LoanLogItemFormat> items);
internal record ReturnLogItemFormat(int returnItemId, string serialNumber, string productName, string status, string? completionDate, string? image);
internal record ReturnLogDetailsFormat(string remarks, List<ReturnLogItemFormat> items);
internal record ClearanceLogItemFormat(int clearanceItemId, string serialNumber, string productName, string reason, decimal originalPrice, decimal clearancePrice);
internal record ClearanceLogDetailsFormat(string batchStatus, int totalItemsCleared, List<ClearanceLogItemFormat> items);