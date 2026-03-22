using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces;

namespace ProRental.Domain.Control;

/// <summary>
/// Factory Pattern — centralises creation of Analytics product types.
/// All three types (DailyLog, SupplierTrend, ProductTrend) are the same
/// Analytic entity differentiated by AnalyticsType enum.
/// </summary>
public class AnalyticsFactory
{
    public IAnalytics CreateDailyLog()
    {
        var entity = new Analytic();
        entity.UpdateType(AnalyticsType.DAILY);
        return new DailyLogAnalytics(entity);
    }

    public IAnalytics CreateSupplierTrend()
    {
        var entity = new Analytic();
        entity.UpdateType(AnalyticsType.SUPTREND);
        return new SupplierTrendAnalytics(entity);
    }

    public IAnalytics CreateProductTrend()
    {
        var entity = new Analytic();
        entity.UpdateType(AnalyticsType.PRODTREND);
        return new ProductTrendAnalytics(entity);
    }
}

// ── Wrappers — all wrap Analytic, differentiated by type ─────────────────────

internal class DailyLogAnalytics : IAnalytics
{
    private readonly Analytic _entity;
    public DailyLogAnalytics(Analytic entity) => _entity = entity;
    public string GetAnalyticsType() => _entity.GetAnalyticsType();
    public int GetID()               => _entity.GetID();
}

internal class SupplierTrendAnalytics : IAnalytics
{
    private readonly Analytic _entity;                        // Analytic, not SupplierTrend
    public SupplierTrendAnalytics(Analytic entity) => _entity = entity;
    public string GetAnalyticsType() => _entity.GetAnalyticsType();
    public int GetID()               => _entity.GetID();
}

internal class ProductTrendAnalytics : IAnalytics
{
    private readonly Analytic _entity;                        // Analytic, not ProductTrend
    public ProductTrendAnalytics(Analytic entity) => _entity = entity;
    public string GetAnalyticsType() => _entity.GetAnalyticsType();
    public int GetID()               => _entity.GetID();
}