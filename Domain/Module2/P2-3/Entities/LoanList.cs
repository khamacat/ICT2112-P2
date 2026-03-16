using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Loanlist
{
	public LoanStatus Status { get; private set; }
}