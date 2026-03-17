using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Payment
{
    public PaymentPurpose? Purpose { get; set; }
    public TransactionStatus? Status { get; set; }
}