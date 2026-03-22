using ProRental.Domain.Entities;

namespace ProRental.Data.Interfaces
{
    public interface ISupplierSelectionStrategy
    {
        List<Supplier> SelectEligibleSuppliers(List<Supplier> suppliers, int stockId, int qty);
    }
}