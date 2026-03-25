namespace ProRental.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using ProRental.Domain.Enums;

public partial class Clearancelog
{
    [NotMapped]
    public int clearance_logid
    {
        get => Clearancelogid;
        set => Clearancelogid = value;
    }

    [NotMapped]
    public int clearance_batchid
    {
        get => Clearancebatchid;
        set => Clearancebatchid = value;
    }

    [NotMapped]
    public string? batch_name
    {
        get => Batchname;
        set => Batchname = value;
    }

    [NotMapped]
    public DateTime? clearance_date
    {
        get => Clearancedate;
        set => Clearancedate = value;
    }

    [NotMapped]
    public string? details_json
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    // TODO: Uncomment once ClearanceLogStatus enum is confirmed
    // private ClearanceLogStatus _status;
    // private ClearanceLogStatus Status { get => _status; set => _status = value; }
    // [NotMapped]
    // public ClearanceLogStatus clearance_status
    // {
    //     get => Status;
    //     set => Status = value;
    // }
}