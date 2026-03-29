using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

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
        var existing = _context.Damagereports
            .FirstOrDefault(d => EF.Property<int>(d, "Damagereportid") == damageReport.GetDamageReportId());

        if (existing == null) return;

        _context.Entry(existing).CurrentValues.SetValues(damageReport);
        _context.SaveChanges();
    }

    public void Delete(Damagereport damageReport)
    {
        var existing = _context.Damagereports
            .FirstOrDefault(d => EF.Property<int>(d, "Damagereportid") == damageReport.GetDamageReportId());
            
        if (existing != null)
        {
            _context.Damagereports.Remove(existing);
            _context.SaveChanges();
        }
    }
}