using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Alertid, Productid, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., a.Alertid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. NO CROSS-AGGREGATE INCLUDES: Do NOT use .Include(a => a.Product). 
 * If a developer needs the Product data, they must use the IProductMapper.
 * 3. UTC TIMESTAMPS: Always override the "Updatedat" to DateTime.UtcNow 
 * using _context.Entry() during updates to maintain the TIMESTAMPTZ standard.
 * =========================================================================
 */

public class AlertMapper : IAlertMapper
{
    private readonly AppDbContext _context;

    public AlertMapper(AppDbContext context)
    {
        _context = context;
    }

    public Alert? FindById(int alertId)
    {
        // Using EF.Property to access the private 'Alertid'
        // No .Include() here. We respect the aggregate boundary.
        return _context.Alerts
            .FirstOrDefault(a => EF.Property<int>(a, "Alertid") == alertId);
    }

    public ICollection<Alert>? FindAll()
    {
        // Clean, decoupled read
        return _context.Alerts.ToList();
    }

    public ICollection<Alert>? FindByProductId(int productId)
    {
        // Using EF.Property to query against the private 'Productid'
        return _context.Alerts
            .Where(a => EF.Property<int>(a, "Productid") == productId)
            .ToList();
    }

    public void Insert(Alert alert)
    {
        _context.Alerts.Add(alert);
        _context.SaveChanges();
    }

    public void Update(Alert alert)
    {
        // Automatically enforce the TIMESTAMPTZ standard for updates
        // We use Entry() to update the private Updatedat property safely
        _context.Entry(alert).Property("Updatedat").CurrentValue = DateTime.UtcNow;

        _context.Alerts.Update(alert);
        _context.SaveChanges();
    }

    public void Delete(Alert alert)
    {
        _context.Alerts.Remove(alert);
        _context.SaveChanges();
    }
}