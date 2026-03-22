using Microsoft.EntityFrameworkCore;
using ProRental.Data.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Gateways
{
    public class POLineItemGateway : IPOLineItemGateway
    {
        private readonly AppDbContext _context;

        public POLineItemGateway(AppDbContext context)
        {
            _context = context;
        }

        public void InsertItems(int poId, List<Polineitem> items)
        {
            _context.Polineitems.AddRange(items);
            _context.SaveChanges();
        }

        public void ReplaceItems(int poId, List<Polineitem> items)
        {
            DeleteItemsByPO(poId);
            InsertItems(poId, items);
        }

        public List<Polineitem> FindItemsByPO(int poId)
        {
            return _context.Polineitems
                .Where(x => EF.Property<int>(x, "Poid") == poId)
                .ToList();
        }

        public void DeleteItemsByPO(int poId)
        {
            var items = _context.Polineitems
                .Where(x => EF.Property<int>(x, "Poid") == poId)
                .ToList();

            _context.Polineitems.RemoveRange(items);
            _context.SaveChanges();
        }
    }
}