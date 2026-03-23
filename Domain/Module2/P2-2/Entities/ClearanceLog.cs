namespace ProRental.Domain.Entities;

using ProRental.Domain.Enums;

/// <summary>
/// Partial class extending the auto-generated Clearancelog entity.
/// NOTE: Class name must be Clearancelog (not ClearanceLog) to match scaffolded entity.
/// </summary>
public partial class Clearancelog
{
    public int clearancelogid
    {
        get => Clearancelogid;
        set => Clearancelogid = value;
    }

    public int clearancebatchid
    {
        get => Clearancebatchid;
        set => Clearancebatchid = value;
    }

    public string? batchname
    {
        get => Batchname;
        set => Batchname = value;
    }

    public DateTime? clearancedate
    {
        get => Clearancedate;
        set => Clearancedate = value;
    }

    public string? detailsjson
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    // Status — PostgreSQL enum (clearance_status_enum)
    // TODO: Confirm with team whether ClearanceLogStatus enum exists or needs to be created.
    //       Values: ONGOING, COMPLETED, CANCELLED
    //       This is different from ClearanceStatus (CLEARANCE, SOLD) used by ClearanceItem.
    // private ClearanceLogStatus _status;
    // private ClearanceLogStatus Status { get => _status; set => _status = value; }
    // public ClearanceLogStatus status
    // {
    //     get => Status;
    //     set => Status = value;
    // }
}