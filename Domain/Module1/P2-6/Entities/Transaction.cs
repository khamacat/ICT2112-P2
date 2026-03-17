using ProRental.Domain.Enums;
namespace ProRental.Domain.Entities;
public partial class Transaction
{
    public TransactionType? Type { get; set; }
    public TransactionPurpose? Purpose { get; set; }
    public TransactionStatus? Status { get; set; }
}