namespace ProRental.Data.Module2.Gateways;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;

public class ClearanceLogGateway : IClearanceLogGateway
{
    private readonly AppDbContext context;

    public ClearanceLogGateway(AppDbContext context)
    {
        this.context = context;
    }

    public Clearancelog Insert(Clearancelog log)
    {
        context.Clearancelogs.Add(log);
        context.SaveChanges();
        return log;
    }

    public List<Clearancelog> GetAll()
    {
        return context.Clearancelogs
            .Include(c => c.ClearancelogNavigation)
            .OrderByDescending(c=> EF.Property<DateTime?>(c.ClearancelogNavigation, "Createdat"))
            .ToList();
    }

    public Clearancelog? GetById(int clearanceLogId)
    {
        return context.Clearancelogs
            .Include(c => c.ClearancelogNavigation)
            .FirstOrDefault(c => EF.Property<int>(c, "Clearancelogid") == clearanceLogId);
    }

    public bool ExistsByClearanceBatchId(int clearanceBatchId)
    {
        return context.Clearancelogs
        .Any(c => EF.Property<int>(c, "Clearancebatchid") == clearanceBatchId);
    }
}