using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnitem
{
    private ReturnItemStatus _status;
    private ReturnItemStatus Status { get => _status; set => _status = value; }

    public int GetReturnItemId() => _returnitemid;
    public void SetReturnItemId(int id) => _returnitemid = id;

    public int GetReturnRequestId() => _returnrequestid;
    public void SetReturnRequestId(int id) => _returnrequestid = id;

    public int GetInventoryItemId() => _inventoryitemid;
    public void SetInventoryItemId(int id) => _inventoryitemid = id;

    public ReturnItemStatus GetStatus() => _status;
    public void SetStatus(ReturnItemStatus status) => _status = status;

    public string? GetImageUrl() => _image;
    public void SetImageUrl(string imageUrl) => _image = imageUrl;

    public DateTime? GetCompletionDate() => _completiondate;
    public void SetCompletionDate(DateTime date) => _completiondate = date;

    public void ConductInspection() => _status = ReturnItemStatus.DAMAGE_INSPECTION;
    public void ConductRepairing() => _status = ReturnItemStatus.REPAIRING;
    public void ConductServicing() => _status = ReturnItemStatus.SERVICING;
    public void ConductCleaning() => _status = ReturnItemStatus.CLEANING;

    public void CompleteReturn()
    {
        _status = ReturnItemStatus.RETURN_TO_INVENTORY;
        _completiondate = DateTime.UtcNow;
    }
}