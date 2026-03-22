using Microsoft.EntityFrameworkCore;
using ProRental.Data.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Gateways
{
    public class PurchaseOrderGateway : IPurchaseOrderGateway
    {
        private readonly AppDbContext _context;

        public PurchaseOrderGateway(AppDbContext context)
        {
            _context = context;
        }

        public void Insert(Purchaseorder po)
        {
            _context.Purchaseorders.Add(po);
            _context.SaveChanges();
        }

        public void Update(Purchaseorder po)
        {
            _context.Purchaseorders.Update(po);
            _context.SaveChanges();
        }

        public void UpdateStatus(int poId, string status)
        {
            var po = FindById(poId);
            if (po == null) return;

            _context.Entry(po).Property("Status").CurrentValue = status;
            _context.SaveChanges();
        }

        public void UpdateExpectedDeliveryDate(int poId, DateOnly date)
        {
            var po = FindById(poId);
            if (po == null) return;

            _context.Entry(po).Property("Expecteddeliverydate").CurrentValue = date;
            _context.SaveChanges();
        }

        public Purchaseorder? FindById(int poId)
        {
            return _context.Purchaseorders
                .Include(x => x.Polineitems)
                .FirstOrDefault(x => EF.Property<int>(x, "Poid") == poId);
        }

        public Purchaseorder? FindByRequestId(int reqId)
        {
            return _context.Purchaseorders
                .FirstOrDefault(x => EF.Property<int?>(x, "Requestid") == reqId);
        }

        public bool ExistsForRequest(int reqId)
        {
            return _context.Purchaseorders
                .Any(x => EF.Property<int?>(x, "Requestid") == reqId);
        }
    }
}