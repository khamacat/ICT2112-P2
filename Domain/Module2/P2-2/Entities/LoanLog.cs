namespace ProRental.Domain.Entities;

using ProRental.Domain.Enums;

/// <summary>
/// Partial class extending the auto-generated Loanlog entity.
/// NOTE: Class name must be Loanlog (not LoanLog) to match scaffolded entity.
/// </summary>
public partial class Loanlog
{
    public int loanlogid
    {
        get => Loanlogid;
        set => Loanlogid = value;
    }

    public int loanlistid
    {
        get => Loanlistid;
        set => Loanlistid = value;
    }

    public int rentalorderlogid
    {
        get => Rentalorderlogid;
        set => Rentalorderlogid = value;
    }

    public DateTime? loandate
    {
        get => Loandate;
        set => Loandate = value;
    }

    public DateTime? returndate
    {
        get => Returndate;
        set => Returndate = value;
    }

    public DateTime? duedate
    {
        get => Duedate;
        set => Duedate = value;
    }

    public string? detailsjson
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    // Status — PostgreSQL enum (loan_status_enum)
    // TODO: Confirm with team whether LoanLogStatus enum exists or needs to be created.
    //       Values: ONGOING, RETURNED, OVERDUE, CANCELLED
    //       This is different from LoanStatus (OPEN, ON_LOAN, RETURNED) used by LoanList.
    // private LoanLogStatus _status;
    // private LoanLogStatus Status { get => _status; set => _status = value; }
    // public LoanLogStatus status
    // {
    //     get => Status;
    //     set => Status = value;
    // }
}