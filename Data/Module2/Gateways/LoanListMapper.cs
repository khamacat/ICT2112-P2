using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

/* * =========================================================================
 * ARCHITECTURE & AI ASSISTANT RULES FOR THIS FILE
 * =========================================================================
 * 1. STRICT ENCAPSULATION: All entity properties (Loanlistid, Orderid, Customerid, etc.) are PRIVATE. 
 * Do NOT attempt to use standard LINQ (e.g., l.Loanlistid == id). 
 * You MUST use EF.Property<T>(entity, "PropertyName") for all queries.
 * 2. NO CROSS-AGGREGATE INCLUDES: Do NOT use .Include() for Customer, Order, Loanitems, or Loanlogs. 
 * If a developer needs the related entities, they must use those respective mappers.
 * 3. NO AUTO-UPDATEDAT: This specific entity relies on Loandate, Duedate, and Returndate. 
 * It DOES NOT have an Updatedat field. Do not hallucinate a DateTime.UtcNow override in Update().
 * =========================================================================
 */

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
        // Returndate, Remarks, and Status are updated via domain logic methods 
        // before calling this mapper Update.
        _context.Loanlists.Update(list);
        _context.SaveChanges();
    }

    public void Delete(Loanlist list)
    {
        _context.Loanlists.Remove(list);
        _context.SaveChanges();
    }
}