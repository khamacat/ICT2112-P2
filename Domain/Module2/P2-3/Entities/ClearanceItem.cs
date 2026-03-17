using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Clearanceitem
{
	public ClearanceStatus Status { get; private set; }
}