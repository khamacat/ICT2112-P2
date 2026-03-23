namespace ProRental.Domain.Module2.P2_2.Strategy;

using ProRental.Domain.Entities;
using ProRental.Interfaces;

/// <summary>
/// Filters transaction logs by Supplier ID.
/// Matches against: PurchaseOrderLog.SupplierId.
/// </summary>
public class FilterBySupplierId : IFilterStrategy
{
    private readonly string _supplierId;

    public FilterBySupplierId(string supplierId)
    {
        _supplierId = supplierId;
    }

    public bool validate()
    {
        return !string.IsNullOrWhiteSpace(_supplierId)
            && int.TryParse(_supplierId, out _);
    }

    public List<Transactionlog> filter(List<Transactionlog> logs)
    {
        int supplierIdInt = int.Parse(_supplierId);

        return logs.Where(log =>
        {
            if (log.Purchaseorderlog != null && log.Purchaseorderlog.supplierid == supplierIdInt)
                return true;

            return false;
        }).ToList();
    }
}