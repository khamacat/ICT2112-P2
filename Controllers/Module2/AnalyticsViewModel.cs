using ProRental.Domain.Entities;
using ProRental.Interfaces;

namespace ProRental.Controllers;

public class AnalyticsIndexViewModel
{
    public IEnumerable<Analytic> Analytics { get; set; } = [];
    public string? FilterType    { get; set; }
    public string? FilterSearch  { get; set; }   // replaces FilterSupplier + FilterProduct
    public DateTime? FilterStart { get; set; }
    public DateTime? FilterEnd   { get; set; }
}

public class AnalyticsDetailsViewModel
{
    public Analytic Analytic { get; set; } = null!;
    public IEnumerable<TransactionLogDto> TransactionLogs { get; set; } = [];
    public Reportexport? ExistingReport { get; set; }
}