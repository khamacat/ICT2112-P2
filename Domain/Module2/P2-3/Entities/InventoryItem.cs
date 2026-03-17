using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Inventoryitem
{
	public InventoryStatus Status { get; private set; }
}