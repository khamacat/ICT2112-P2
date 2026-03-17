using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnitem
{
	public ReturnItemStatus Status { get; private set; }
}