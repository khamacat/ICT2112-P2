using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class DeliveryBatch
{
    private BatchStatus? _deliveryBatchStatus;
    private BatchStatus? DeliveryBatchStatus { get => _deliveryBatchStatus; set => _deliveryBatchStatus = value; }
    public void UpdateDeliveryBatchStatus(BatchStatus newValue) => _deliveryBatchStatus = newValue;
}
