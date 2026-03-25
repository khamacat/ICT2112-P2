namespace ProRental.Data.Module2.Gateways;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;

public class LoanLogGateway : ILoanLogGateway
{
    private readonly AppDbContext context;

    public LoanLogGateway(AppDbContext context)
    {
        this.context = context;
    }

    public Loanlog Insert(Loanlog log)
    {
        context.Loanlogs.Add(log);
        context.SaveChanges();
        return log;
    }

    public List<Loanlog> GetAll()
    {
        return context.Loanlogs
            .Include(l => l.LoanlogNavigation)
            .OrderByDescending(l => EF.Property<DateTime?>(l.LoanlogNavigation, "Createdat"))
            .ToList();
    }

    public List<Loanlog> GetByRentalOrderLogId(int rentalOrderLogId)
{
    return context.Loanlogs
        .Include(l => l.LoanlogNavigation)
        .Where(l => EF.Property<int>(l, "Rentalorderlogid") == rentalOrderLogId)
        .OrderByDescending(l => EF.Property<DateTime?>(l.LoanlogNavigation, "Createdat"))
        .ToList();
}

    public Loanlog? GetById(int loanLogId)
    {
        return context.Loanlogs
            .Include(l => l.LoanlogNavigation)
            .FirstOrDefault(l => EF.Property<int>(l, "Loanlogid") == loanLogId);
    }

    public bool ExistsByLoanListId(int loanListId)
    {
        return context.Loanlogs.Any(l => EF.Property<int>(l, "Loanlistid") == loanListId);
    }
}