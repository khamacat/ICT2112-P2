namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Lineitem
{
    private ReplenishmentReason? _reasoncode;

    [Column("reasoncode")]
    public ReplenishmentReason Reason
    {
        get => _reasoncode ?? ReplenishmentReason.LOWSTOCK;
        set => _reasoncode = value;
    }

    public int GetLineItemId() => _lineitemid;

    public int? GetRequestId() => _requestid;

    public int? GetProductId() => _productid;

    public int? GetQuantityRequest() => _quantityrequest;

    public string? GetRemarks() => _remarks;

    public void InitializeForRequest(int requestId, int productId)
    {
        _requestid = requestId;
        _productid = productId;
        _quantityrequest = 0;
        Reason = ReplenishmentReason.LOWSTOCK;
    }

    public bool SetQuantity(int quantity)
    {
        if (quantity < 0)
        {
            return false;
        }

        _quantityrequest = quantity;
        return true;
    }

    public void SetReason(ReplenishmentReason reason)
    {
        Reason = reason;
    }


    public void SetRemarks(string? remarks)
    {
        _remarks = remarks;
    }

    public bool IsValid()
    {
        return _productid.HasValue &&
               _productid.Value > 0 &&
               _quantityrequest.HasValue &&
               _quantityrequest.Value > 0;
    }
}
