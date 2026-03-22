using ProRental.Domain.Entities;

namespace ProRental.Data.Interfaces
{
    public interface IPOLineItemGateway
    {
        void InsertItems(int poId, List<Polineitem> items);
        void ReplaceItems(int poId, List<Polineitem> items);
        List<Polineitem> FindItemsByPO(int poId);
        void DeleteItemsByPO(int poId);
    }
}