namespace ProRental.Data.Module2.Gateways;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;

public class RentalOrderLogGateway : IRentalOrderLogGateway
{
    private readonly AppDbContext context;

    public RentalOrderLogGateway(AppDbContext context)
    {
        this.context = context;
    }

    public Rentalorderlog Insert(Rentalorderlog log)
    {
        context.Rentalorderlogs.Add(log);
        context.SaveChanges();
        return log;
    }

    public List<Rentalorderlog> GetAll()
    {
        return context.Rentalorderlogs
            .Include(r => r.RentalorderlogNavigation)
            .OrderByDescending(r => EF.Property<DateTime?>(r.RentalorderlogNavigation, "Createdat"))
            .ToList();
    }

    public Rentalorderlog? GetById(int rentalOrderLogId)
    {
        return context.Rentalorderlogs
            .Include(r => r.RentalorderlogNavigation)
            .FirstOrDefault(r => EF.Property<int>(r, "Rentalorderlogid") == rentalOrderLogId);
    }

    public bool ExistsByOrderId(int orderId)
    {
        return context.Rentalorderlogs.Any(r => EF.Property<int?>(r, "Orderid") == orderId);
    }
}