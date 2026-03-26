using ProRental.Controllers;
using ProRental.Interfaces;
using ProRental.Interfaces.Module2;
using ProRental.Data.Interfaces;
using System.Text.Json;

namespace ProRental.Domain.Control
{
    public class PurchaseOrderControl : IPurchaseOrderService
    {
        private readonly IPurchaseOrderMapper _purchaseOrderMapper;
        private readonly IPOLineItemMapper _poLineItemMapper;
        private readonly Interfaces.Module2.IReplenishmentRequestQuery _replenishmentRequestQuery;
        private readonly ProRental.Interfaces.Module2.ISupplier _supplierService;

        public PurchaseOrderControl(
            IPurchaseOrderMapper purchaseOrderMapper,
            IPOLineItemMapper poLineItemMapper,
            Interfaces.Module2.IReplenishmentRequestQuery replenishmentRequestQuery,
            ProRental.Interfaces.Module2.ISupplier supplierService)
        {
            _purchaseOrderMapper = purchaseOrderMapper;
            _poLineItemMapper = poLineItemMapper;
            _replenishmentRequestQuery = replenishmentRequestQuery;
            _supplierService = supplierService;
        }

        public PurchaseOrderPageViewModel GetPurchaseOrderPageData(int reqId)
        {
            var vm = new PurchaseOrderPageViewModel
            {
                RequestId = reqId
            };

            var request = _replenishmentRequestQuery.GetRequest(reqId);
            if (request != null)
            {
                vm.RequestId = request.GetRequestId();
                vm.RequestedBy = request.GetRequestedBy() ?? "";
                vm.CreatedAt = request.GetCreatedAt();
                vm.Remarks = request.GetRemarks() ?? "";
                vm.Status = request.Status.ToString();
            }

            vm.Items = _poLineItemMapper.GetRequestItemsWithProductName(reqId);
            vm.Suppliers = _supplierService.getVerifiedSuppliers()
                .Select(s => new PurchaseOrderSupplierViewModel
                {
                    SupplierId = s.SupplierID,
                    SupplierName = s.Name,
                    Details = s.Details,
                    CreditPeriod = s.CreditPeriod,
                    AvgTurnaroundTime = s.AvgTurnaroundTime,
                    IsVerified = s.IsVerified
                }).ToList();

            return vm;
        }

        public List<PurchaseOrderRequestListItemViewModel> GetAllRequests()
        {
            return _purchaseOrderMapper.GetAllRequests();
        }

        public List<PurchaseOrderListItemViewModel> GetAllPurchaseOrders()
        {
            return _purchaseOrderMapper.GetAllPurchaseOrders();
        }

        public int ConfirmPurchaseOrder(int reqId, int supplierId, DateOnly? expectedDeliveryDate)
        {
            var po = new ProRental.Domain.Entities.Purchaseorder();

            // Mapper insert only depends on these private fields.
            SetPrivateField(po, "_supplierid", supplierId);
            SetPrivateField(po, "_expecteddeliverydate", expectedDeliveryDate);
            SetPrivateField(po, "_totalamount", 0m);

            var poId = _purchaseOrderMapper.Insert(po);

            _poLineItemMapper.InsertItemsFromReplenishmentRequest(poId, reqId);
            var totalAmount = _poLineItemMapper.GetTotalAmountByPO(poId);
            _purchaseOrderMapper.UpdatePurchaseOrderTotalAmount(poId, totalAmount);
            _purchaseOrderMapper.UpdateReplenishmentRequestStatusToSubmitted(reqId);

            var supplierName = _supplierService.getVerifiedSuppliers()
                .FirstOrDefault(s => s.SupplierID == supplierId)?.Name ?? $"Supplier #{supplierId}";

            var lineItems = _poLineItemMapper.GetPOLineItemsWithDetails(poId);

            var logId = _purchaseOrderMapper.InsertTransactionLogForPurchaseOrder();
            string detailsJson = JsonSerializer.Serialize(new
            {
                reqId,
                status = "CONFIRMED",
                supplierName,
                lineItems = lineItems.Select(li => new
                {
                    productId = li.ProductId,
                    productName = li.ProductName,
                    qty = li.Qty,
                    unitPrice = li.UnitPrice,
                    lineTotal = li.LineTotal
                })
            });

            _purchaseOrderMapper.InsertPurchaseOrderLog(
                logId,
                poId,
                supplierId,
                expectedDeliveryDate,
                totalAmount,
                detailsJson);

            return poId;
        }

        public void ApprovePurchaseOrder(int poId)
        {
            _purchaseOrderMapper.ApprovePurchaseOrder(poId);
        }

        public void CompletePurchaseOrder(int poId)
        {
            _purchaseOrderMapper.CompletePurchaseOrder(poId);

            var reqId = _purchaseOrderMapper.FindLinkedRequestIdByPoId(poId);
            if (!reqId.HasValue)
            {
                throw new InvalidOperationException($"No linked replenishment request found for PO #{poId}.");
            }

            _purchaseOrderMapper.CompleteReplenishmentRequest(reqId.Value, "system");
        }

        public void CancelPurchaseOrder(int poId)
        {
            _purchaseOrderMapper.CancelPurchaseOrder(poId);
        }

        public void CancelReplenishmentRequest(int reqId)
        {
            _purchaseOrderMapper.CancelDraftReplenishmentRequest(reqId);
        }

        private static void SetPrivateField(object target, string fieldName, object? value)
        {
            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field == null)
            {
                throw new InvalidOperationException($"Field '{fieldName}' was not found on type '{target.GetType().Name}'.");
            }

            field.SetValue(target, value);
        }
    }
}
