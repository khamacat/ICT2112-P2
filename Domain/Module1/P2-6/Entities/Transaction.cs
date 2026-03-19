using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Transaction
{
    private TransactionType? _type;
    private TransactionType? Type { get => _type; set => _type = value; }
    public void UpdateType(TransactionType newValue) => _type = newValue;

    private TransactionPurpose? _purpose;
    private TransactionPurpose? Purpose { get => _purpose; set => _purpose = value; }
    public void UpdatePurpose(TransactionPurpose newValue) => _purpose = newValue;

    private TransactionStatus? _status;
    private TransactionStatus? Status { get => _status; set => _status = value; }
    public void UpdateStatus(TransactionStatus newValue) => _status = newValue;
}