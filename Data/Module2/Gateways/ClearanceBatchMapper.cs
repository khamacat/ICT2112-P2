using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

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
        var existing = _context.Clearancebatches
            .FirstOrDefault(c => EF.Property<int>(c, "Clearancebatchid") == batch.GetClearanceBatchId());

        if (existing == null) return;

        _context.Entry(existing).CurrentValues.SetValues(batch);
        _context.SaveChanges(); // No timestamp override needed
    }

    public void Delete(Clearancebatch batch)
    {
        var existing = _context.Clearancebatches
            .FirstOrDefault(c => EF.Property<int>(c, "Clearancebatchid") == batch.GetClearanceBatchId());
            
        if (existing != null)
        {
            _context.Clearancebatches.Remove(existing);
            _context.SaveChanges();
        }
    }
}