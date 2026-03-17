using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Checkout
{
    public CheckoutStatus? Status { get; set; }
    public PaymentMethod? PaymentMethodType { get; set; }
}