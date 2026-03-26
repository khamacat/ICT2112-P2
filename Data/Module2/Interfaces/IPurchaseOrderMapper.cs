using ProRental.Domain.Entities;
using ProRental.Controllers;

namespace ProRental.Data.Interfaces
{
    public interface IPurchaseOrderMapper
    {
        int Insert(Purchaseorder po);
        Purchaseorder? FindById(int poId);
        Purchaseorder? FindByRequestId(int reqId);
        void UpdateExpectedDeliveryDate(int poId, DateOnly expectedDeliveryDate);
        List<PurchaseOrderRequestListItemViewModel> GetAllRequests();
        List<PurchaseOrderListItemViewModel> GetAllPurchaseOrders();
        void UpdatePurchaseOrderTotalAmount(int poId, decimal totalAmount);
        void UpdateReplenishmentRequestStatusToSubmitted(int reqId);
        int InsertTransactionLogForPurchaseOrder();
        void InsertPurchaseOrderLog(
            int logId,
            int poId,
            int supplierId,
            DateOnly? expectedDeliveryDate,
            decimal totalAmount,
            string detailsJson);
        void ApprovePurchaseOrder(int poId);
        void CompletePurchaseOrder(int poId);
        void CancelPurchaseOrder(int poId);
        int? FindLinkedRequestIdByPoId(int poId);
        void CompleteReplenishmentRequest(int reqId, string completedBy);
        void CancelDraftReplenishmentRequest(int reqId);
    }
}
