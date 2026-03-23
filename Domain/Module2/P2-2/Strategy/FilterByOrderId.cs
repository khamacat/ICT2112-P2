namespace ProRental.Domain.Module2.P2_2.Strategy;

using ProRental.Domain.Entities;
using ProRental.Interfaces;

/// <summary>
/// Filters transaction logs by Order ID.
/// Matches against: RentalOrderLog.OrderId.
/// </summary>
public class FilterByOrderId : IFilterStrategy
{
    private readonly string _orderId;

    public FilterByOrderId(string orderId)
    {
        _orderId = orderId;
    }

    public bool validate()
    {
        return !string.IsNullOrWhiteSpace(_orderId)
            && int.TryParse(_orderId, out _);
    }

    public List<Transactionlog> filter(List<Transactionlog> logs)
    {
        int orderIdInt = int.Parse(_orderId);

        return logs.Where(log =>
        {
            if (log.Rentalorderlog != null && log.Rentalorderlog.orderid == orderIdInt)
                return true;

            return false;
        }).ToList();
    }
}