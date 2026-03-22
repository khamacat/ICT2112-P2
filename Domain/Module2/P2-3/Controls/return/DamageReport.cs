using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class DamageReportControl : iDamageReportCRUD, iDamageReportQuery
{
    private readonly IDamageReportMapper _damageReportMapper;
    private readonly IReturnItemMapper _returnItemMapper;

    public DamageReportControl(
        IDamageReportMapper damageReportMapper,
        IReturnItemMapper returnItemMapper)
    {
        _damageReportMapper = damageReportMapper ?? throw new ArgumentNullException(nameof(damageReportMapper));
        _returnItemMapper   = returnItemMapper   ?? throw new ArgumentNullException(nameof(returnItemMapper));
    }

    // -- iDamageReportCRUD ------------------------------------------------

    public bool SubmitDamageReport(int returnItemId, Damagereport damageReport)
    {
        if (damageReport is null)
        {
            return false;
        }

        try
        {
            var existing = _damageReportMapper.FindByReturnItemId(returnItemId);
            if (existing is null)
            {
                _damageReportMapper.Insert(damageReport);
            }
            else
            {
                existing.SetDescription(damageReport.GetDescription() ?? string.Empty);
                existing.SetSeverity(damageReport.GetSeverity() ?? string.Empty);
                existing.SetRepairCost(damageReport.GetRepairCost());
                existing.SetImages(damageReport.GetImages() ?? string.Empty);
                existing.SetReportDate(damageReport.GetReportDate());
                _damageReportMapper.Update(existing);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    // -- iDamageReportQuery -----------------------------------------------

    public Damagereport? GetDamageReportByReturnItem(int returnItemId)
    {
        return _damageReportMapper.FindByReturnItemId(returnItemId);
    }

    public List<Damagereport> GetDamageReportsByReturnRequest(int returnRequestId)
    {
        var items = _returnItemMapper.FindByReturnRequest(returnRequestId);
        if (items is null)
        {
            return new List<Damagereport>();
        }

        var reports = new List<Damagereport>();
        foreach (var item in items)
        {
            var report = _damageReportMapper.FindByReturnItemId(item.GetReturnItemId());
            if (report != null)
            {
                reports.Add(report);
            }
        }
        return reports;
    }

    public bool ValidateDamageReport(Damagereport damageReport)
    {
        if (damageReport is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(damageReport.GetDescription()))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(damageReport.GetSeverity()))
        {
            return false;
        }

        return true;
    }

    public byte[] DownloadDamageReport(int damageReportId)
    {
        return Array.Empty<byte>();
    }
}