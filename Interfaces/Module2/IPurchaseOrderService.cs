using ProRental.Domain.Entities;

namespace ProRental.Interfaces
{
    public interface IPurchaseOrderService
    {
        List<Supplier> GetEligibleSuppliers(int stockId, int qty);
        Purchaseorder BuildPODraft(int reqId, int supplierId);
        int ConfirmPO(int reqId, Purchaseorder po);
        List<Polineitem> GeneratePOLineItems(int poId, int reqId);
        void RecordExpectedDeliveryDate(int poId, DateOnly date);
        Purchaseorder? GetPOById(int poId);
        Purchaseorder? GetPOByRequestId(int reqId);
    }
}