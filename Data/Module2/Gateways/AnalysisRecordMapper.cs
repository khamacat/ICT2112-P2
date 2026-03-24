using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces;

namespace ProRental.Data.Gateways;

public class AnalysisRecordMapper : IAnalyticsMapper
{
    private readonly AppDbContext _db;
    public AnalysisRecordMapper(AppDbContext db) => _db = db;

    public async Task<bool> InsertAsync(Analytic analytics)
    {
        _db.Analytics.Add(analytics);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<Analytic?> FindByIDAsync(int id)
        => await _db.Analytics.FindAsync(id);

    public async Task<IEnumerable<Analytic>> FindAllAsync()
        => await _db.Analytics.ToListAsync();

    public async Task<IEnumerable<Analytic>> FindByDateAsync(DateTime start, DateTime end)
        // Overlap logic: record overlaps filter if its start <= filterEnd AND its end >= filterStart
        => (await _db.Analytics.ToListAsync())
            .Where(a => a.GetStartDate() <= end && a.GetEndDate() >= start);

    public async Task<IEnumerable<Analytic>> FindBySupplierAsync(int supplierID)
        => (await _db.Analytics.ToListAsync())
            .Where(a => a.GetAnalyticsType() == AnalyticsType.SUPTREND.ToString()
                     && a.GetRefPrimaryID() == supplierID);

    public async Task<IEnumerable<Analytic>> FindByProductAsync(int productID)
        => (await _db.Analytics.ToListAsync())
            .Where(a => a.GetAnalyticsType() == AnalyticsType.PRODTREND.ToString()
                     && a.GetRefPrimaryID() == productID);

    // Name-based search across all analytics
    public async Task<IEnumerable<Analytic>> FindByNameAsync(string name)
        => (await _db.Analytics.ToListAsync())
            .Where(a => a.GetRefPrimaryName()
                         ?.Contains(name, StringComparison.OrdinalIgnoreCase) == true);

    public async Task<bool> UpdateAsync(Analytic analytics)
    {
        _db.Analytics.Update(analytics);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Analytic analytics)
    {
        _db.Analytics.Remove(analytics);
        return await _db.SaveChangesAsync() > 0;
    }
}