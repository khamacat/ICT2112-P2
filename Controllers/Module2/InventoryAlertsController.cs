using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Controllers.Module2;

[Route("module2/[controller]")]
public class InventoryAlertsController : Controller
{
    private readonly AppDbContext _dbContext;

    public InventoryAlertsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("DisplayAllThresholds")]
    public async Task<IActionResult> DisplayAllThresholds()
    {
        var alerts = await _dbContext.Alerts
            .AsNoTracking()
            .OrderBy(a => EF.Property<int>(a, "Productid"))
            .ThenBy(a => EF.Property<int>(a, "Alertid"))
            .ToListAsync();

        ViewData["Filter"] = "All Thresholds";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpGet("DisplayThreshold/{threshold:int}")]
    public async Task<IActionResult> DisplayThreshold(int threshold)
    {
        var alerts = await _dbContext.Alerts
            .AsNoTracking()
            .Where(a => EF.Property<int>(a, "Minthreshold") == threshold)
            .OrderBy(a => EF.Property<int>(a, "Alertid"))
            .ToListAsync();

        ViewData["Filter"] = $"Threshold = {threshold}";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpPost("EditThreshold/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditThreshold(int alertId, int threshold)
    {
        var alert = await _dbContext.Alerts.FirstOrDefaultAsync(a => EF.Property<int>(a, "Alertid") == alertId);
        if (alert is null)
        {
            return NotFound();
        }

        alert.SetMinThreshold(threshold);
        await _dbContext.SaveChangesAsync();

        TempData["Message"] = $"Threshold updated for alert #{alertId}.";
        return RedirectToAction(nameof(DisplayAllAlerts));
    }

    [HttpGet("DisplayAllAlerts")]
    public async Task<IActionResult> DisplayAllAlerts()
    {
        var alerts = await _dbContext.Alerts
            .AsNoTracking()
            .OrderByDescending(a => EF.Property<DateTime>(a, "Createdat"))
            .ToListAsync();

        ViewData["Filter"] = "All Alerts";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpGet("DisplayAlert/{alertId:int}")]
    public async Task<IActionResult> DisplayAlert(int alertId)
    {
        var alerts = await _dbContext.Alerts
            .AsNoTracking()
            .Where(a => EF.Property<int>(a, "Alertid") == alertId)
            .ToListAsync();

        ViewData["Filter"] = $"Alert #{alertId}";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpPost("AcknowledgeAlert/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcknowledgeAlert(int alertId)
    {
        var alert = await _dbContext.Alerts.FirstOrDefaultAsync(a => EF.Property<int>(a, "Alertid") == alertId);
        if (alert is null)
        {
            return NotFound();
        }

        alert.SetAlertStatus(AlertStatus.ACKNOWLEDGED);
        await _dbContext.SaveChangesAsync();

        TempData["Message"] = $"Alert #{alertId} acknowledged.";
        return RedirectToAction(nameof(DisplayAllAlerts));
    }

    [HttpPost("ResolveAlert/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveAlert(int alertId)
    {
        var alert = await _dbContext.Alerts.FirstOrDefaultAsync(a => EF.Property<int>(a, "Alertid") == alertId);
        if (alert is null)
        {
            return NotFound();
        }

        alert.SetAlertStatus(AlertStatus.RESOLVED);
        alert.SetResolvedAt(DateTime.UtcNow);
        await _dbContext.SaveChangesAsync();

        TempData["Message"] = $"Alert #{alertId} resolved.";
        return RedirectToAction(nameof(DisplayAllAlerts));
    }
}
