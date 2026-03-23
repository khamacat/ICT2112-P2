namespace ProRental.Domain.Entities;

using ProRental.Domain.Enums;

/// <summary>
/// Partial class extending the auto-generated Returnlog entity.
/// NOTE: Class name must be Returnlog (not ReturnLog) to match scaffolded entity.
/// </summary>
public partial class Returnlog
{
    public int returnlogid
    {
        get => Returnlogid;
        set => Returnlogid = value;
    }

    public int returnrequestid
    {
        get => Returnrequestid;
        set => Returnrequestid = value;
    }

    public int rentalorderlogid
    {
        get => Rentalorderlogid;
        set => Rentalorderlogid = value;
    }

    public string? customerid
    {
        get => Customerid;
        set => Customerid = value;
    }

    public DateTime? requestdate
    {
        get => Requestdate;
        set => Requestdate = value;
    }

    public DateTime? completiondate
    {
        get => Completiondate;
        set => Completiondate = value;
    }

    public string? imageurl
    {
        get => Imageurl;
        set => Imageurl = value;
    }

    public string? detailsjson
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    // Status — PostgreSQL enum (return_status_enum)
    private ReturnStatus _status;
    private ReturnStatus Status { get => _status; set => _status = value; }

    public ReturnStatus status
    {
        get => Status;
        set => Status = value;
    }
}