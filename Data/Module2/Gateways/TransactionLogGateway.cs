namespace ProRental.Data.Module2.Gateways;

using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;

public class TransactionLogGateway : ITransactionLogGateway
{
    private readonly AppDbContext context;

    public TransactionLogGateway(AppDbContext context)
    {
        this.context = context;
    }

    public Transactionlog Insert(Transactionlog log)
    {
        context.Transactionlogs.Add(log);
        context.SaveChanges();
        return log;
    }

    public List<Transactionlog> GetAll()
    {
        return context.Transactionlogs
            .OrderByDescending(t => t.createdat)
            .ToList();
    }

    public Transactionlog? GetById(int transactionLogId)
    {
        return context.Transactionlogs.Find(transactionLogId);
    }

    public bool Delete(int transactionLogId)
    {
        var log = GetById(transactionLogId);
        if (log == null) return false;

        context.Transactionlogs.Remove(log);
        context.SaveChanges();
        return true;
    }
}