using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Controls;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

[Route("module2/[controller]")]
public class ReturnProcessingController : Controller
{
    private readonly iReturnOrderQuery   _returnOrderQuery;
    private readonly iReturnOrderCRUD    _returnOrderCRUD;
    private readonly iReturnItemQuery    _returnItemQuery;
    private readonly iReturnItemCRUD     _returnItemCRUD;
    private readonly iDamageReportQuery  _damageReportQuery;
    private readonly iDamageReportCRUD   _damageReportCRUD;
    private readonly ReturnOrderControl  _returnOrderControl;
    private readonly ReturnItemControl   _returnItemControl;

    public ReturnProcessingController(
        iReturnOrderQuery  returnOrderQuery,
        iReturnOrderCRUD   returnOrderCRUD,
        iReturnItemQuery   returnItemQuery,
        iReturnItemCRUD    returnItemCRUD,
        iDamageReportQuery damageReportQuery,
        iDamageReportCRUD  damageReportCRUD,
        ReturnOrderControl returnOrderControl,
        ReturnItemControl  returnItemControl
    )
    {
        _returnOrderQuery   = returnOrderQuery;
        _returnOrderCRUD    = returnOrderCRUD;
        _returnItemQuery    = returnItemQuery;
        _returnItemCRUD     = returnItemCRUD;
        _damageReportQuery  = damageReportQuery;
        _damageReportCRUD   = damageReportCRUD;
        _returnOrderControl = returnOrderControl;
        _returnItemControl  = returnItemControl;
    }

    // ── ReturnRequest.cshtml — queue of orders awaiting return ────────────
    [HttpGet("")]
    [HttpGet("index")]
    [HttpGet("DisplayReturnRequests")]
    public IActionResult DisplayReturnRequests()
    {
        try
        {
            var requests = _returnOrderQuery.GetAllReturnRequests();

            var itemMap        = new Dictionary<int, List<Returnitem>>();
            var damageReportMap = new Dictionary<int, bool>(); // itemId → has damage report

            foreach (var req in requests)
            {
                var items = _returnItemQuery.GetReturnItemByRequestId(req.GetReturnRequestId());
                itemMap[req.GetReturnRequestId()] = items;

                foreach (var item in items)
                {
                    var report = _damageReportQuery.GetDamageReportByReturnItem(item.GetReturnItemId());
                    damageReportMap[item.GetReturnItemId()] = report != null;
                }
            }

            ViewBag.ItemMap        = itemMap;
            ViewBag.DamageReportMap = damageReportMap;
            return View("~/Views/Module2/ReturnProcess/ReturnRequest.cshtml", requests);
        }
        catch
        {
            TempData["Message"] = "Unable to load return requests.";
            return View("~/Views/Module2/ReturnProcess/ReturnRequest.cshtml", new List<Returnrequest>());
        }
    }

    // ── ReturnItemDetail.cshtml — items inside a return request ──────────
    [HttpGet("ShowReturnDetails/{requestId:int}")]
    public IActionResult ShowReturnDetails(int requestId)
    {
        try
        {
            var request = _returnOrderQuery.GetReturnRequestById(requestId);
            if (request is null) return NotFound();

            var items = _returnItemQuery.GetReturnItemByRequestId(requestId);

            // Build damage report map: itemId → DamageReport (null if none)
            var damageReportMap = new Dictionary<int, ProRental.Domain.Entities.Damagereport?>();
            foreach (var item in items)
            {
                damageReportMap[item.GetReturnItemId()] =
                    _damageReportQuery.GetDamageReportByReturnItem(item.GetReturnItemId());
            }

            ViewBag.Request         = request;
            ViewBag.DamageReportMap = damageReportMap;
            return View("~/Views/Module2/ReturnProcess/ReturnItemDetail.cshtml", items);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    // ── ReturnItemDamageReport.cshtml — 4-phase processing form ──────────
    [HttpGet("ReturnForm/{itemId:int}")]
    public IActionResult ReturnForm(int itemId)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            // Pass existing damage report (if any) to pre-fill the modal
            ViewBag.DamageReport = _damageReportQuery.GetDamageReportByReturnItem(itemId);
            return View("~/Views/Module2/ReturnProcess/ReturnItemDamageReport.cshtml", item);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    // ── Save damage report (from popup modal in Phase 1) ─────────────────
    // hasDamage is a JS-side flag only — it determines whether to show the
    // damage report modal in the view. It is NOT stored in the database.
    [HttpPost("SaveDamageReport/{returnItemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult SaveDamageReport(int returnItemId,
        string? description, string? severity, decimal? repairCost, string? images)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(returnItemId);
            if (item is null) return NotFound();

            if (item.GetStatus() != ReturnItemStatus.DAMAGE_INSPECTION)
            {
                TempData["Message"] = "Damage report can no longer be edited.";
                return RedirectToAction(nameof(ReturnForm), new { itemId = returnItemId });
            }

            var existing = _damageReportQuery.GetDamageReportByReturnItem(returnItemId);
            Damagereport report = existing ?? new Damagereport();
            report.SetReturnItemId(returnItemId);
            report.SetDescription(description ?? string.Empty);
            report.SetSeverity(severity ?? string.Empty);
            report.SetRepairCost(repairCost ?? 0m);
            report.SetImages(images ?? string.Empty);
            report.SetReportDate(DateTime.UtcNow);

            TempData["Message"] = _damageReportCRUD.SubmitDamageReport(returnItemId, report)
                ? "Damage report saved."
                : "Failed to save damage report.";

            return RedirectToAction(nameof(ReturnForm), new { itemId = returnItemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(ReturnForm), new { itemId = returnItemId });
        }
    }

    // ── Submit Phase 1: Damage Inspection ────────────────────────────────
    // hasDamage: true → REPAIRING, false → SERVICING (skips repair phase)
    [HttpPost("SubmitDamageInspection/{itemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitDamageInspection(int itemId, bool hasDamage)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            if (item.GetStatus() != ReturnItemStatus.DAMAGE_INSPECTION)
            {
                TempData["Message"] = "Item is no longer in Damage Inspection stage.";
                return RedirectToAction(nameof(ReturnForm), new { itemId });
            }

            if (hasDamage) item.ConductRepairing();
            else           item.ConductServicing();

            TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                ? (hasDamage ? "Damage found. Status set to Repairing."
                             : "No damage. Status set to Servicing.")
                : "Failed to submit inspection.";

            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
    }

    // ── Submit Phase 2: Repairing → Servicing ────────────────────────────
    [HttpPost("SubmitRepairing/{itemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitRepairing(int itemId)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            if (item.GetStatus() != ReturnItemStatus.REPAIRING)
            {
                TempData["Message"] = "Item is not in the Repairing stage.";
                return RedirectToAction(nameof(ReturnForm), new { itemId });
            }

            item.ConductServicing();

            TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                ? "Repair complete. Status set to Servicing."
                : "Failed to submit repair.";

            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
    }

    // ── Submit Phase 3: Servicing → Cleaning ─────────────────────────────
    [HttpPost("SubmitServicing/{itemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitServicing(int itemId)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            if (item.GetStatus() != ReturnItemStatus.SERVICING)
            {
                TempData["Message"] = "Item is not in the Servicing stage.";
                return RedirectToAction(nameof(ReturnForm), new { itemId });
            }

            item.ConductCleaning();

            TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                ? "Servicing complete. Status set to Cleaning."
                : "Failed to submit servicing.";

            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
    }

    // ── Submit Phase 4: Cleaning → Return to Inventory ───────────────────
    [HttpPost("SubmitCleaning/{itemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitCleaning(int itemId)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            if (item.GetStatus() != ReturnItemStatus.CLEANING)
            {
                TempData["Message"] = "Item is not in the Cleaning stage.";
                return RedirectToAction(nameof(ReturnForm), new { itemId });
            }

            // Step 1: CompleteReturnItemProcess — sets item RETURN_TO_INVENTORY + inventory AVAILABLE
            bool itemDone = _returnItemControl.CompleteReturnItemProcess(itemId);
            if (!itemDone)
            {
                TempData["Message"] = "Failed to complete item process.";
                return RedirectToAction(nameof(ReturnForm), new { itemId });
            }

            // Step 2: Check if ALL items in request are done, then close ReturnRequest
            var allItems = _returnItemQuery.GetReturnItemByRequestId(item.GetReturnRequestId());
            if (allItems.All(i => i.GetStatus() == ReturnItemStatus.RETURN_TO_INVENTORY))
            {
                _returnOrderControl.CompleteReturnProcess(item.GetReturnRequestId());
            }

            TempData["Message"] = "Cleaning complete. Item returned to inventory.";
            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
    }

    // ── Export damage report as PDF ───────────────────────────────────────
    [HttpGet("ExportDamageReport/{itemId:int}")]
    public IActionResult ExportDamageReport(int itemId)
    {
        try
        {
            var report = _damageReportQuery.GetDamageReportByReturnItem(itemId);
            if (report is null) return NotFound();

            var bytes = _damageReportQuery.DownloadDamageReport(report.GetDamageReportId());
            if (bytes == null || bytes.Length == 0) return NotFound();

            return File(bytes, "text/html", $"DamageReport_Item{itemId}.html");
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Export failed: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }
}