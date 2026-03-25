namespace ProRental.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using ProRental.Domain.Enums;

public partial class Rentalorderlog
{
    [NotMapped]
    public int rental_orderlogid
    {
        get => Rentalorderlogid;
        set => Rentalorderlogid = value;
    }

    [NotMapped]
    public int? order_id
    {
        get => Orderid;
        set => Orderid = value;
    }

    [NotMapped]
    public int? customer_id
    {
        get => Customerid;
        set => Customerid = value;
    }

    [NotMapped]
    public DateTime? order_date
    {
        get => Orderdate;
        set => Orderdate = value;
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

    private DeliveryType _deliverytype;
    private DeliveryType Deliverytype { get => _deliverytype; set => _deliverytype = value; }

    [NotMapped]
    public DeliveryType delivery_type
    {
        get => Deliverytype;
        set => Deliverytype = value;
    }

    private RentalStatus _status;
    private RentalStatus Status { get => _status; set => _status = value; }

    [NotMapped]
    public RentalStatus rental_status
    {
        get => Status;
        set => Status = value;
    }
}