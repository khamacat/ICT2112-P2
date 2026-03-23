namespace ProRental.Domain.Module2.P2_2.Strategy;

using ProRental.Domain.Entities;
using ProRental.Interfaces;

/// <summary>
/// Filters transaction logs by Customer ID.
/// Matches against: RentalOrderLog.CustomerId, ReturnLog.CustomerId.
/// </summary>
public class FilterByCustomerId : IFilterStrategy
{
    private readonly string _customerId;

    public FilterByCustomerId(string customerId)
    {
        _customerId = customerId;
    }

    public bool validate()
    {
        return !string.IsNullOrWhiteSpace(_customerId)
            && int.TryParse(_customerId, out _);
    }

    public List<Transactionlog> filter(List<Transactionlog> logs)
    {
        int customerIdInt = int.Parse(_customerId);

        return logs.Where(log =>
        {
            if (log.Rentalorderlog != null && log.Rentalorderlog.customerid == customerIdInt)
                return true;

            if (log.Returnlog != null && log.Returnlog.customerid == _customerId)
                return true;

            return false;
        }).ToList();
    }
}