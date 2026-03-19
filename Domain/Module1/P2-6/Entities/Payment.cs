using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Payment
{
    private PaymentPurpose? _purpose;
    private PaymentPurpose? Purpose { get => _purpose; set => _purpose = value; }
    public void UpdatePurpose(PaymentPurpose newValue) => _purpose = newValue;

    private TransactionStatus? _status;
    private TransactionStatus? Status { get => _status; set => _status = value; }
    public void UpdateStatus(TransactionStatus newValue) => _status = newValue;
}