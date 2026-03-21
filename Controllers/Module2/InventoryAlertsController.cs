using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Module2.P2_3;

namespace ProRental.Controllers.Module2;

[Route("module2/[controller]")]
public class InventoryAlertsController : Controller
{
    private readonly iAlertControl _alertControl;

    public InventoryAlertsController(iAlertControl alertControl)
    {
        _alertControl = alertControl;
    }

    [HttpGet("DisplayAllThresholds")]
    public IActionResult DisplayAllThresholds()
    {
        var alerts = _alertControl.GetAllAlerts();
        ViewData["Filter"] = "All Thresholds";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpGet("DisplayThreshold/{threshold:int}")]
    public IActionResult DisplayThreshold(int threshold)
    {
        var alerts = _alertControl.GetAlertsByThreshold(threshold);
        ViewData["Filter"] = $"Threshold = {threshold}";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpPost("EditThreshold/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult EditThreshold(int alertId, int threshold)
    {
        if (_alertControl.UpdateAlertThreshold(alertId, threshold))
        {
            TempData["Message"] = $"Threshold updated for alert #{alertId}.";
        }
        else
        {
            TempData["Message"] = "Alert not found or could not be updated.";
        }
        return RedirectToAction(nameof(DisplayAllAlerts));
    }

    [HttpGet("DisplayAllAlerts")]
    public IActionResult DisplayAllAlerts()
    {
        var alerts = _alertControl.GetAllAlerts();
        ViewData["Filter"] = "All Alerts";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpGet("DisplayAlert/{alertId:int}")]
    public IActionResult DisplayAlert(int alertId)
    {
        var alert = _alertControl.GetAlertById(alertId);
        if (alert is null)
        {
            return NotFound();
        }

        var alerts = new List<Alert> { alert };
        ViewData["Filter"] = $"Alert #{alertId}";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpPost("AcknowledgeAlert/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult AcknowledgeAlert(int alertId)
    {
        if (_alertControl.UpdateAlertStatus(alertId, AlertStatus.ACKNOWLEDGED))
        {
            TempData["Message"] = $"Alert #{alertId} acknowledged.";
        }
        else
        {
            TempData["Message"] = "Alert not found or could not be updated.";
        }
        return RedirectToAction(nameof(DisplayAllAlerts));
    }

    [HttpPost("ResolveAlert/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult ResolveAlert(int alertId)
    {
        if (_alertControl.ResolveAlert(alertId))
        {
            TempData["Message"] = $"Alert #{alertId} resolved.";
        }
        else
        {
            TempData["Message"] = "Alert not found or could not be resolved.";
        }
        return RedirectToAction(nameof(DisplayAllAlerts));
    }
}
