using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Clearancebatch
{
    private ClearanceBatchStatus _status;
    private ClearanceBatchStatus Status { get => _status; set => _status = value; }
    public void UpdateStatus(ClearanceBatchStatus newValue) => _status = newValue;
}