using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Returnitemid, Returnrequestid, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., r.Returnitemid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. NO CROSS-AGGREGATE INCLUDES: Do NOT use .Include() for Inventoryitem, Returnrequest, or Damagereports. 
 * If a developer needs those, they must use their respective mappers.
 * 3. NO AUTO-UPDATEDAT: This specific entity does not have an Updatedat field. Do not hallucinate one.
 * =========================================================================
 */

public class ReturnItemMapper : IReturnItemMapper
{
    private readonly AppDbContext _context;

    public ReturnItemMapper(AppDbContext context)
    {
        _context = context;
    }

    public Returnitem? FindById(int itemId)
    {
        // RULE: Use EF.Property to access the private 'Returnitemid'.
        // RULE: No .Include() for navigation properties.
        return _context.Returnitems
            .FirstOrDefault(r => EF.Property<int>(r, "Returnitemid") == itemId);
    }

    public ICollection<Returnitem>? FindAll()
    {
        return _context.Returnitems.ToList();
    }

    public ICollection<Returnitem>? FindByReturnRequest(int requestId)
    {
        // RULE: Use EF.Property to query against the private 'Returnrequestid'.
        return _context.Returnitems
            .Where(r => EF.Property<int>(r, "Returnrequestid") == requestId)
            .ToList();
    }

    public ICollection<Returnitem>? FindByStatus(ReturnItemStatus status)
    {
        // RULE: Use EF.Property to query against the private 'Status' enum from the manual partial class.
        return _context.Returnitems
            .Where(r => EF.Property<ReturnItemStatus>(r, "Status") == status)
            .ToList();
    }

    public void Insert(Returnitem item)
    {
        _context.Returnitems.Add(item);
        _context.SaveChanges();
    }

    public void Update(Returnitem item)
    {
        // Note: Completiondate should be set via a domain method (e.g., item.MarkAsCompleted())
        // prior to calling this Update method when the item finishes its inspection/repair pipeline.
        _context.Returnitems.Update(item);
        _context.SaveChanges();
    }

    public void Delete(Returnitem item)
    {
        _context.Returnitems.Remove(item);
        _context.SaveChanges();
    }
}