namespace ProRental.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using ProRental.Domain.Enums;

public partial class Loanlog
{
    [NotMapped]
    public int loan_logid
    {
        get => Loanlogid;
        set => Loanlogid = value;
    }

    [NotMapped]
    public int loan_listid
    {
        get => Loanlistid;
        set => Loanlistid = value;
    }

    [NotMapped]
    public int rental_orderlogid
    {
        get => Rentalorderlogid;
        set => Rentalorderlogid = value;
    }

    [NotMapped]
    public DateTime? loan_date
    {
        get => Loandate;
        set => Loandate = value;
    }

    [NotMapped]
    public DateTime? return_date
    {
        get => Returndate;
        set => Returndate = value;
    }

    [NotMapped]
    public DateTime? due_date
    {
        get => Duedate;
        set => Duedate = value;
    }

    [NotMapped]
    public string? details_json
    {
        get => Detailsjson;
        set => Detailsjson = value;
    }

    // TODO: Uncomment once LoanLogStatus enum is confirmed
    // private LoanLogStatus _status;
    // private LoanLogStatus Status { get => _status; set => _status = value; }
    // [NotMapped]
    // public LoanLogStatus loan_status
    // {
    //     get => Status;
    //     set => Status = value;
    // }
}