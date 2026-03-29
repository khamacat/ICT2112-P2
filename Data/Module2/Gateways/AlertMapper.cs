using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

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
        var existing = _context.Alerts
            .FirstOrDefault(a => EF.Property<int>(a, "Alertid") == alert.GetAlertId());

        if (existing == null) return;

        // Automatically map all scalar properties from incoming to existing
        _context.Entry(existing).CurrentValues.SetValues(alert);

        _context.SaveChanges();
    }

    public void Delete(Alert alert)
    {
        var existing = _context.Alerts
            .FirstOrDefault(a => EF.Property<int>(a, "Alertid") == alert.GetAlertId());
            
        if (existing != null)
        {
            _context.Alerts.Remove(existing);
            _context.SaveChanges();
        }
    }
}