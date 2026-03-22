namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Lineitem
{
    // Private field for enum (not scaffolded by EF Core)
    private ReplenishmentReason? _reasoncode;

    // Public accessors for scaffolded private fields with column mappings
    // TEST: avoid duplicate EF column mapping with scaffolded Lineitemid property
    [NotMapped]
    public int LineItemId
    {
        get => _lineitemid;
        set => _lineitemid = value;
    }

    // TEST: avoid duplicate EF column mapping with scaffolded Requestid property
    [NotMapped]
    public int? RequestId
    {
        get => _requestid;
        set => _requestid = value;
    }

    // TEST: avoid duplicate EF column mapping with scaffolded Productid property
    [NotMapped]
    public int? ProductId
    {
        get => _productid;
        set => _productid = value;
    }

    // TEST: avoid duplicate EF column mapping with scaffolded Quantityrequest property
    [NotMapped]
    public int? QuantityRequest
    {
        get => _quantityrequest;
        set => _quantityrequest = value;
    }

    // Public accessor for Reason (enum) with default value
    [Column("reasoncode")]
    public ReplenishmentReason Reason
    {
        get => _reasoncode ?? ReplenishmentReason.LOWSTOCK;
        set => _reasoncode = value;
    }

    // Business logic methods from class diagram
    // Set quantity for this line item
    public bool SetQuantity(int quantity)
    {
        if (quantity < 0)
        {
            return false;
        }

        QuantityRequest = quantity;
        return true;
    }

    // Set the reason code for this line item
    public void SetReason(ReplenishmentReason reason)
    {
        Reason = reason;
    }


    // Set remarks for this line item
    public void SetRemarks(string remarks)
    {
        _remarks = remarks;
    }

    // Get remarks for this line item
    public string? GetRemarks()
    {
        return _remarks;
    }

    // Validate if the line item has required data
    public bool IsValid()
    {
        return ProductId.HasValue &&
               ProductId.Value > 0 &&
               QuantityRequest.HasValue &&
               QuantityRequest.Value > 0;
    }
}
