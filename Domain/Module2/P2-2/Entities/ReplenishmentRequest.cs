namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Replenishmentrequest
{
    private ReplenishmentStatus? _status;

    [Column("status")]
    public ReplenishmentStatus Status
    {
        get => _status ?? ReplenishmentStatus.DRAFT;
        set => _status = value;
    }

    public int GetRequestId() => _requestid;

    public string? GetRequestedBy() => _requestedby;

    public DateTime? GetCreatedAt() => _createdat;

    public DateTime? GetCompletedAt() => _completedat;

    public string? GetCompletedBy() => _completedby;

    public string? GetRemarks() => _remarks;

    public void InitializeDraft(string requestedByStaffId, DateTime createdAtUtc)
    {
        _requestedby = requestedByStaffId;
        Status = ReplenishmentStatus.DRAFT;
        _createdat = createdAtUtc;
    }

    public bool CanMarkComplete() => Status == ReplenishmentStatus.SUBMITTED;

    public bool MarkComplete(string completedByStaffId, DateTime completedAtUtc)
    {
        if (!CanMarkComplete())
        {
            return false;
        }

        Status = ReplenishmentStatus.COMPLETED;
        _completedat = completedAtUtc;
        _completedby = completedByStaffId;
        return true;
    }

    public void SetRemarks(string? remarks)
    {
        _remarks = remarks;
    }

    public bool CanEdit()
    {
        return Status == ReplenishmentStatus.DRAFT;
    }

    public bool HasLineItem()
    {
        return Lineitems != null && Lineitems.Any();
    }

    public Lineitem? FindLineItem(int lineItemId)
    {
        return Lineitems?.FirstOrDefault(li => li.GetLineItemId() == lineItemId);
    }

    public Lineitem AddLineItem(int productId)
    {
        if (!CanEdit())
        {
            throw new InvalidOperationException("Cannot add line items to a non-draft request");
        }

        var lineItem = new Lineitem();
        lineItem.InitializeForRequest(_requestid, productId);
        lineItem.SetRemarks(string.Empty);

        Lineitems.Add(lineItem);
        return lineItem;
    }

    public bool RemoveLineItem(int lineItemId)
    {
        if (!CanEdit())
        {
            return false;
        }

        var lineItem = FindLineItem(lineItemId);
        if (lineItem == null)
        {
            return false;
        }

        Lineitems.Remove(lineItem);
        return true;
    }

    public bool Submit()
    {
        if (Status != ReplenishmentStatus.DRAFT)
        {
            return false;
        }

        if (!HasLineItem())
        {
            throw new InvalidOperationException("Cannot submit a request without line items");
        }

        foreach (var lineItem in Lineitems)
        {
            if (!lineItem.IsValid())
            {
                throw new InvalidOperationException($"Line item {lineItem.GetLineItemId()} is invalid");
            }
        }

        Status = ReplenishmentStatus.SUBMITTED;
        return true;
    }

    public bool Cancel()
    {
        if (Status == ReplenishmentStatus.COMPLETED || Status == ReplenishmentStatus.CANCELLED)
        {
            return false;
        }

        Status = ReplenishmentStatus.CANCELLED;
        return true;
    }
}
