using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Clearancebatch
{
	public ClearanceBatchStatus Status { get; private set; }
}