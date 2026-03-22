using ProRental.Domain.Entities;

namespace ProRental.Data.Interfaces
{
    public interface IPurchaseOrderGateway
    {
        void Insert(Purchaseorder po);
        void Update(Purchaseorder po);
        void UpdateStatus(int poId, string status);
        void UpdateExpectedDeliveryDate(int poId, DateOnly date);
        Purchaseorder? FindById(int poId);
        Purchaseorder? FindByRequestId(int reqId);
        bool ExistsForRequest(int reqId);
    }
}