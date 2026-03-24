using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces;

namespace ProRental.Domain.Control;

public class AnalyticsControl : IAnalyticsData
{
    private readonly IAnalyticsMapper _analyticsMapper;
    private readonly AnalyticsFactory _factory;
    private readonly ITransactionLogService _transactionLogService;

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
        var logs    = await _transactionLogService.GetTransactionLogsByDateRangeAsync(startDate, endDate);
        var logList = logs.ToList();
        int loanAmt   = logList.Count(l => l.LogType == "LOAN");
        int returnAmt = logList.Count(l => l.LogType == "RETURN");

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
        entity.SetRefValue(0);

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

    // Unified name search — searches refprimaryname regardless of type
    public async Task<IEnumerable<Analytic>> GetAnalyticsByNameAsync(string name)
        => await _analyticsMapper.FindByNameAsync(name);

    // Keep old methods for interface compatibility
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