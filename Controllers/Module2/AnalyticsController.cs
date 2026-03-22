using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Control;
using ProRental.Domain.Enums;
using ProRental.Interfaces;

namespace ProRental.Controllers;

/// <summary>
/// Page Controller (Boundary) for the Analytics feature.
/// Delegates all business logic to AnalyticsControl and ReportExportControl.
/// Never contains business logic itself.
/// </summary>
public class AnalyticsController : Controller
{
    private readonly AnalyticsControl _analyticsControl;
    private readonly ReportExportControl _reportControl;

    public AnalyticsController(AnalyticsControl analyticsControl, ReportExportControl reportControl)
    {
        _analyticsControl = analyticsControl;
        _reportControl    = reportControl;
    }

    // ── Analytics Views ─────────────────────────────────────────────────────────

    /// <summary>Landing page — list all analytics records.</summary>
    public async Task<IActionResult> Index()
    {
        var analytics = await _analyticsControl.GetAnalyticsByDateRangeAsync(
            DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);
        return View("~/Views/Module2/Analytics/Index.cshtml", analytics);
    }

    public async Task<IActionResult> Details(int id)
    {
        var analytics = await _analyticsControl.GetAnalyticsAsync(id);
        if (analytics is null) return NotFound();
        return View("~/Views/Module2/Analytics/Details.cshtml", analytics);
    }

    public async Task<IActionResult> ByDateRange(DateTime start, DateTime end)
    {
        var analytics = await _analyticsControl.GetAnalyticsByDateRangeAsync(start, end);
        return View("~/Views/Module2/Analytics/Index.cshtml", analytics);
    }

    public async Task<IActionResult> BySupplier(string supplier)
    {
        var analytics = await _analyticsControl.GetAnalyticsBySupplierAsync(supplier);
        return View("~/Views/Module2/Analytics/Index.cshtml", analytics);
    }

    public async Task<IActionResult> ByProduct(string product)
    {
        var analytics = await _analyticsControl.GetAnalyticsByProductAsync(product);
        return View("~/Views/Module2/Analytics/Index.cshtml", analytics);
    }
    // ── Report Views ─────────────────────────────────────────────────────────────

    /// <summary>Display a report export record.</summary>
    public async Task<IActionResult> Report(int id)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is null) return NotFound();
        return View("~/Views/Module2/Analytics/Report.cshtml", report);
    }

    public async Task<IActionResult> RenderReport(int id, string format)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is null) return NotFound();
        return View("~/Views/Module2/Analytics/Report.cshtml", report);
    }

    /// <summary>Export report as a file download.</summary>
    public async Task<IActionResult> ExportReport(int id, string format)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is null) return NotFound();

        // File served from stored URL via public partial class accessor
        return Redirect(report.GetFileURL() ?? "/");
    }

    // ── Report CRUD ──────────────────────────────────────────────────────────────

    [HttpPost]
    public async Task<IActionResult> GenerateReport(
        int refAnalyticsID, string title,
        VisualType? visualType, FileFormat? fileFormat)
    {
        await _reportControl.GenerateReportAsync(refAnalyticsID, title, visualType, fileFormat);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateReport(
        int id, string title, VisualType visualType, FileFormat fileFormat)
    {
        await _reportControl.UpdateReportAsync(id, title, visualType, fileFormat);
        return RedirectToAction(nameof(Report), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteReport(int id)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is not null)
            await _reportControl.DeleteReportAsync(report);
        return RedirectToAction(nameof(Index));
    }
}