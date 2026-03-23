namespace ProRental.Domain.Module2.P2_2.Controls;

using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces;
using ProRental.Data.Module2.Interfaces;

/// <summary>
/// Control class responsible for WRITING transaction logs.
/// - Receives pushed data from P2-6 (rental orders) and P2-3 (loans, returns, clearance)
/// - Pulls purchase order data from IPurchaseOrderService and logs new POs
/// 
/// Implements: IRentalOrderLogger, IInventoryTransactionLogger
/// </summary>
public class TransactionLogControl : IRentalOrderLogger, IInventoryTransactionLogger
{
    private readonly ITransactionLogGateway _transactionLogGateway;
    private readonly IRentalOrderLogGateway _rentalOrderLogGateway;
    private readonly ILoanLogGateway _loanLogGateway;
    private readonly IReturnLogGateway _returnLogGateway;
    private readonly IClearanceLogGateway _clearanceLogGateway;
    private readonly IPurchaseOrderLogGateway _purchaseOrderLogGateway;
    private readonly IPurchaseOrderService _purchaseOrderService;

    public TransactionLogControl(
        ITransactionLogGateway transactionLogGateway,
        IRentalOrderLogGateway rentalOrderLogGateway,
        ILoanLogGateway loanLogGateway,
        IReturnLogGateway returnLogGateway,
        IClearanceLogGateway clearanceLogGateway,
        IPurchaseOrderLogGateway purchaseOrderLogGateway,
        IPurchaseOrderService purchaseOrderService)
    {
        _transactionLogGateway = transactionLogGateway;
        _rentalOrderLogGateway = rentalOrderLogGateway;
        _loanLogGateway = loanLogGateway;
        _returnLogGateway = returnLogGateway;
        _clearanceLogGateway = clearanceLogGateway;
        _purchaseOrderLogGateway = purchaseOrderLogGateway;
        _purchaseOrderService = purchaseOrderService;
    }

    // ── IRentalOrderLogger ──────────────────────────────────────

    public void pushRentalOrderData(int orderId, int customerId, DateTime orderDate,
                                     decimal totalAmount, string deliveryType, string status,
                                     string? detailsJson)
    {
        if (_rentalOrderLogGateway.ExistsByOrderId(orderId))
            return;

        var transactionLog = new Transactionlog
        {
            logtype = LogType.RENTAL_ORDER,
            createdat = DateTime.UtcNow
        };
        _transactionLogGateway.Insert(transactionLog);

        var rentalLog = new Rentalorderlog
        {
            rentalorderlogid = transactionLog.transactionlogid,
            orderid = orderId,
            customerid = customerId,
            orderdate = orderDate,
            totalamount = totalAmount,
            detailsjson = detailsJson
        };

        // TODO: Set enum fields once mappings are confirmed
        // rentalLog.deliverytype = Enum.Parse<DeliveryType>(deliveryType);
        // rentalLog.status = Enum.Parse<RentalStatus>(status);

        _rentalOrderLogGateway.Insert(rentalLog);
    }

    // ── IInventoryTransactionLogger ─────────────────────────────

    public void pushLoanData(int loanListId, int rentalOrderLogId, string status,
                              DateTime? loanDate, DateTime? returnDate, DateTime? dueDate,
                              string? detailsJson)
    {
        if (_loanLogGateway.ExistsByLoanListId(loanListId))
            return;

        var transactionLog = new Transactionlog
        {
            logtype = LogType.LOAN,
            createdat = DateTime.UtcNow
        };
        _transactionLogGateway.Insert(transactionLog);

        var loanLog = new Loanlog
        {
            loanlogid = transactionLog.transactionlogid,
            loanlistid = loanListId,
            rentalorderlogid = rentalOrderLogId,
            loandate = loanDate,
            returndate = returnDate,
            duedate = dueDate,
            detailsjson = detailsJson
        };

        // TODO: Set enum field once mapping is confirmed
        // loanLog.status = Enum.Parse<LoanLogStatus>(status);

        _loanLogGateway.Insert(loanLog);
    }

    public void pushReturnData(int returnRequestId, int rentalOrderLogId, string? customerId,
                                string status, DateTime? requestDate, DateTime? completionDate,
                                string? imageUrl, string? detailsJson)
    {
        if (_returnLogGateway.ExistsByReturnRequestId(returnRequestId))
            return;

        var transactionLog = new Transactionlog
        {
            logtype = LogType.RETURN,
            createdat = DateTime.UtcNow
        };
        _transactionLogGateway.Insert(transactionLog);

        var returnLog = new Returnlog
        {
            returnlogid = transactionLog.transactionlogid,
            returnrequestid = returnRequestId,
            rentalorderlogid = rentalOrderLogId,
            customerid = customerId,
            requestdate = requestDate,
            completiondate = completionDate,
            imageurl = imageUrl,
            detailsjson = detailsJson
        };

        // TODO: Set enum field once mapping is confirmed
        // returnLog.status = Enum.Parse<ReturnStatus>(status);

        _returnLogGateway.Insert(returnLog);
    }

    public void pushClearanceLogData(int clearanceBatchId, string? batchName,
                                      DateTime? clearanceDate, string status,
                                      string? detailsJson)
    {
        if (_clearanceLogGateway.ExistsByClearanceBatchId(clearanceBatchId))
            return;

        var transactionLog = new Transactionlog
        {
            logtype = LogType.CLEARANCE,
            createdat = DateTime.UtcNow
        };
        _transactionLogGateway.Insert(transactionLog);

        var clearanceLog = new Clearancelog
        {
            clearancelogid = transactionLog.transactionlogid,
            clearancebatchid = clearanceBatchId,
            batchname = batchName,
            clearancedate = clearanceDate,
            detailsjson = detailsJson
        };

        // TODO: Set enum field once mapping is confirmed
        // clearanceLog.status = Enum.Parse<ClearanceLogStatus>(status);

        _clearanceLogGateway.Insert(clearanceLog);
    }

    // ── Purchase Order Pull ─────────────────────────────────────

    /// <summary>
    /// Pulls purchase order data from IPurchaseOrderService and logs any new POs.
    /// Called by TransactionFilterControl before displaying logs.
    /// Checks for duplicates — only logs POs that haven't been logged yet.
    /// </summary>
    public void PullAndLogPurchaseOrders()
    {
        var allPOs = _purchaseOrderService.GetAllPurchaseOrders();

        foreach (var po in allPOs)
        {
            // Skip if already logged
            if (_purchaseOrderLogGateway.ExistsByPoId(po.PoId))
                continue;

            var transactionLog = new Transactionlog
            {
                logtype = LogType.PURCHASE_ORDER,
                createdat = DateTime.UtcNow
            };
            _transactionLogGateway.Insert(transactionLog);

            var poLog = new Purchaseorderlog
            {
                purchaseorderlogid = transactionLog.transactionlogid,
                poid = po.PoId,
                podate = po.PoDate,
                supplierid = po.SupplierId,
                expecteddeliverydate = po.ExpectedDeliveryDate,
                totalamount = po.TotalAmount,
                detailsjson = po.DetailsJson
            };

            // TODO: Set enum field once mapping is confirmed
            // poLog.status = Enum.Parse<RentalStatus>(po.Status);

            _purchaseOrderLogGateway.Insert(poLog);
        }
    }
}