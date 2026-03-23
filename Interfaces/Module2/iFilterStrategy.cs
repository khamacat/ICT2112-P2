namespace ProRental.Interfaces;

using ProRental.Domain.Entities;

/// <summary>
/// Strategy interface for filtering transaction logs.
/// Each implementation filters by a different criteria.
/// Used by TransactionFilterControl.
/// </summary>
public interface IFilterStrategy
{
    bool validate();
    List<Transactionlog> filter(List<Transactionlog> logs);
}