using ProRental.Domain.Entities;

namespace ProRental.Data.Interfaces
{
    public interface ISupplierGateway
    {
        Supplier? FindById(int supplierId);
        List<Supplier> FindVerifiedSuppliers();
        List<Supplier> FindAll();
    }
}