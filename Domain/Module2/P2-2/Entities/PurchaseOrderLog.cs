namespace ProRental.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using ProRental.Domain.Enums;

public partial class Purchaseorderlog
{
    [NotMapped]
    public int purchaseorder_logid
    {
        get => Purchaseorderlogid;
        set => Purchaseorderlogid = value;
    }

    [NotMapped]
    public int po_id
    {
        get => Poid;
        set => Poid = value;
    }

    [NotMapped]
    public DateTime? po_date
    {
        get => Podate;
        set => Podate = value;
    }

    [NotMapped]
    public int? supplier_id
    {
        get => Supplierid;
        set => Supplierid = value;
    }

    [NotMapped]
    public DateTime? expected_deliverydate
    {
        get => Expecteddeliverydate;
        set => Expecteddeliverydate = value;
    }

    [NotMapped]
    public decimal? total_amount
    {
        get => Totalamount;
        set => Totalamount = value;
    }

    [NotMapped]
    public string? details_json
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    private RentalStatus _status;
    private RentalStatus Status { get => _status; set => _status = value; }

    [NotMapped]
    public RentalStatus po_status
    {
        get => Status;
        set => Status = value;
    }
}