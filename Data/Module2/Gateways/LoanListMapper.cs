using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class LoanListMapper : ILoanListMapper
{
    private readonly AppDbContext _context;

    public LoanListMapper(AppDbContext context)
    {
        _context = context;
    }

    public Loanlist? FindById(int listId)
    {
        // RULE: Use EF.Property to access the private 'Loanlistid'.
        return _context.Loanlists
            .FirstOrDefault(l => EF.Property<int>(l, "Loanlistid") == listId);
    }

    public Loanlist? FindByOrderId(int orderId)
    {
        // RULE: Use EF.Property to access the private 'Orderid'.
        return _context.Loanlists
            .FirstOrDefault(l => EF.Property<int>(l, "Orderid") == orderId);
    }

    public ICollection<Loanlist>? FindAll()
    {
        return _context.Loanlists.ToList();
    }

    public ICollection<Loanlist>? FindByBorrowerId(int borrowerId)
    {
        // Interface calls this "BorrowerId", but schema calls it "Customerid"
        return _context.Loanlists
            .Where(l => EF.Property<int>(l, "Customerid") == borrowerId)
            .ToList();
    }

    public ICollection<Loanlist>? FindByDate(DateTime loanDate)
    {
        // Match the requested date against the private 'Loandate'
        // Using .Date ensures we match the day regardless of the specific timestamp time
        return _context.Loanlists
            .Where(l => EF.Property<DateTime>(l, "Loandate").Date == loanDate.Date)
            .ToList();
    }

    public ICollection<Loanlist>? FindByStatus(LoanStatus status)
    {
        // RULE: Use EF.Property to query against the private 'Status' enum from the manual partial class.
        return _context.Loanlists
            .Where(l => EF.Property<LoanStatus>(l, "Status") == status)
            .ToList();
    }

    public void Insert(Loanlist list)
    {
        _context.Loanlists.Add(list);
        _context.SaveChanges();
    }

    public void Update(Loanlist list)
    {
        var existing = _context.Loanlists
            .FirstOrDefault(l => EF.Property<int>(l, "Loanlistid") == list.GetLoanListId());

        if (existing == null) return;

        _context.Entry(existing).CurrentValues.SetValues(list);
        _context.SaveChanges();
    }

    public void Delete(Loanlist list)
    {
        var existing = _context.Loanlists
            .FirstOrDefault(l => EF.Property<int>(l, "Loanlistid") == list.GetLoanListId());
            
        if (existing != null)
        {
            _context.Loanlists.Remove(existing);
            _context.SaveChanges();
        }
    }
}