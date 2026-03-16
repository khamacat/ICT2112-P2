using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Alert
{
	public AlertStatus Status { get; private set; }
}