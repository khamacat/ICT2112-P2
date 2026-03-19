namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Replenishmentrequest
{
    private ReplenishmentStatus _status;
    private ReplenishmentStatus status { get => _status; set => _status = value; }
    public void UpdateStatus(ReplenishmentStatus newValue) => _status = newValue;
}