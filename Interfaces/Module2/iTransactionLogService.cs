namespace ProRental.Interfaces;

using ProRental.Domain.Entities;

public interface ITransactionLogService
{
    List<Transactionlog> GetAllLogs();
}