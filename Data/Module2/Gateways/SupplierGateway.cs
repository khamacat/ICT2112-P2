using Microsoft.EntityFrameworkCore;
using ProRental.Data.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Gateways
{
    public class SupplierGateway : ISupplierGateway
    {
        private readonly AppDbContext _context;

        public SupplierGateway(AppDbContext context)
        {
            _context = context;
        }

        public Supplier? FindById(int supplierId)
        {
            return _context.Suppliers
                .FirstOrDefault(x => EF.Property<int>(x, "Supplierid") == supplierId);
        }

        public List<Supplier> FindVerifiedSuppliers()
        {
            return _context.Suppliers
                .Where(x => EF.Property<bool?>(x, "Isverified") == true)
                .ToList();
        }

        public List<Supplier> FindAll()
        {
            return _context.Suppliers.ToList();
        }
    }
}