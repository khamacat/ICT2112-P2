using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces;

namespace ProRental.Domain.Control;

public class AnalyticsControl : IAnalyticsData, ITrend
{
    private readonly IAnalyticsMapper _analyticsMapper;
    private readonly AnalyticsFactory _factory;
    private readonly ITransactionLogService _transactionLogService;
    private static readonly Random _rng = new();

    public AnalyticsControl(
        IAnalyticsMapper analyticsMapper,
        AnalyticsFactory factory,
        ITransactionLogService transactionLogService)
    {
        _analyticsMapper       = analyticsMapper;
        _factory               = factory;
        _transactionLogService = transactionLogService;
    }

    // ── Create ───────────────────────────────────────────────────────────────

    public async Task<Analytic> CreateAnalyticsAsync(
        string analyticsType, DateTime startDate, DateTime endDate,
        string refPrimaryName, int? refPrimaryID = null)
    {
        var logs      = await _transactionLogService.GetTransactionLogsByDateRangeAsync(startDate, endDate);
        var logList   = logs.ToList();
        int loanAmt   = logList.Count(l => l.LogType == "LOAN");
        int returnAmt = logList.Count(l => l.LogType == "RETURN");

        // Generate ref value based on type
        decimal refValue = analyticsType switch
        {
            "SUPTREND"  => Math.Round((decimal)(_rng.NextDouble() * 20 + 80), 2), // 80.00–100.00
            "PRODTREND" => Math.Round((decimal)(_rng.NextDouble() * 3.8 + 0.2), 2), // 0.20–4.00
            _           => 0m
        };

        var entity = new Analytic();
        entity.UpdateType(analyticsType switch
        {
            "SUPTREND"  => AnalyticsType.SUPTREND,
            "PRODTREND" => AnalyticsType.PRODTREND,
            _           => AnalyticsType.DAILY
        });
        entity.SetStartDate(startDate);
        entity.SetEndDate(endDate);
        entity.SetLoanAmt(loanAmt);
        entity.SetReturnAmt(returnAmt);
        entity.SetRefPrimaryName(refPrimaryName);
        entity.SetRefPrimaryID(refPrimaryID);
        entity.SetRefValue(refValue);

        await _analyticsMapper.InsertAsync(entity);
        return entity;
    }

    // ── Read ─────────────────────────────────────────────────────────────────

    public async Task<Analytic?> GetAnalyticsAsync(int targetID)
        => await _analyticsMapper.FindByIDAsync(targetID);

    public async Task<IEnumerable<Analytic>> GetAllAnalyticsAsync()
        => await _analyticsMapper.FindAllAsync();

    public async Task<IEnumerable<Analytic>> GetAnalyticsByDateAsync(DateTime day)
        => await _analyticsMapper.FindByDateAsync(day.Date, day.Date.AddDays(1).AddTicks(-1));

    public async Task<IEnumerable<Analytic>> GetAnalyticsByDateRangeAsync(DateTime start, DateTime end)
        => await _analyticsMapper.FindByDateAsync(start, end);

    public async Task<IEnumerable<Analytic>> GetAnalyticsByNameAsync(string name)
        => await _analyticsMapper.FindByNameAsync(name);

    public async Task<IEnumerable<Analytic>> GetAnalyticsBySupplierAsync(string supplier)
        => await _analyticsMapper.FindByNameAsync(supplier);

    public async Task<IEnumerable<Analytic>> GetAnalyticsByProductAsync(string product)
        => await _analyticsMapper.FindByNameAsync(product);

    // ── Transaction Logs ─────────────────────────────────────────────────────

    public async Task<IEnumerable<TransactionLogDto>> GetLogsForAnalyticsAsync(Analytic analytic)
    {
        var start = analytic.GetStartDate() ?? DateTime.MinValue;
        var end   = analytic.GetEndDate()   ?? DateTime.MaxValue;
        return await _transactionLogService.GetTransactionLogsByDateRangeAsync(start, end);
    }

    public async Task<IEnumerable<TransactionLogDto>> GetAllLogsAsync()
        => await _transactionLogService.GetAllTransactionLogsAsync();

    // ── Trend (ITrend) ──────────────────────────────────────────────────────

    // Returns the supplier reliability score (RefValue) for the most recent
    // SUPTREND analytics record matching the given supplier ID.
    public async Task<float?> GetSupplierReliabilityAsync(int targetID)
    {
        var records = await _analyticsMapper.FindBySupplierAsync(targetID);
        var latest  = records
            .Where(a => a.GetAnalyticsType() == AnalyticsType.SUPTREND.ToString())
            .OrderByDescending(a => a.GetEndDate())
            .FirstOrDefault();
        return latest is null ? null : (float?)latest.GetRefValue();
    }

    // Returns the product turnover rate (RefValue) for the most recent
    // PRODTREND analytics record matching the given product ID.
    public async Task<float?> GetTurnoverRateAsync(int targetID)
    {
        var records = await _analyticsMapper.FindByProductAsync(targetID);
        var latest  = records
            .Where(a => a.GetAnalyticsType() == AnalyticsType.PRODTREND.ToString())
            .OrderByDescending(a => a.GetEndDate())
            .FirstOrDefault();
        return latest is null ? null : (float?)latest.GetRefValue();
    }

    // ── Update / Delete ───────────────────────────────────────────────────────

    public async Task UpdateAnalyticsAsync(int targetID, List<object> transactions)
    {
        var existing = await _analyticsMapper.FindByIDAsync(targetID);
        if (existing is null) return;
        await _analyticsMapper.UpdateAsync(existing);
    }

    public async Task DeleteAnalyticsAsync(int targetID)
    {
        var existing = await _analyticsMapper.FindByIDAsync(targetID);
        if (existing is null) return;
        await _analyticsMapper.DeleteAsync(existing);
    }
}