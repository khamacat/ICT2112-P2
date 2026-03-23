namespace ProRental.Data.Module2.Gateways;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;

public class ReturnLogGateway : IReturnLogGateway
{
    private readonly AppDbContext context;

    public ReturnLogGateway(AppDbContext context)
    {
        this.context = context;
    }

    public Returnlog Insert(Returnlog log)
    {
        context.Returnlogs.Add(log);
        context.SaveChanges();
        return log;
    }

    public List<Returnlog> GetAll()
    {
        return context.Returnlogs
            .Include(r => r.ReturnlogNavigation) // joins TransactionLog for CreatedAt
            .OrderByDescending(r => r.ReturnlogNavigation.createdat)
            .ToList();
    }

    public Returnlog? GetById(int returnLogId)
    {
        return context.Returnlogs
            .Include(r => r.ReturnlogNavigation)
            .FirstOrDefault(r => r.returnlogid == returnLogId);
    }

    public bool ExistsByReturnRequestId(int returnRequestId)
    {
        return context.Returnlogs
            .Any(r => r.returnrequestid == returnRequestId);
    }
}