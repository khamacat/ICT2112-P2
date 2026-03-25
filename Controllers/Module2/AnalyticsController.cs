using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Control;
using ProRental.Domain.Enums;
using ProRental.Interfaces;
using System.Text;

namespace ProRental.Controllers;

public class AnalyticsController : Controller
{
    private readonly AnalyticsControl    _analyticsControl;
    private readonly ReportExportControl _reportControl;

    private const string IndexView  = "~/Views/Module2/Analytics/Index.cshtml";
    private const string DetailView = "~/Views/Module2/Analytics/Details.cshtml";
    private const string ReportView = "~/Views/Module2/Analytics/Report.cshtml";
    private const string CreateView = "~/Views/Module2/Analytics/Create.cshtml";

    public AnalyticsController(AnalyticsControl analyticsControl, ReportExportControl reportControl)
    {
        _analyticsControl = analyticsControl;
        _reportControl    = reportControl;
    }

    // ── Index ─────────────────────────────────────────────────────────────────

    public async Task<IActionResult> Index(
        string? type, string? search, DateTime? start, DateTime? end)
    {
        IEnumerable<ProRental.Domain.Entities.Analytic> analytics;

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Name search across all records
            analytics = await _analyticsControl.GetAnalyticsByNameAsync(search);
        }
        else if (start.HasValue || end.HasValue)
        {
            // Date overlap filter — convert SGT input to UTC
            var startUtc = start.HasValue
                ? start.Value.Date.ToUniversalTime()
                : DateTime.MinValue;
            var endUtc = end.HasValue
                ? end.Value.Date.AddDays(1).AddTicks(-1).ToUniversalTime()
                : DateTime.MaxValue;
            analytics = await _analyticsControl.GetAnalyticsByDateRangeAsync(startUtc, endUtc);
        }
        else
        {
            analytics = await _analyticsControl.GetAllAnalyticsAsync();
        }

        // Type filter applied after fetch (since type is stored in unmapped _analyticsType)
        if (!string.IsNullOrWhiteSpace(type) && type != "ALL")
            analytics = analytics.Where(a => a.GetAnalyticsType() == type);

        var now = DateTime.UtcNow.AddHours(8);
        var vm = new AnalyticsIndexViewModel
        {
            Analytics   = analytics,
            FilterType  = type,
            FilterSearch = search,
            FilterStart = start ?? now,
            FilterEnd   = end   ?? now,
        };

        return View(IndexView, vm);
    }

    // ── Details ───────────────────────────────────────────────────────────────

    public async Task<IActionResult> Details(int id)
    {
        var analytic = await _analyticsControl.GetAnalyticsAsync(id);
        if (analytic is null) return NotFound();

        var logs           = await _analyticsControl.GetLogsForAnalyticsAsync(analytic);
        var allReports     = await _reportControl.GetAllReportsAsync();
        var existingReport = allReports.FirstOrDefault(r => r.GetRefAnalyticsID() == id);

        var vm = new AnalyticsDetailsViewModel
        {
            Analytic        = analytic,
            TransactionLogs = logs,
            ExistingReport  = existingReport
        };

        return View(DetailView, vm);
    }

    // ── Create ────────────────────────────────────────────────────────────────

    public async Task<IActionResult> Create()
    {
        var logs = await _analyticsControl.GetAllLogsAsync();
        return View(CreateView, logs);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        string analyticsType, DateTime startDate, DateTime endDate, string refPrimaryName)
    {
        var start = startDate.ToUniversalTime();
        var end   = endDate.Date.AddDays(1).AddTicks(-1).ToUniversalTime();

        var analytic = await _analyticsControl.CreateAnalyticsAsync(
            analyticsType, start, end, refPrimaryName);

        return RedirectToAction(nameof(Details), new { id = analytic.GetID() });
    }

    // ── Report actions ────────────────────────────────────────────────────────

    [HttpPost]
    public async Task<IActionResult> GenerateReport(
        int refAnalyticsID, string title, VisualType visualType, FileFormat fileFormat)
    {
        await _reportControl.GenerateReportAsync(refAnalyticsID, title, visualType, fileFormat);
        return RedirectToAction(nameof(Details), new { id = refAnalyticsID });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateReport(
        int id, int refAnalyticsID, string title, VisualType visualType, FileFormat fileFormat)
    {
        await _reportControl.UpdateReportAsync(id, title, visualType, fileFormat);
        return RedirectToAction(nameof(Details), new { id = refAnalyticsID });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteReport(int id, int refAnalyticsID)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is not null) await _reportControl.DeleteReportAsync(report);
        return RedirectToAction(nameof(Details), new { id = refAnalyticsID });
    }

    // ── Export ────────────────────────────────────────────────────────────────
    // Only CSV is supported for demo. XLSX/PNG require external libraries.
    // PDF uses inline display via iframe, not download.

    public async Task<IActionResult> ExportReport(int id, bool download = false)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is null) return NotFound();

        var analyticsID = report.GetRefAnalyticsID();
        if (analyticsID is null) return NotFound();

        var analytic = await _analyticsControl.GetAnalyticsAsync(analyticsID.Value);
        if (analytic is null) return NotFound();

        var logs   = (await _analyticsControl.GetLogsForAnalyticsAsync(analytic)).ToList();
        var format = report.GetFileFormat();
        var title  = report.GetTitle() ?? "report";

        // Build CSV content (used for both CSV and as fallback for unsupported formats)
        var sb = new StringBuilder();
        sb.AppendLine("Log ID,Type,Date,Supplier,Product,Summary");
        foreach (var log in logs)
            sb.AppendLine(
                $"{log.LogID}," +
                $"{log.LogType}," +
                $"{log.CreatedAt.AddHours(8):yyyy-MM-dd HH:mm}," +
                $"\"{log.SupplierName ?? "-"}\"," +
                $"\"{(log.ProductNames.Any() ? string.Join("; ", log.ProductNames) : "-")}\"," +
                $"\"{log.Summary}\"");

        var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());

        if (format == FileFormat.PDF || format == FileFormat.PNG)
        {
            if (download)
                return File(csvBytes, format == FileFormat.PNG ? "image/png" : "application/pdf",
                    $"{title}.{format.ToString().ToLower()}");
            // Inline for iframe
            return Content(sb.ToString(), "text/plain");
        }
        else if (format == FileFormat.CSV)
        {
            return File(csvBytes, "text/csv", $"{title}.csv");
        }
        else
        {
            // XLSX and PNG not supported without external libraries
            // Return CSV with a note for demo purposes
            TempData["ExportNote"] = $"{format} export is not supported in demo mode. Downloaded as CSV instead.";
            return File(csvBytes, "text/csv", $"{title}.csv");
        }
    }

    // ── Report standalone page ────────────────────────────────────────────────

    public async Task<IActionResult> Report(int id)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is null) return NotFound();
        return View(ReportView, report);
    }
}