using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Order
{
    public OrderStatus? Status { get; set; }
    public DeliveryDuration? DeliveryType { get; set; }
}