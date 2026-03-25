namespace ProRental.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using ProRental.Domain.Enums;

public partial class Transactionlog
{
    [NotMapped]
    public int transaction_logid
    {
        get => Transactionlogid;
        set => Transactionlogid = value;
    }

    [NotMapped]
    public DateTime? created_at
    {
        get => Createdat;
        set => Createdat = value;
    }

    private LogType _logtype;
    private LogType Logtype { get => _logtype; set => _logtype = value; }

    [NotMapped]
    public LogType log_type
    {
        get => Logtype;
        set => Logtype = value;
    }
}