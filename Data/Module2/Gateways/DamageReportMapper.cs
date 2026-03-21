using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Damagereportid, Returnitemid, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., d.Damagereportid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. NO CROSS-AGGREGATE INCLUDES: Do NOT use .Include(d => d.Returnitem). 
 * If a developer needs the return item details, they must use the IReturnItemMapper.
 * 3. NO AUTO-UPDATEDAT: This specific entity relies on Reportdate which is set 
 * upon creation. It DOES NOT have an Updatedat field. Do not hallucinate one in Update().
 * =========================================================================
 */

public class DamageReportMapper : IDamageReportMapper
{
    private readonly AppDbContext _context;

    public DamageReportMapper(AppDbContext context)
    {
        _context = context;
    }

    public Damagereport? FindById(int damageReportId)
    {
        // RULE: Use EF.Property to access the private 'Damagereportid'.
        return _context.Damagereports
            .FirstOrDefault(d => EF.Property<int>(d, "Damagereportid") == damageReportId);
    }

    public ICollection<Damagereport>? FindAll()
    {
        return _context.Damagereports.ToList();
    }

    public Damagereport? FindByReturnItemId(int returnItemId)
    {
        // RULE: Use EF.Property to query against the private 'Returnitemid'.
        // Note: Returning a single item as per the IDamageReportMapper interface.
        return _context.Damagereports
            .FirstOrDefault(d => EF.Property<int>(d, "Returnitemid") == returnItemId);
    }

    public void Insert(Damagereport damageReport)
    {
        _context.Damagereports.Add(damageReport);
        _context.SaveChanges();
    }

    public void Update(Damagereport damageReport)
    {
        // No auto-timestamping here as the table only tracks the initial Reportdate
        _context.Damagereports.Update(damageReport);
        _context.SaveChanges();
    }

    public void Delete(Damagereport damageReport)
    {
        _context.Damagereports.Remove(damageReport);
        _context.SaveChanges();
    }
}