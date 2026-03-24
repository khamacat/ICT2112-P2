using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ReturnItemControl : iReturnItemCRUD, iReturnItemQuery
{
    private readonly IReturnItemMapper       _returnItemMapper;
    private readonly iInventoryStatusControl _inventoryStatusControl;

    public ReturnItemControl(
        IReturnItemMapper        returnItemMapper,
        iInventoryStatusControl  inventoryStatusControl)
    {
        _returnItemMapper       = returnItemMapper       ?? throw new ArgumentNullException(nameof(returnItemMapper));
        _inventoryStatusControl = inventoryStatusControl ?? throw new ArgumentNullException(nameof(inventoryStatusControl));
    }

    // -- iReturnItemQuery -------------------------------------------------

    public Returnitem? GetReturnItem(int returnItemId)
    {
        return _returnItemMapper.FindById(returnItemId);
    }

    public List<Returnitem> GetReturnItemByRequestId(int returnRequestId)
    {
        return _returnItemMapper.FindByReturnRequest(returnRequestId)?.ToList()
               ?? new List<Returnitem>();
    }

    // -- iReturnItemCRUD --------------------------------------------------

    public bool CreateReturnItem(Returnitem returnItem)
    {
        if (returnItem is null) return false;
        try { _returnItemMapper.Insert(returnItem); return true; }
        catch { return false; }
    }

    public bool UpdateReturnItem(Returnitem returnItem)
    {
        if (returnItem is null) return false;
        try
        {
            var fresh = _returnItemMapper.FindById(returnItem.GetReturnItemId());
            if (fresh is null) return false;
            fresh.SetStatus(returnItem.GetStatus());
            if (returnItem.GetCompletionDate().HasValue)
                fresh.SetCompletionDate(returnItem.GetCompletionDate()!.Value);
            _returnItemMapper.Update(fresh);
            return true;
        }
        catch { return false; }
    }

    // -- Control-level methods (from diagram) -----------------------------

    public bool UpdateReturnItemStatus(int returnItemId, string status)
    {
        if (!Enum.TryParse<ReturnItemStatus>(status, out var parsedStatus)) return false;
        var fresh = _returnItemMapper.FindById(returnItemId);
        if (fresh is null) return false;
        fresh.SetStatus(parsedStatus);
        try { _returnItemMapper.Update(fresh); return true; }
        catch { return false; }
    }

    public bool AcknowledgeReturn(int returnItemId, int staffId)
    {
        var fresh = _returnItemMapper.FindById(returnItemId);
        if (fresh is null) return false;
        fresh.ConductInspection();
        try { _returnItemMapper.Update(fresh); return true; }
        catch { return false; }
    }

    public bool RejectReturn(int returnItemId, int staffId, string reason)
    {
        var fresh = _returnItemMapper.FindById(returnItemId);
        if (fresh is null || string.IsNullOrWhiteSpace(reason)) return false;
        try { _returnItemMapper.Update(fresh); return true; }
        catch { return false; }
    }

    /// <summary>
    /// Sets item RETURN_TO_INVENTORY and updates inventory to AVAILABLE.
    /// Also checks if damage report exists via iDamageReportQuery.
    /// </summary>
    public bool CompleteReturnItemProcess(int returnItemId)
    {
        var fresh = _returnItemMapper.FindById(returnItemId);
        if (fresh is null) return false;

        fresh.CompleteReturn();

        try
        {
            _returnItemMapper.Update(fresh);

            // Notify inventory: MAINTENANCE → AVAILABLE
            _inventoryStatusControl.UpdateInventoryStatus(
                fresh.GetInventoryItemId(),
                InventoryStatus.AVAILABLE);

            return true;
        }
        catch { return false; }
    }
}