using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

[Route("module2/[controller]")]
public class ReturnProcessingController : Controller
{
    private readonly iReturnOrderQuery  _returnOrderQuery;
    private readonly iReturnOrderCRUD   _returnOrderCRUD;
    private readonly iReturnItemQuery   _returnItemQuery;
    private readonly iReturnItemCRUD    _returnItemCRUD;
    private readonly iDamageReportQuery _damageReportQuery;
    private readonly iDamageReportCRUD  _damageReportCRUD;

    public ReturnProcessingController(
        iReturnOrderQuery  returnOrderQuery,
        iReturnOrderCRUD   returnOrderCRUD,
        iReturnItemQuery   returnItemQuery,
        iReturnItemCRUD    returnItemCRUD,
        iDamageReportQuery damageReportQuery,
        iDamageReportCRUD  damageReportCRUD)
    {
        _returnOrderQuery  = returnOrderQuery;
        _returnOrderCRUD   = returnOrderCRUD;
        _returnItemQuery   = returnItemQuery;
        _returnItemCRUD    = returnItemCRUD;
        _damageReportQuery = damageReportQuery;
        _damageReportCRUD  = damageReportCRUD;
    }

    // ReturnRequest.cshtml — return processing queue
    [HttpGet("")]
    [HttpGet("index")]
    [HttpGet("DisplayReturnRequests")]
    public IActionResult DisplayReturnRequests()
    {
        try
        {
            var requests = _returnOrderQuery.GetAllReturnRequests();
            var itemMap  = new Dictionary<int, List<Returnitem>>();
            foreach (var req in requests)
            {
                itemMap[req.GetReturnRequestId()] =
                    _returnItemQuery.GetReturnItemByRequestId(req.GetReturnRequestId());
            }
            ViewBag.ItemMap = itemMap;
            return View("~/Views/Module2/ReturnProcess/ReturnRequest.cshtml", requests);
        }
        catch
        {
            TempData["Message"] = "Unable to load return requests. Please verify database access permissions.";
            return View("~/Views/Module2/ReturnProcess/ReturnRequest.cshtml", new List<Returnrequest>());
        }
    }

    // ReturnItemDetail.cshtml — list of items inside one return request
    [HttpGet("ShowReturnDetails/{requestId:int}")]
    public IActionResult ShowReturnDetails(int requestId)
    {
        try
        {
            var request = _returnOrderQuery.GetReturnRequestById(requestId);
            if (request is null) return NotFound();

            var items = _returnItemQuery.GetReturnItemByRequestId(requestId);
            ViewBag.Request = request;
            return View("~/Views/Module2/ReturnProcess/ReturnItemDetail.cshtml", items);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    // ReturnItemDamageReport.cshtml — 4-phase processing form for one item
    [HttpGet("DisplayRepairProgress/{itemId:int}")]
    public IActionResult DisplayRepairProgress(int itemId)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            ViewBag.DamageReport = _damageReportQuery.GetDamageReportByReturnItem(itemId);
            return View("~/Views/Module2/ReturnProcess/ReturnItemDamageReport.cshtml", item);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    // ReturnItemDamageReport.cshtml — direct link to damage report section
    [HttpGet("DisplayDamageReport/{returnItemId:int}")]
    public IActionResult DisplayDamageReport(int returnItemId)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(returnItemId);
            if (item is null) return NotFound();

            ViewBag.DamageReport = _damageReportQuery.GetDamageReportByReturnItem(returnItemId);
            return View("~/Views/Module2/ReturnProcess/ReturnItemDamageReport.cshtml", item);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    // ReturnItemDetail.cshtml — status tracking (same as ShowReturnDetails)
    [HttpGet("ShowReturnStatusTracking/{requestId:int}")]
    public IActionResult ShowReturnStatusTracking(int requestId)
    {
        try
        {
            var request = _returnOrderQuery.GetReturnRequestById(requestId);
            if (request is null) return NotFound();

            var items = _returnItemQuery.GetReturnItemByRequestId(requestId);
            ViewBag.Request = request;
            return View("~/Views/Module2/ReturnProcess/ReturnItemDetail.cshtml", items);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    // Upload damage images — saves to existing damage report
    [HttpPost("UploadDamageImages/{returnItemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult UploadDamageImages(int returnItemId, string? imageUrls)
    {
        try
        {
            var existing = _damageReportQuery.GetDamageReportByReturnItem(returnItemId);
            if (existing is null)
            {
                TempData["Message"] = "No damage report found for this item.";
                return RedirectToAction(nameof(DisplayDamageReport), new { returnItemId });
            }
            existing.SetImages(imageUrls ?? string.Empty);
            TempData["Message"] = _damageReportCRUD.SubmitDamageReport(returnItemId, existing)
                ? "Damage images updated successfully."
                : "Failed to update damage images.";
            return RedirectToAction(nameof(DisplayDamageReport), new { returnItemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    // Single POST handler for all 4 phases + damage report save
    [HttpPost("HandleReturnSubmission/{itemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult HandleReturnSubmission(int itemId, string stage,
        bool hasDamage = false, string? description = null, string? severity = null,
        decimal? repairCost = null, string? images = null,
        string? partsUsed = null, string? technicianNotes = null,
        string? beforeImageUrl = null, string? afterImageUrl = null,
        string? serviceType = null, string? technicianName = null,
        string? serviceNotes = null, string? partsChecked = null,
        string? cleaningMethod = null, string? cleaningAgent = null,
        string? cleanedBy = null, string? cleaningNotes = null)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            switch (stage.ToUpper())
            {
                case "DAMAGE_INSPECTION":
                    if (item.GetStatus() != ReturnItemStatus.DAMAGE_INSPECTION)
                    {
                        TempData["Message"] = "This item is no longer in the Damage Inspection stage.";
                        break;
                    }
                    if (hasDamage) item.ConductRepairing();
                    else           item.ConductServicing();
                    TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                        ? (hasDamage ? "Damage inspection submitted. Status set to Repairing."
                                     : "No damage found. Status set to Servicing.")
                        : "Failed to submit damage inspection.";
                    break;

                case "SAVE_DAMAGE_REPORT":
                    if (item.GetStatus() != ReturnItemStatus.DAMAGE_INSPECTION)
                    {
                        TempData["Message"] = "Damage report can no longer be edited.";
                        break;
                    }
                    var existing = _damageReportQuery.GetDamageReportByReturnItem(itemId);
                    Damagereport report = existing ?? new Damagereport();
                    report.SetReturnItemId(itemId);
                    report.SetDescription(description ?? string.Empty);
                    report.SetSeverity(severity ?? string.Empty);
                    report.SetRepairCost(repairCost ?? 0m);
                    report.SetImages(images ?? string.Empty);
                    report.SetReportDate(DateTime.UtcNow);
                    TempData["Message"] = _damageReportCRUD.SubmitDamageReport(itemId, report)
                        ? "Damage report saved successfully."
                        : "Failed to save damage report.";
                    break;

                case "REPAIRING":
                    if (item.GetStatus() != ReturnItemStatus.REPAIRING)
                    {
                        TempData["Message"] = "This item is not in the Repairing stage.";
                        break;
                    }
                    item.ConductServicing();
                    TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                        ? "Repair submitted. Status set to Servicing."
                        : "Failed to submit repair.";
                    break;

                case "SERVICING":
                    if (item.GetStatus() != ReturnItemStatus.SERVICING)
                    {
                        TempData["Message"] = "This item is not in the Servicing stage.";
                        break;
                    }
                    item.ConductCleaning();
                    TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                        ? "Servicing submitted. Status set to Cleaning."
                        : "Failed to submit servicing.";
                    break;

                case "CLEANING":
                    if (item.GetStatus() != ReturnItemStatus.CLEANING)
                    {
                        TempData["Message"] = "This item is not in the Cleaning stage.";
                        break;
                    }
                    item.CompleteReturn();
                    TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                        ? "Cleaning complete. Item returned to inventory."
                        : "Failed to submit cleaning.";
                    break;

                default:
                    TempData["Message"] = "Unknown submission stage.";
                    break;
            }

            return RedirectToAction(nameof(DisplayRepairProgress), new { itemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayRepairProgress), new { itemId });
        }
    }
}