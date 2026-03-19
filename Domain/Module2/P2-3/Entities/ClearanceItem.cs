using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Clearanceitem
{
    private ClearanceStatus _status;
    private ClearanceStatus Status { get => _status; set => _status = value; }
    public void UpdateStatus(ClearanceStatus newValue) => _status = newValue;
}