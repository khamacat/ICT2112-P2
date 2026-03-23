namespace ProRental.Domain.Entities;

using ProRental.Domain.Enums;

/// <summary>
/// Partial class extending the auto-generated Transactionlog entity.
/// Public accessors delegate to the scaffolded private properties,
/// NOT directly to backing fields — avoids EF field conflict validation errors.
/// </summary>
public partial class Transactionlog
{
    public int transactionlogid
    {
        get => Transactionlogid;
        set => Transactionlogid = value;
    }

    public DateTime? createdat
    {
        get => Createdat;
        set => Createdat = value;
    }

    // LogType is a PostgreSQL enum (log_type_enum) that the scaffolder
    // doesn't generate a property for — it's handled via AppDbContext.Custom.cs.
    // We declare the backing field + private property + public accessor here.
    private LogType _logtype;
    private LogType Logtype { get => _logtype; set => _logtype = value; }

    public LogType logtype
    {
        get => Logtype;
        set => Logtype = value;
    }
}