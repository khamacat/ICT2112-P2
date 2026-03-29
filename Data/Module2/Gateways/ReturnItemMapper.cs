using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

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
        var existing = _context.Returnitems
            .FirstOrDefault(r => EF.Property<int>(r, "Returnitemid") == item.GetReturnItemId());

        if (existing == null) return;

        _context.Entry(existing).CurrentValues.SetValues(item);
        _context.SaveChanges();
    }

    public void Delete(Returnitem item)
    {
        var existing = _context.Returnitems
            .FirstOrDefault(r => EF.Property<int>(r, "Returnitemid") == item.GetReturnItemId());
            
        if (existing != null)
        {
            _context.Returnitems.Remove(existing);
            _context.SaveChanges();
        }
    }
}