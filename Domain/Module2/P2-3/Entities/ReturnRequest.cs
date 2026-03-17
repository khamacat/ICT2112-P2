using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnrequest
{
	public ReturnRequestStatus Status { get; private set; }
}