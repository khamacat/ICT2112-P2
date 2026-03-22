using Microsoft.EntityFrameworkCore;
using ProRental.Data.Interfaces;
using ProRental.Domain.Entities;

namespace ProRental.Domain.Control
{
    public class FastestSupplierStrategy : ISupplierSelectionStrategy
    {
        public List<Supplier> SelectEligibleSuppliers(List<Supplier> suppliers, int stockId, int qty)
        {
            return suppliers
                .OrderBy(s => EF.Property<double?>(s, "Avgturnaroundtime") ?? double.MaxValue)
                .ToList();
        }
    }
}