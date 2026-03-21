using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Clearancebatchid, Batchname, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., c.Clearancebatchid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. NO CROSS-AGGREGATE INCLUDES: Do NOT use .Include() for Clearanceitems or Clearancelogs. 
 * If a developer needs the items inside a batch, they must use the IClearanceItemMapper.
 * 3. NO AUTO-UPDATEDAT: This specific entity relies on Createddate and Clearancedate. 
 * It DOES NOT have an Updateddate field. Do not hallucinate a DateTime.UtcNow override in Update().
 * =========================================================================
 */

public class ClearanceBatchMapper : IClearanceBatchMapper
{
    private readonly AppDbContext _context;

    public ClearanceBatchMapper(AppDbContext context)
    {
        _context = context;
    }

    public Clearancebatch? FindById(int batchId)
    {
        // RULE: Use EF.Property to access the private 'Clearancebatchid'.
        return _context.Clearancebatches
            .FirstOrDefault(c => EF.Property<int>(c, "Clearancebatchid") == batchId);
    }

    public ICollection<Clearancebatch>? FindAll()
    {
        return _context.Clearancebatches.ToList();
    }

    public ICollection<Clearancebatch>? FindByStatus(ClearanceBatchStatus status)
    {
        // RULE: Use EF.Property to query against the private 'Status' enum from the manual partial class.
        return _context.Clearancebatches
            .Where(c => EF.Property<ClearanceBatchStatus>(c, "Status") == status)
            .ToList();
    }

    public void Insert(Clearancebatch batch)
    {
        _context.Clearancebatches.Add(batch);
        _context.SaveChanges();
    }

    public void Update(Clearancebatch batch)
    {
        // Note: Clearancedate and Status are updated via domain logic methods 
        // before calling this mapper Update. No automatic timestamping is applied here.
        _context.Clearancebatches.Update(batch);
        _context.SaveChanges();
    }

    public void Delete(Clearancebatch batch)
    {
        _context.Clearancebatches.Remove(batch);
        _context.SaveChanges();
    }
}