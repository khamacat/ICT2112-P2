using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Enums;
using ProRental.Domain.Module2.P2_2.Entities;

namespace ProRental.Data.Module2.Gateways;

public class CategoryChangeLogMapper : ICategoryChangeLogMapper
{
    private readonly AppDbContext _context;

    public CategoryChangeLogMapper(AppDbContext context)
    {
        _context = context;
    }

    public void insertCategoryChangeLog(SupplierCategoryChangeLog log)
    {
        var dbEntity = Activator.CreateInstance(typeof(ProRental.Domain.Entities.Suppliercategorychangelog))!;
        var entry = _context.Entry(dbEntity);
        entry.Property("Supplierid").CurrentValue       = log.SupplierID;
        entry.Property("Previouscategory").CurrentValue = log.PreviousCategory;
        entry.Property("Newcategory").CurrentValue      = log.NewCategory;
        entry.Property("Changereason").CurrentValue     = log.ChangedReason;
        entry.Property("Changedat").CurrentValue        = log.ChangedAt;

        _context.Add(dbEntity);
        _context.SaveChanges();

        log.LogID = (int)(_context.Entry(dbEntity).Property("Logid").CurrentValue ?? 0);
    }

    public void updateCategoryChangeLog(SupplierCategoryChangeLog log)
    {
        var dbEntity = _context.Suppliercategorychangelogs
            .SingleOrDefault(l => EF.Property<int>(l, "Logid") == log.LogID);

        if (dbEntity is null) { insertCategoryChangeLog(log); return; }

        var entry = _context.Entry(dbEntity);
        entry.Property("Supplierid").CurrentValue       = log.SupplierID;
        entry.Property("Previouscategory").CurrentValue = log.PreviousCategory;
        entry.Property("Newcategory").CurrentValue      = log.NewCategory;
        entry.Property("Changereason").CurrentValue     = log.ChangedReason;
        entry.Property("Changedat").CurrentValue        = log.ChangedAt;
        _context.SaveChanges();
    }

    public bool deleteCategoryChangeLog(int logID)
    {
        var dbEntity = _context.Suppliercategorychangelogs
            .SingleOrDefault(l => EF.Property<int>(l, "Logid") == logID);

        if (dbEntity is null) return false;
        _context.Suppliercategorychangelogs.Remove(dbEntity);
        _context.SaveChanges();
        return true;
    }

    public SupplierCategoryChangeLog findCategoryChangeLogById(int logID)
    {
        var dbEntity = _context.Suppliercategorychangelogs
            .SingleOrDefault(l => EF.Property<int>(l, "Logid") == logID);

        if (dbEntity is null) return null!;
        var result = MapFromEntry(_context.Entry(dbEntity));
        _context.Entry(dbEntity).State = EntityState.Detached;
        return result;
    }

    public List<SupplierCategoryChangeLog> findLogsBySupplier(int supplierID)
    {
        var dbEntities = _context.Suppliercategorychangelogs
            .Where(l => EF.Property<int?>(l, "Supplierid") == supplierID)
            .ToList();

        var results = dbEntities.Select(e => MapFromEntry(_context.Entry(e))).ToList();

        foreach (var e in dbEntities)
            _context.Entry(e).State = EntityState.Detached;

        return results;
    }

    private static SupplierCategoryChangeLog MapFromEntry(EntityEntry entry)
    {
        var prevRaw = entry.Property("Previouscategory").CurrentValue;
        var newRaw  = entry.Property("Newcategory").CurrentValue;

        return new SupplierCategoryChangeLog
        {
            LogID            = (int)(entry.Property("Logid").CurrentValue ?? 0),
            SupplierID       = (int)(entry.Property("Supplierid").CurrentValue ?? 0),
            PreviousCategory = prevRaw is null ? SupplierCategory.NEWUNTESTED : (SupplierCategory)prevRaw,
            NewCategory      = newRaw  is null ? SupplierCategory.NEWUNTESTED : (SupplierCategory)newRaw,
            ChangedReason    = (string)(entry.Property("Changereason").CurrentValue ?? string.Empty),
            ChangedAt        = (DateTime)(entry.Property("Changedat").CurrentValue ?? DateTime.UtcNow),
        };
    }
}