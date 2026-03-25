namespace ProRental.Domain.Module2.P2_2.Strategy;

using ProRental.Domain.Entities;
using ProRental.Interfaces;

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
            if (log.Purchaseorderlog != null && log.Purchaseorderlog.supplier_id == supplierIdInt)
                return true;
            return false;
        }).ToList();
    }
}