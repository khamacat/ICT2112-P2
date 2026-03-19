namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Purchaseorder
{
    private PurchaseOrderStatus _status;
    private PurchaseOrderStatus status { get => _status; set => _status = value; }
    public void UpdateStatus(PurchaseOrderStatus newValue) => _status = newValue;
}