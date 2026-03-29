using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class LoanItemMapper : ILoanItemMapper
{
    private readonly AppDbContext _context;

    public LoanItemMapper(AppDbContext context)
    {
        _context = context;
    }

    public Loanitem? FindById(int itemId)
    {
        // RULE: Use EF.Property to access the private 'Loanitemid'.
        return _context.Loanitems
            .FirstOrDefault(l => EF.Property<int>(l, "Loanitemid") == itemId);
    }

    public ICollection<Loanitem>? FindByLoanListId(int listId)
    {
        // RULE: Use EF.Property to query against the private 'Loanlistid'.
        return _context.Loanitems
            .Where(l => EF.Property<int>(l, "Loanlistid") == listId)
            .ToList();
    }

    public ICollection<Loanitem>? FindAll()
    {
        return _context.Loanitems.ToList();
    }

    public void Insert(Loanitem item)
    {
        _context.Loanitems.Add(item);
        _context.SaveChanges();
    }

    public void Update(Loanitem item)
    {
        var existing = _context.Loanitems
            .FirstOrDefault(l => EF.Property<int>(l, "Loanitemid") == item.GetLoanItemId());

        if (existing == null) return;

        _context.Entry(existing).CurrentValues.SetValues(item);
        _context.SaveChanges();
    }

    public void Delete(Loanitem item)
    {
        var existing = _context.Loanitems
            .FirstOrDefault(l => EF.Property<int>(l, "Loanitemid") == item.GetLoanItemId());
            
        if (existing != null)
        {
            _context.Loanitems.Remove(existing);
            _context.SaveChanges();
        }
    }
}