namespace ProRental.Data.Module2.Gateways;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;

public class PurchaseOrderLogGateway : IPurchaseOrderLogGateway
{
    private readonly AppDbContext context;

    public PurchaseOrderLogGateway(AppDbContext context)
    {
        this.context = context;
    }

    public Purchaseorderlog Insert(Purchaseorderlog log)
    {
        context.Purchaseorderlogs.Add(log);
        context.SaveChanges();
        return log;
    }

    public List<Purchaseorderlog> GetAll()
    {
        return context.Purchaseorderlogs
            .Include(p => p.PurchaseorderlogNavigation) // joins TransactionLog for CreatedAt
            .OrderByDescending(p => p.PurchaseorderlogNavigation.createdat)
            .ToList();
    }

    public Purchaseorderlog? GetById(int purchaseOrderLogId)
    {
        return context.Purchaseorderlogs
            .Include(p => p.PurchaseorderlogNavigation)
            .FirstOrDefault(p => p.purchaseorderlogid == purchaseOrderLogId);
    }

    public bool ExistsByPoId(int poId)
    {
        return context.Purchaseorderlogs
            .Any(p => p.poid == poId);
    }
}