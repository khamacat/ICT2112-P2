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
        string? type, string? supplier, string? product,
        DateTime? start, DateTime? end)
    {
        var now       = DateTime.UtcNow.AddHours(8);
        var startDate = start.HasValue ? start.Value.Date.ToUniversalTime() : DateTime.MinValue;
        var endDate   = end.HasValue   ? end.Value.Date.AddDays(1).AddTicks(-1).ToUniversalTime() : DateTime.MaxValue;

        IEnumerable<ProRental.Domain.Entities.Analytic> analytics;

        if (!string.IsNullOrWhiteSpace(supplier))
            analytics = await _analyticsControl.GetAnalyticsBySupplierAsync(supplier);
        else if (!string.IsNullOrWhiteSpace(product))
            analytics = await _analyticsControl.GetAnalyticsByProductAsync(product);
        else if (start.HasValue || end.HasValue)
            analytics = await _analyticsControl.GetAnalyticsByDateRangeAsync(startDate, endDate);
        else
            analytics = await _analyticsControl.GetAllAnalyticsAsync();

        if (!string.IsNullOrWhiteSpace(type) && type != "ALL")
            analytics = analytics.Where(a => a.GetAnalyticsType() == type);

        var vm = new AnalyticsIndexViewModel
        {
            Analytics      = analytics,
            FilterType     = type,
            FilterSupplier = supplier,
            FilterProduct  = product,
            FilterStart    = start ?? now,
            FilterEnd      = end   ?? now,
        };

        return View(IndexView, vm);
    }

    // ── Details ───────────────────────────────────────────────────────────────

    public async Task<IActionResult> Details(int id)
    {
        var analytic = await _analyticsControl.GetAnalyticsAsync(id);
        if (analytic is null) return NotFound();

        var logs         = await _analyticsControl.GetLogsForAnalyticsAsync(analytic);
        var allReports   = await _reportControl.GetAllReportsAsync();
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
        // Convert SGT input dates to UTC for storage
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

    // ── Export — transaction log data as CSV ──────────────────────────────────

    public async Task<IActionResult> ExportReport(int id)
    {
        var report = await _reportControl.GetReportAsync(id);
        if (report is null) return NotFound();

        var analyticsID = report.GetRefAnalyticsID();
        if (analyticsID is null) return NotFound();

        var analytic = await _analyticsControl.GetAnalyticsAsync(analyticsID.Value);
        if (analytic is null) return NotFound();

        var logs = (await _analyticsControl.GetLogsForAnalyticsAsync(analytic)).ToList();

        var format = report.GetFileFormat();

        if (format == FileFormat.PDF)
        {
            // Placeholder PDF text — replace with real PDF library when needed
            var content = $"Report: {report.GetTitle()}\n" +
                          $"Analytics ID: {analyticsID}\n" +
                          $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}\n\n" +
                          "Log ID,Type,Date,Supplier,Product,Summary\n" +
                          string.Join("\n", logs.Select(l =>
                              $"{l.LogID},{l.LogType},{l.CreatedAt.AddHours(8):yyyy-MM-dd HH:mm}," +
                              $"{l.SupplierName ?? "-"},{l.ProductName ?? "-"},{l.Summary}"));
            return File(Encoding.UTF8.GetBytes(content), "application/pdf",
                $"{report.GetTitle()}.pdf");
        }
        else
        {
            // CSV export with transaction log data
            var sb = new StringBuilder();
            sb.AppendLine("Log ID,Type,Date,Supplier,Product,Summary");
            foreach (var log in logs)
            {
                sb.AppendLine(
                    $"{log.LogID}," +
                    $"{log.LogType}," +
                    $"{log.CreatedAt.AddHours(8):yyyy-MM-dd HH:mm}," +
                    $"\"{log.SupplierName ?? "-"}\"," +
                    $"\"{log.ProductName ?? "-"}\"," +
                    $"\"{log.Summary}\"");
            }

            var contentType = format == FileFormat.XLSX
                ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                : "text/csv";
            var ext = format == FileFormat.XLSX ? "xlsx" : "csv";

            return File(Encoding.UTF8.GetBytes(sb.ToString()),
                contentType, $"{report.GetTitle()}.{ext}");
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