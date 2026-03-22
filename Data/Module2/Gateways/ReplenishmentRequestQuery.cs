using Microsoft.EntityFrameworkCore;
using ProRental.Data.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Gateways
{
    public class ReplenishmentRequestQuery : IReplenishmentRequestQuery
    {
        private readonly AppDbContext _context;

        public ReplenishmentRequestQuery(AppDbContext context)
        {
            _context = context;
        }

        public Replenishmentrequest? GetRequest(int reqId)
        {
            return _context.Replenishmentrequests
                .FirstOrDefault(x => EF.Property<int>(x, "Requestid") == reqId);
        }

        public List<Lineitem> GetLineItems(int reqId)
        {
            return _context.Lineitems
                .Where(x => EF.Property<int?>(x, "Requestid") == reqId)
                .ToList();
        }
    }
}