using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;

public partial class Checkout
{
    private CheckoutStatus? _status;
    private CheckoutStatus? Status { get => _status; set => _status = value; }
    public void UpdateStatus(CheckoutStatus newValue) => _status = newValue;

    private PaymentMethod? _paymentMethodType;
    private PaymentMethod? PaymentMethodType { get => _paymentMethodType; set => _paymentMethodType = value; }
    public void UpdatePaymentMethodType(PaymentMethod newValue) => _paymentMethodType = newValue;
}