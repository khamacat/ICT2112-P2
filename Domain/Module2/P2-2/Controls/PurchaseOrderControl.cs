using Microsoft.EntityFrameworkCore;
using ProRental.Data.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Interfaces;

namespace ProRental.Domain.Control
{
    public class PurchaseOrderControl : IPurchaseOrderService
    {
        private readonly IPurchaseOrderGateway _purchaseOrderGateway;
        private readonly IPOLineItemGateway _poLineItemGateway;
        private readonly IReplenishmentRequestQuery _replenishmentRequestQuery;
        private readonly ISupplierGateway _supplierGateway;
        private readonly ISupplierSelectionStrategy _supplierSelectionStrategy;

        public PurchaseOrderControl(
            IPurchaseOrderGateway purchaseOrderGateway,
            IPOLineItemGateway poLineItemGateway,
            IReplenishmentRequestQuery replenishmentRequestQuery,
            ISupplierGateway supplierGateway,
            ISupplierSelectionStrategy supplierSelectionStrategy)
        {
            _purchaseOrderGateway = purchaseOrderGateway;
            _poLineItemGateway = poLineItemGateway;
            _replenishmentRequestQuery = replenishmentRequestQuery;
            _supplierGateway = supplierGateway;
            _supplierSelectionStrategy = supplierSelectionStrategy;
        }

        public List<Supplier> GetEligibleSuppliers(int stockId, int qty)
        {
            var suppliers = _supplierGateway.FindVerifiedSuppliers();
            return _supplierSelectionStrategy.SelectEligibleSuppliers(suppliers, stockId, qty);
        }

        public Purchaseorder BuildPODraft(int reqId, int supplierId)
        {
            var request = _replenishmentRequestQuery.GetRequest(reqId);
            if (request == null)
                throw new InvalidOperationException("Replenishment request not found.");

            if (_purchaseOrderGateway.ExistsForRequest(reqId))
                throw new InvalidOperationException("Purchase order already exists for this request.");

            var po = new Purchaseorder();

            SetField(po, "_supplierid", supplierId);
            SetField(po, "_podate", DateOnly.FromDateTime(DateTime.Today));
            SetField(po, "_status", "SUBMITTED");
            SetField(po, "_totalamount", 0m);

            return po;
        }

        public int ConfirmPO(int reqId, Purchaseorder po)
        {
            if (_purchaseOrderGateway.ExistsForRequest(reqId))
                throw new InvalidOperationException("Purchase order already exists for this request.");

            _purchaseOrderGateway.Insert(po);

            int poId = EF.Property<int>(po, "Poid");
            var items = GeneratePOLineItems(poId, reqId);
            _poLineItemGateway.InsertItems(poId, items);

            decimal total = items.Sum(x => EF.Property<decimal?>(x, "Linetotal") ?? 0m);
            SetField(po, "_totalamount", total);

            _purchaseOrderGateway.Update(po);
            _purchaseOrderGateway.UpdateStatus(poId, "CONFIRMED");

            return poId;
        }

        public List<Polineitem> GeneratePOLineItems(int poId, int reqId)
        {
            var requestItems = _replenishmentRequestQuery.GetLineItems(reqId);

            return requestItems.Select(line =>
            {
                var item = new Polineitem();

                int qty = EF.Property<int?>(line, "Quantityrequest") ?? 0;
                int? productId = EF.Property<int?>(line, "Productid");
                decimal unitPrice = 10.00m;
                decimal lineTotal = qty * unitPrice;

                SetField(item, "_poid", poId);
                SetField(item, "_productid", productId);
                SetField(item, "_qty", qty);
                SetField(item, "_unitprice", unitPrice);
                SetField(item, "_linetotal", lineTotal);

                return item;
            }).ToList();
        }

        public void RecordExpectedDeliveryDate(int poId, DateOnly date)
        {
            _purchaseOrderGateway.UpdateExpectedDeliveryDate(poId, date);
        }

        public Purchaseorder? GetPOById(int poId)
        {
            return _purchaseOrderGateway.FindById(poId);
        }

        public Purchaseorder? GetPOByRequestId(int reqId)
        {
            return _purchaseOrderGateway.FindByRequestId(reqId);
        }

        private static void SetField(object target, string fieldName, object? value)
        {
            var field = target.GetType().GetField(
                fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (field != null)
            {
                field.SetValue(target, value);
            }
        }
    }
}