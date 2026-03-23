using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

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
        // Redirect to product-based view since thresholds are now managed at the product level
        return RedirectToAction(nameof(DisplayAlertsByProduct));
    }

    [HttpGet("DisplayThreshold/{threshold:int}")]
    public IActionResult DisplayThreshold(int threshold)
    {
        // This functionality is replaced by product-based and staff-based filtering
        TempData["Message"] = "Use DisplayAlertsByProduct or DisplayAlertsByStaff filters instead.";
        return RedirectToAction(nameof(DisplayAlerts));
    }

    [HttpPost("EditThreshold/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult EditThreshold(int alertId, int threshold)
    {
        // Thresholds are now managed through product configuration via IProductStatusControl
        TempData["Message"] = "Threshold management has been moved to product configuration.";
        return RedirectToAction(nameof(DisplayAlerts));
    }

    [HttpGet("DisplayAllAlerts")]
    public IActionResult DisplayAllAlerts()
    {
        return RedirectToAction(nameof(DisplayAlerts));
    }

    [HttpGet("DisplayAlerts")]
    public IActionResult DisplayAlerts()
    {
        var alerts = _alertControl.GetAllAlerts();
        ViewData["Filter"] = "Select a filter below:";
        ViewData["Message"] = "Use DisplayAlertsByProduct or DisplayAlertsByStaff with appropriate IDs";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpGet("DisplayAlertsByProduct/{productId:int}")]
    public IActionResult DisplayAlertsByProduct(int productId)
    {
        var alerts = _alertControl.GetAlertsByProduct(productId);
        ViewData["Filter"] = $"Alerts for Product #{productId}";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpGet("DisplayAlertsByStaff/{staffId:int}")]
    public IActionResult DisplayAlertsByStaff(int staffId)
    {
        var alerts = _alertControl.GetAlertsByStaff(staffId);
        ViewData["Filter"] = $"Alerts for Staff #{staffId}";
        return View("~/Views/Module2/Alerts.cshtml", alerts);
    }

    [HttpGet("DisplayAlert/{alertId:int}")]
    public IActionResult DisplayAlert(int alertId)
    {
        // GetAlertById has been removed from the interface
        // Use GetAlertsByProduct or GetAlertsByStaff instead
        TempData["Message"] = "Use DisplayAlertsByProduct or DisplayAlertsByStaff to view alerts.";
        return RedirectToAction(nameof(DisplayAlerts));
    }

    [HttpPost("SendAlertToStaff/{alertId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult SendAlertToStaff(int alertId, int staffId)
    {
        if (staffId <= 0)
        {
            TempData["Message"] = "Invalid staff ID.";
            return RedirectToAction(nameof(DisplayAlerts));
        }

        if (_alertControl.SendAlertToStaff(alertId, staffId))
        {
            TempData["Message"] = $"Alert #{alertId} assigned to Staff #{staffId}.";
        }
        else
        {
            TempData["Message"] = "Alert not found or could not be assigned.";
        }
        return RedirectToAction(nameof(DisplayAlerts));
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
