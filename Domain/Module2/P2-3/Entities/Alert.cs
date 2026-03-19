using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Alert
{
    private AlertStatus _status;
    private AlertStatus Status { get => _status; set => _status = value; }
    public void UpdateStatus(AlertStatus newValue) => _status = newValue;
}