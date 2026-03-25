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
            .Include(r => r.ReturnlogNavigation)
            .OrderByDescending(r => EF.Property<DateTime?>(r.ReturnlogNavigation, "Createdat"))
            .ToList();
    }

    public List<Returnlog> GetByRentalOrderLogId(int rentalOrderLogId)
{
    return context.Returnlogs
        .Include(r => r.ReturnlogNavigation)
        .Where(r => EF.Property<int>(r, "Rentalorderlogid") == rentalOrderLogId)
        .OrderByDescending(r => EF.Property<DateTime?>(r.ReturnlogNavigation, "Createdat"))
        .ToList();
}

    public Returnlog? GetById(int returnLogId)
    {
        return context.Returnlogs
            .Include(r => r.ReturnlogNavigation)
            .FirstOrDefault(r => EF.Property<int>(r, "Returnlogid") == returnLogId);
    }

    public bool ExistsByReturnRequestId(int returnRequestId)
    {
        return context.Returnlogs.
        Any(r => EF.Property<int>(r, "Returnrequestid") == returnRequestId);
    }
}