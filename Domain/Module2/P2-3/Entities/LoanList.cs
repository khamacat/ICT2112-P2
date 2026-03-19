using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Loanlist
{
    private LoanStatus _status;
    private LoanStatus Status { get => _status; set => _status = value; }
    public void UpdateStatus(LoanStatus newValue) => _status = newValue;
}