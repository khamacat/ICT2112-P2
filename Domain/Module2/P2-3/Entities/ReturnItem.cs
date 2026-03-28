using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnitem
{
    // PUBLIC GETTERS
    public static Returnitem Create(int returnRequestId, int inventoryItemId)
    {
        var item = new Returnitem();
        item.SetReturnRequestId(returnRequestId);
        item.SetInventoryItemId(inventoryItemId);
        item.SetStatus(ReturnItemStatus.DAMAGE_INSPECTION);
        return item;
    }

    public int GetReturnItemId() => _returnitemid;

    public int GetReturnRequestId() => _returnrequestid;

    public int GetInventoryItemId() => _inventoryitemid;

    public ReturnItemStatus GetStatus() => _status;

    public string? GetImageUrl() => _image;

    public DateTime? GetCompletionDate() => _completiondate;

    public void UpdateStatus(ReturnItemStatus status) => _status = status;

    public void UpdateImageUrl(string imageUrl) => _image = imageUrl;

    public void UpdateCompletionDate(DateTime date) => _completiondate = date;

    public void ConductInspection() => _status = ReturnItemStatus.DAMAGE_INSPECTION;

    public void ConductRepairing() => _status = ReturnItemStatus.REPAIRING;

    public void ConductServicing() => _status = ReturnItemStatus.SERVICING;

    public void ConductCleaning() => _status = ReturnItemStatus.CLEANING;

    public void CompleteReturn()
    {
        _status = ReturnItemStatus.RETURN_TO_INVENTORY;
        _completiondate = DateTime.UtcNow;
    }

    // PRIVATE SETTERS 
    private ReturnItemStatus _status;

    private ReturnItemStatus Status
    {
        get => _status;
        set => _status = value;
    }

    private void SetReturnItemId(int id) => _returnitemid = id;

    private void SetReturnRequestId(int id) => _returnrequestid = id;

    private void SetInventoryItemId(int id) => _inventoryitemid = id;

    private void SetStatus(ReturnItemStatus status) => _status = status;
}