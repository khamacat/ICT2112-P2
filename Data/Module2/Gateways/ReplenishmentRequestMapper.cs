using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Module2;

namespace ProRental.Data.Module2.Gateways
{
    // Main EF Core Data Mapper for ReplenishmentRequest
    // Also serves as the cross-module query provider for PurchaseOrder module
    public class ReplenishmentRequestMapper : IReplenishmentRequestMapper, IReplenishmentRequestQuery
    {
        private readonly AppDbContext _context;

        public ReplenishmentRequestMapper(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Insert a new replenishment request into the database
        public void Insert(Replenishmentrequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _context.Replenishmentrequests.Add(request);
            _context.SaveChanges();
        }

        // Update an existing replenishment request in the database
        public void Update(Replenishmentrequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var entry = _context.Entry(request);

            // If entity is not tracked, attach/update it
            if (entry.State == EntityState.Detached)
            {
                _context.Replenishmentrequests.Update(request);
            }

            // If already tracked, EF Core will detect changes automatically
            _context.SaveChanges();
        }

        // Find a replenishment request by ID including its line items and products
        public Replenishmentrequest? FindById(int id)
        {
            return _context.Replenishmentrequests
                .Include(r => r.Lineitems)
                    .ThenInclude(li => li.Product)
                    .ThenInclude(p => p.Productdetail)
                .FirstOrDefault(r => EF.Property<int>(r, "Requestid") == id);
        }

        // Find all replenishment requests including line items
        public List<Replenishmentrequest> FindAll()
        {
            return _context.Replenishmentrequests
                .Include(r => r.Lineitems)
                    .ThenInclude(li => li.Product)
                    .ThenInclude(p => p.Productdetail)
                .OrderByDescending(r => EF.Property<DateTime?>(r, "Createdat"))
                .ToList();
        }

        // Find all line items for a specific replenishment request
        public List<Lineitem> FindLineItems(int requestId)
        {
            return _context.Lineitems
                .Include(li => li.Product)
                .ThenInclude(p => p.Productdetail)
                .Where(li => EF.Property<int?>(li, "Requestid") == requestId)
                .OrderBy(li => EF.Property<int>(li, "Lineitemid"))
                .ToList();
        }

        // Cross-module query method for Purchase Order module
        public Replenishmentrequest? GetRequest(int reqId)
        {
            return _context.Replenishmentrequests
                .FirstOrDefault(r => EF.Property<int>(r, "Requestid") == reqId);
        }

        // Cross-module query method for Purchase Order module
        public List<Lineitem> GetLineItems(int reqId)
        {
            return _context.Lineitems
                .Where(li => EF.Property<int?>(li, "Requestid") == reqId)
                .OrderBy(li => EF.Property<int>(li, "Lineitemid"))
                .ToList();
        }
    }
}