using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces;

namespace ProRental.Data.Gateways;

public class ReportMapper : IReportExportMapper
{
    private readonly AppDbContext _db;

    public ReportMapper(AppDbContext db) => _db = db;

    public async Task<bool> InsertAsync(Reportexport report)
    {
        _db.Reportexports.Add(report);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<Reportexport?> FindByIDAsync(int id)
        => await _db.Reportexports.FindAsync(id);

    public async Task<Reportexport?> FindByTitleAsync(string title)
        // Load into memory — EF cannot translate private property access
        => (await _db.Reportexports.ToListAsync())
            .FirstOrDefault(r => r.GetTitle() == title);

    public async Task<bool> UpdateAsync(Reportexport report)
    {
        _db.Reportexports.Update(report);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Reportexport report)
    {
        _db.Reportexports.Remove(report);
        return await _db.SaveChangesAsync() > 0;
    }
}