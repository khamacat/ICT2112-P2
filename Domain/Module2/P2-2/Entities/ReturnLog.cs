namespace ProRental.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using ProRental.Domain.Enums;

public partial class Returnlog
{
    [NotMapped]
    public int return_logid
    {
        get => Returnlogid;
        set => Returnlogid = value;
    }

    [NotMapped]
    public int return_requestid
    {
        get => Returnrequestid;
        set => Returnrequestid = value;
    }

    [NotMapped]
    public int rental_orderlogid
    {
        get => Rentalorderlogid;
        set => Rentalorderlogid = value;
    }

    [NotMapped]
    public string? customer_id
    {
        get => Customerid;
        set => Customerid = value;
    }

    [NotMapped]
    public DateTime? request_date
    {
        get => Requestdate;
        set => Requestdate = value;
    }

    [NotMapped]
    public DateTime? completion_date
    {
        get => Completiondate;
        set => Completiondate = value;
    }

    [NotMapped]
    public string? image_url
    {
        get => Imageurl;
        set => Imageurl = value;
    }

    [NotMapped]
    public string? details_json
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    private ReturnStatus _status;
    private ReturnStatus Status { get => _status; set => _status = value; }

    [NotMapped]
    public ReturnStatus return_status
    {
        get => Status;
        set => Status = value;
    }
}