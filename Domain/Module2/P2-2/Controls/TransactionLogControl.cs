namespace ProRental.Domain.Module2.P2_2.Controls;

using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces;
using ProRental.Data.Module2.Interfaces;

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

        public void pushRentalOrderData(int orderId, int customerId, DateTime orderDate,
                                    decimal totalAmount, string deliveryType, string status,
                                    string? detailsJson)
    {
        if (_rentalOrderLogGateway.ExistsByOrderId(orderId))
            return;

        var transactionLog = new Transactionlog
        {
            log_type = LogType.RENTAL_ORDER,
            created_at = DateTime.UtcNow
        };

        try
        {
            _transactionLogGateway.Insert(transactionLog);

            var rentalLog = new Rentalorderlog
            {
                rental_orderlogid = transactionLog.transaction_logid,
                order_id = orderId,
                customer_id = customerId,
                order_date = orderDate,
                total_amount = totalAmount,
                details_json = detailsJson
            };

            _rentalOrderLogGateway.Insert(rentalLog);
        }
        catch
        {
            _transactionLogGateway.Delete(transactionLog.transaction_logid);
        }
    }

        public void pushLoanData(int loanListId, int rentalOrderLogId, string status,
                            DateTime? loanDate, DateTime? returnDate, DateTime? dueDate,
                            string? detailsJson)
    {
        if (_loanLogGateway.ExistsByLoanListId(loanListId))
            return;

        var transactionLog = new Transactionlog
        {
            log_type = LogType.LOAN,
            created_at = DateTime.UtcNow
        };

        try
        {
            _transactionLogGateway.Insert(transactionLog);

            var loanLog = new Loanlog
            {
                loan_logid = transactionLog.transaction_logid,
                loan_listid = loanListId,
                rental_orderlogid = rentalOrderLogId,
                loan_date = loanDate,
                return_date = returnDate,
                due_date = dueDate,
                details_json = detailsJson
            };

            _loanLogGateway.Insert(loanLog);
        }
        catch
        {
            _transactionLogGateway.Delete(transactionLog.transaction_logid);
        }
    }

        public void pushReturnData(int returnRequestId, int rentalOrderLogId, string? customerId,
                                string status, DateTime? requestDate, DateTime? completionDate,
                                string? imageUrl, string? detailsJson)
    {
        if (_returnLogGateway.ExistsByReturnRequestId(returnRequestId))
            return;

        var transactionLog = new Transactionlog
        {
            log_type = LogType.RETURN,
            created_at = DateTime.UtcNow
        };

        try
        {
            _transactionLogGateway.Insert(transactionLog);

            var returnLog = new Returnlog
            {
                return_logid = transactionLog.transaction_logid,
                return_requestid = returnRequestId,
                rental_orderlogid = rentalOrderLogId,
                customer_id = customerId,
                request_date = requestDate,
                completion_date = completionDate,
                image_url = imageUrl,
                details_json = detailsJson
            };

            _returnLogGateway.Insert(returnLog);
        }
        catch
        {
            _transactionLogGateway.Delete(transactionLog.transaction_logid);
        }
    }

        public void pushClearanceLogData(int clearanceBatchId, string? batchName,
                                    DateTime? clearanceDate, string status,
                                    string? detailsJson)
    {
        if (_clearanceLogGateway.ExistsByClearanceBatchId(clearanceBatchId))
            return;

        var transactionLog = new Transactionlog
        {
            log_type = LogType.CLEARANCE,
            created_at = DateTime.UtcNow
        };

        try
        {
            _transactionLogGateway.Insert(transactionLog);

            var clearanceLog = new Clearancelog
            {
                clearance_logid = transactionLog.transaction_logid,
                clearance_batchid = clearanceBatchId,
                batch_name = batchName,
                clearance_date = clearanceDate,
                details_json = detailsJson
            };

            _clearanceLogGateway.Insert(clearanceLog);
        }
        catch
        {
            _transactionLogGateway.Delete(transactionLog.transaction_logid);
        }
    }

    public void PullAndLogPurchaseOrders()
    {
        var allPOs = _purchaseOrderService.GetAllPurchaseOrders();

        foreach (var po in allPOs)
        {
            if (_purchaseOrderLogGateway.ExistsByPoId(po.PoId))
                continue;

            var transactionLog = new Transactionlog
            {
                log_type = LogType.PURCHASE_ORDER,
                created_at = DateTime.UtcNow
            };

            try
            {
                _transactionLogGateway.Insert(transactionLog);

                var poLog = new Purchaseorderlog
                {
                    purchaseorder_logid = transactionLog.transaction_logid,
                    po_id = po.PoId,
                    po_date = po.PoDate,
                    supplier_id = po.SupplierId,
                    expected_deliverydate = po.ExpectedDeliveryDate,
                    total_amount = po.TotalAmount,
                    details_json = po.DetailsJson
                };

                _purchaseOrderLogGateway.Insert(poLog);
            }
            catch
            {
                // If child insert fails, clean up the orphan parent
                _transactionLogGateway.Delete(transactionLog.transaction_logid);
            }
        }
    }
}