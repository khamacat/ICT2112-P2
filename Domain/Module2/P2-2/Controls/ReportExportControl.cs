using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces;

namespace ProRental.Domain.Control;

public class ReportExportControl
{
    private readonly IReportExportMapper _reportMapper;

    public ReportExportControl(IReportExportMapper reportMapper)
        => _reportMapper = reportMapper;

    public async Task GenerateReportAsync(
        int refAnalyticsID, string title,
        VisualType? visualType = null, FileFormat? fileFormat = null)
    {
        var report = new Reportexport();
        report.SetRefAnalyticsID(refAnalyticsID);   // via partial class setter
        report.SetTitle(title);                      // via partial class setter
        report.UpdateType(visualType ?? VisualType.TABLE);
        report.UpdateFormat(fileFormat ?? FileFormat.PDF);
        await _reportMapper.InsertAsync(report);
    }

    public async Task<Reportexport?> GetReportAsync(int targetID)
        => await _reportMapper.FindByIDAsync(targetID);

    public async Task UpdateReportAsync(
        int targetID, string title, VisualType visualType, FileFormat fileFormat)
    {
        var report = await _reportMapper.FindByIDAsync(targetID);
        if (report is null) return;
        report.SetTitle(title);
        report.UpdateType(visualType);
        report.UpdateFormat(fileFormat);
        await _reportMapper.UpdateAsync(report);
    }

    public async Task DeleteReportAsync(Reportexport report)
        => await _reportMapper.DeleteAsync(report);
}