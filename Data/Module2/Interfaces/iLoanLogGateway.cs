namespace ProRental.Data.Module2.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Gateway interface for the LoanLog child table.
/// Each row's PK (LoanLogId) must match an existing TransactionLogID.
/// </summary>
public interface ILoanLogGateway
{
    Loanlog Insert(Loanlog log);
    List<Loanlog> GetAll();
    Loanlog? GetById(int loanLogId);
    bool ExistsByLoanListId(int loanListId);
}