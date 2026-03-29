using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class ClearanceItemMapper : IClearanceItemMapper
{
    private readonly AppDbContext _context;

    public ClearanceItemMapper(AppDbContext context)
    {
        _context = context;
    }

    public Clearanceitem? FindById(int itemId)
    {
        // RULE: Use EF.Property to access the private 'Clearanceitemid'.
        return _context.Clearanceitems
            .FirstOrDefault(c => EF.Property<int>(c, "Clearanceitemid") == itemId);
    }

    public Clearanceitem? FindByInventoryItemId(int inventoryItemId)
    {
        // RULE: Use EF.Property to query against the private 'Inventoryitemid'.
        // Note: Returning a single item because the database enforces a 1-to-1 unique index.
        return _context.Clearanceitems
            .FirstOrDefault(c => EF.Property<int>(c, "Inventoryitemid") == inventoryItemId);
    }

    public ICollection<Clearanceitem>? FindByBatchId(int batchId)
    {
        // RULE: Use EF.Property to query against the private 'Clearancebatchid'.
        return _context.Clearanceitems
            .Where(c => EF.Property<int>(c, "Clearancebatchid") == batchId)
            .ToList();
    }

    public ICollection<Clearanceitem>? FindByStatus(ClearanceStatus status)
    {
        // RULE: Use EF.Property to query against the private 'Status' enum from the manual partial class.
        return _context.Clearanceitems
            .Where(c => EF.Property<ClearanceStatus>(c, "Status") == status)
            .ToList();
    }

    public ICollection<Clearanceitem>? FindAll()
    {
        return _context.Clearanceitems.ToList();
    }

    public void Insert(Clearanceitem item)
    {
        _context.Clearanceitems.Add(item);
        _context.SaveChanges();
    }

    public void Update(Clearanceitem item)
    {
        var existing = _context.Clearanceitems
            .FirstOrDefault(c => EF.Property<int>(c, "Clearanceitemid") == item.GetClearanceItemId());

        if (existing == null) return;

        _context.Entry(existing).CurrentValues.SetValues(item);
        _context.SaveChanges();
    }

    public void Delete(Clearanceitem item)
    {
        var existing = _context.Clearanceitems
            .FirstOrDefault(c => EF.Property<int>(c, "Clearanceitemid") == item.GetClearanceItemId());
            
        if (existing != null)
        {
            _context.Clearanceitems.Remove(existing);
            _context.SaveChanges();
        }
    }
}