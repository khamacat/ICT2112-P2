using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Returnrequestid, Orderid, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., r.Returnrequestid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. NO CROSS-AGGREGATE INCLUDES: Do NOT use .Include() for Customer, Order, Returnitems, etc. 
 * If a developer needs those related records, they must use their respective mappers.
 * 3. NO AUTO-UPDATEDAT: This specific entity uses Requestdate and Completiondate. 
 * It DOES NOT have an Updatedat field. Do not hallucinate a DateTime.UtcNow override in Update().
 * =========================================================================
 */

public class ReturnRequestMapper : IReturnRequestMapper
{
    private readonly AppDbContext _context;

    public ReturnRequestMapper(AppDbContext context)
    {
        _context = context;
    }

    public Returnrequest? FindById(int requestId)
    {
        // RULE: Use EF.Property to access the private 'Returnrequestid'.
        return _context.Returnrequests
            .FirstOrDefault(r => EF.Property<int>(r, "Returnrequestid") == requestId);
    }

    public Returnrequest? FindByOrderId(int orderId)
    {
        // RULE: Use EF.Property to access the private 'Orderid'.
        return _context.Returnrequests
            .FirstOrDefault(r => EF.Property<int>(r, "Orderid") == orderId);
    }

    public ICollection<Returnrequest>? FindAll()
    {
        return _context.Returnrequests.ToList();
    }

    public ICollection<Returnrequest>? FindByCustomerId(int customerId)
    {
        // RULE: Use EF.Property to query against the private 'Customerid'.
        return _context.Returnrequests
            .Where(r => EF.Property<int>(r, "Customerid") == customerId)
            .ToList();
    }

    public ICollection<Returnrequest>? FindByStatus(ReturnRequestStatus status)
    {
        // RULE: Use EF.Property to query against the private 'Status' enum from the manual partial class.
        return _context.Returnrequests
            .Where(r => EF.Property<ReturnRequestStatus>(r, "Status") == status)
            .ToList();
    }

    public void Insert(Returnrequest request)
    {
        _context.Returnrequests.Add(request);
        _context.SaveChanges();
    }

    public void Update(Returnrequest request)
    {
        // Note: Completiondate and Status are updated via domain logic methods 
        // (e.g., request.CompleteRequest()) before calling this mapper Update.
        _context.Returnrequests.Update(request);
        _context.SaveChanges();
    }

    public void Delete(Returnrequest request)
    {
        _context.Returnrequests.Remove(request);
        _context.SaveChanges();
    }
}