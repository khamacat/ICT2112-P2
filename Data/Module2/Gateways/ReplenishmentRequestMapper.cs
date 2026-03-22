using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Module2;

namespace ProRental.Data.Module2.Gateways;


// Data Mapper implementation for ReplenishmentRequest
// Handles database operations using Entity Framework Core
// Also implements IReplenishmentRequestQuery for cross-module access
public class ReplenishmentRequestMapper : IReplenishmentRequestMapper, IReplenishmentRequestQuery
{
    private readonly AppDbContext _context;

    public ReplenishmentRequestMapper(AppDbContext context)
    {
        _context = context;
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

        // Check if entity is already tracked (from FindById)
        var entry = _context.Entry(request);
        if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Detached)
        {
            // Only attach/update if not already tracked
            _context.Replenishmentrequests.Update(request);
        }
        // If already tracked, EF Core will detect changes automatically

        _context.SaveChanges();
    }

    // Find a replenishment request by ID including its line items
    public Replenishmentrequest? FindById(int id)
    {
        return _context.Replenishmentrequests
            .Include(r => r.Lineitems)
                .ThenInclude(li => li.Product)
            .FirstOrDefault(r => EF.Property<int>(r, "Requestid") == id);
    }

    // Find all replenishment requests including their line items
    public List<Replenishmentrequest> FindAll()
    {
        return _context.Replenishmentrequests
            .Include(r => r.Lineitems)
            .OrderByDescending(r => EF.Property<DateTime?>(r, "Createdat"))
            .ToList();
    }

    // Find all line items for a specific replenishment request
    public List<Lineitem> FindLineItems(int requestId)
    {
        return _context.Lineitems
            .Include(li => li.Product)
            .Where(li => EF.Property<int?>(li, "Requestid") == requestId)
            .ToList();
    }

    // IReplenishmentRequestQuery implementation (for cross-module access)

    // Get a replenishment request by ID (query interface for PurchaseOrder module)
    public Replenishmentrequest? GetRequest(int reqId)
    {
        return FindById(reqId);
    }

    // Get all line items for a request (query interface for PurchaseOrder module)
    public List<Lineitem> GetLineItems(int reqId)
    {
        return FindLineItems(reqId);
    }
}
