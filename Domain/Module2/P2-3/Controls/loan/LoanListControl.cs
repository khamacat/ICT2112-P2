using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class LoanListControl : ILoanListQuery, ILoanListCRUD, ILoanActions, ILoanValidation
{
    private readonly ILoanListMapper _loanListMapper;
    private readonly ILoanItemCRUD _loanItemCRUD;
    private readonly ILoanItemQuery _loanItemQuery;

    public LoanListControl(
        ILoanListMapper loanListMapper, 
        ILoanItemCRUD loanItemCRUD, 
        ILoanItemQuery loanItemQuery)
    {
        _loanListMapper = loanListMapper;
        _loanItemCRUD = loanItemCRUD;
        _loanItemQuery = loanItemQuery;
    }

    // ── Pre-Flight Validation (ILoanValidation) ──────────────────────────────
    
    public bool ValidateLoanParameters(int orderId, DateTime startDate, DateTime dueDate)
    {
        // 1. Time Validation
        if (startDate > dueDate)
            return false;

        // 2. Duplicate Check: Does this Order ID already have a Loanlist?
        var existingLoan = _loanListMapper.FindByOrderId(orderId);
        if (existingLoan != null)
            return false; // Reject: Cannot create two loan lists for the same order

        return true;
    }

    // ── Queries (ILoanListQuery) ─────────────────────────────────────────────

    public Loanlist? GetLoanListById(int loanlistid)
    {
        return _loanListMapper.FindById(loanlistid);
    }

    public List<Loanlist>? GetAllLoanList()
    {
        var results = _loanListMapper.FindAll();
        return results?.ToList() ?? new List<Loanlist>();
    }

    public List<int>? GetInventoryItemList(int orderid)
    {
        var loanList = _loanListMapper.FindByOrderId(orderid);
        if (loanList == null)
            return new List<int>();

        // Delegate to LoanItemQuery to get the physical inventory IDs
        return _loanItemQuery.GetInventoryItemIdByList(loanList.GetLoanListId());
    }

    // ── CRUD Operations (ILoanListCRUD) ──────────────────────────────────────

    public bool CreateLoanList(int orderId, int customerId, DateTime startDate, DateTime dueDate)
    {
        try
        {
            var newList = Loanlist.Create(orderId, customerId, startDate, dueDate);
            _loanListMapper.Insert(newList);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateLoanList(Loanlist loan)
    {
        try
        {
            _loanListMapper.Update(loan);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteLoanList(int loanlistid)
    {
        try
        {
            var existing = _loanListMapper.FindById(loanlistid);
            if (existing == null) return false;

            // Referential Integrity: Delete child items first
            var childItems = _loanItemQuery.GetAllLoanItems()?
                .Where(i => i.GetLoanListId() == loanlistid).ToList();

            if (childItems != null)
            {
                foreach (var item in childItems)
                {
                    _loanItemCRUD.DeleteLoanItem(item.GetLoanItemId());
                }
            }

            _loanListMapper.Delete(existing);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ── Domain Actions Orchestration (ILoanActions) ──────────────────────────

    public bool ProcessLoan(int orderId, int customerId, DateTime startDate, DateTime dueDate, List<int> inventoryItemIds)
    {
        // 1. Create the parent Loanlist
        bool listCreated = CreateLoanList(orderId, customerId, startDate, dueDate);
        if (!listCreated) 
            return false;

        // 2. Fetch the newly created list to get its Database-generated ID
        var newLoanList = _loanListMapper.FindByOrderId(orderId);
        if (newLoanList == null) 
            return false;

        // 3. Orchestrate the creation of all child Loanitems
        foreach (var invId in inventoryItemIds)
        {
            _loanItemCRUD.CreateLoanItem(newLoanList.GetLoanListId(), invId);
        }

        // 4. Update status to ON_LOAN
        newLoanList.ProcessLoan();
        _loanListMapper.Update(newLoanList);

        return true;
    }

    public bool CloseLoan(int orderId)
    {
        try
        {
            var loanList = _loanListMapper.FindByOrderId(orderId);
            if (loanList == null) return false;

            // Use the intrinsic domain method to safely process the return
            loanList.ProcessReturn();

            _loanListMapper.Update(loanList);
            return true;
        }
        catch
        {
            return false;
        }
    }
}