namespace ProRental.Domain.Entities;

using ProRental.Domain.Enums;

/// <summary>
/// Partial class extending the auto-generated Rentalorderlog entity.
/// Public accessors delegate to the scaffolded private properties,
/// NOT directly to backing fields — avoids EF field conflict validation errors.
/// </summary>
public partial class Rentalorderlog
{
    public int rentalorderlogid
    {
        get => Rentalorderlogid;
        set => Rentalorderlogid = value;
    }

    public int? orderid
    {
        get => Orderid;
        set => Orderid = value;
    }

    public int? customerid
    {
        get => Customerid;
        set => Customerid = value;
    }

    public DateTime? orderdate
    {
        get => Orderdate;
        set => Orderdate = value;
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

    // DeliveryType — PostgreSQL enum (delivery_type_enum)
    private DeliveryType _deliverytype;
    private DeliveryType Deliverytype { get => _deliverytype; set => _deliverytype = value; }

    public DeliveryType deliverytype
    {
        get => Deliverytype;
        set => Deliverytype = value;
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