namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Lineitem
{
    private ReplenishmentReason _reason;
    private ReplenishmentReason reason { get => _reason; set => _reason = value; }
    public void UpdateReason(ReplenishmentReason newValue) => _reason = newValue;
}