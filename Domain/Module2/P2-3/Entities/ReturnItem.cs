using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnitem
{
    private ReturnItemStatus _status;
    private ReturnItemStatus Status { get => _status; set => _status = value; }
    public void UpdateStatus(ReturnItemStatus newValue) => _status = newValue;
}