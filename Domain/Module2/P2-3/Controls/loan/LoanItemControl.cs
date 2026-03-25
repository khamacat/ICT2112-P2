using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class LoanItemControl : ILoanItemQuery, ILoanItemCRUD
{
    private readonly ILoanItemMapper _loanItemMapper;

    public LoanItemControl(ILoanItemMapper loanItemMapper)
    {
        _loanItemMapper = loanItemMapper;
    }

    // ── Queries ──────────────────────────────────────────────────────────────

    public Loanitem? GetLoanItemById(int loanitemid)
    {
        return _loanItemMapper.FindById(loanitemid);
    }

    public List<Loanitem>? GetAllLoanItems()
    {
        var results = _loanItemMapper.FindAll();
        return results?.ToList() ?? new List<Loanitem>();
    }

    public List<int>? GetInventoryItemIdByList(int loanlistid)
    {
        // This is a highly useful utility method for the Facade to quickly figure
        // out which physical items belong to a specific loan list without dragging
        // around heavy objects.
        var items = _loanItemMapper.FindByLoanListId(loanlistid);
        
        return items?
            .Select(item => item.GetInventoryItemId())
            .ToList() ?? new List<int>();
    }

    // ── CRUD Operations ──────────────────────────────────────────────────────

    public bool CreateLoanItem(int loanlistid, int inventoryitemid)
    {
        try
        {
            // Utilize the intrinsic factory method we designed in the partial class
            var newItem = Loanitem.Create(loanlistid, inventoryitemid);
            
            _loanItemMapper.Insert(newItem);
            return true;
        }
        catch
        {
            // Catches DB constraints, e.g., if the loanlistid doesn't exist
            return false;
        }
    }

    public bool UpdateLoanItem(Loanitem loanitem)
    {
        try
        {
            _loanItemMapper.Update(loanitem);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteLoanItem(int loanitemid)
    {
        try
        {
            var existing = _loanItemMapper.FindById(loanitemid);
            if (existing == null) 
                return false;

            _loanItemMapper.Delete(existing);
            return true;
        }
        catch
        {
            return false;
        }
    }
}