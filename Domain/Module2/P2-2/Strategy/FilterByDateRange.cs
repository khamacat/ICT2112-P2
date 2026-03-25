namespace ProRental.Domain.Module2.P2_2.Strategy;

using ProRental.Domain.Entities;
using ProRental.Interfaces;

public class FilterByDateRange : IFilterStrategy
{
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;

    public FilterByDateRange(DateTime startDate, DateTime endDate)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    public FilterByDateRange(string dateRange)
    {
        var parts = dateRange.Split('|');
        _startDate = DateTime.TryParse(parts.ElementAtOrDefault(0), out var s) ? s : DateTime.MinValue;
        _endDate = DateTime.TryParse(parts.ElementAtOrDefault(1), out var e) ? e : DateTime.MaxValue;
    }

    public bool validate()
    {
        return _startDate <= _endDate && _startDate != DateTime.MinValue;
    }

    public List<Transactionlog> filter(List<Transactionlog> logs)
    {
        return logs.Where(log =>
        {
            if (log.created_at == null) return false;
            return log.created_at >= _startDate && log.created_at <= _endDate;
        }).ToList();
    }
}