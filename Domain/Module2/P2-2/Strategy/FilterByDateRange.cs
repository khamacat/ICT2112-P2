namespace ProRental.Domain.Module2.P2_2.Strategy;

using ProRental.Domain.Entities;
using ProRental.Interfaces;

/// <summary>
/// Filters transaction logs by date range.
/// Matches against: TransactionLog.CreatedAt.
/// </summary>
public class FilterByDateRange : IFilterStrategy
{
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;

    public FilterByDateRange(DateTime startDate, DateTime endDate)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    /// <summary>
    /// Constructor that parses a pipe-delimited string "startDate|endDate".
    /// Used when the filter value comes from the UI as a single string.
    /// </summary>
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
            if (log.createdat == null) return false;
            return log.createdat >= _startDate && log.createdat <= _endDate;
        }).ToList();
    }
}