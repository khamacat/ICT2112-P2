using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;

public partial class Order
{
    private OrderStatus? _status;
    private OrderStatus? Status { get => _status; set => _status = value; }
    public void UpdateStatus(OrderStatus newValue) => _status = newValue;

    private DeliveryDuration? _deliveryType;
    private DeliveryDuration? DeliveryType { get => _deliveryType; set => _deliveryType = value; }
    public void UpdateDeliveryType(DeliveryDuration newValue) => _deliveryType = newValue;
}