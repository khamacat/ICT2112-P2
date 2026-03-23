namespace ProRental.Domain.Entities;

using ProRental.Domain.Enums;

/// <summary>
/// Partial class extending the auto-generated Purchaseorderlog entity.
/// NOTE: Class name must be Purchaseorderlog (not PurchaseOrderLog) to match scaffolded entity.
/// </summary>
public partial class Purchaseorderlog
{
    public int purchaseorderlogid
    {
        get => Purchaseorderlogid;
        set => Purchaseorderlogid = value;
    }

    public int poid
    {
        get => Poid;
        set => Poid = value;
    }

    public DateTime? podate
    {
        get => Podate;
        set => Podate = value;
    }

    public int? supplierid
    {
        get => Supplierid;
        set => Supplierid = value;
    }

    public DateTime? expecteddeliverydate
    {
        get => Expecteddeliverydate;
        set => Expecteddeliverydate = value;
    }

    public decimal? totalamount
    {
        get => Totalamount;
        set => Totalamount = value;
    }

    public string? detailsjson
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    // Status — PostgreSQL enum (rental_status_enum)
    private RentalStatus _status;
    private RentalStatus Status { get => _status; set => _status = value; }

    public RentalStatus status
    {
        get => Status;
        set => Status = value;
    }
}