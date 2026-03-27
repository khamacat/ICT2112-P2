using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Controls;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

[Route("module2/[controller]")]
[StaffAuth]
public class ReturnProcessingController : Controller
{
    private readonly iReturnOrderQuery _returnOrderQuery;
    private readonly iReturnOrderCRUD _returnOrderCRUD;
    private readonly iReturnItemQuery _returnItemQuery;
    private readonly iReturnItemCRUD _returnItemCRUD;
    private readonly iDamageReportQuery _damageReportQuery;
    private readonly iDamageReportCRUD _damageReportCRUD;
    private readonly ReturnOrderControl _returnOrderControl;
    private readonly ReturnItemControl _returnItemControl;

    public ReturnProcessingController(
        iReturnOrderQuery returnOrderQuery,
        iReturnOrderCRUD returnOrderCRUD,
        iReturnItemQuery returnItemQuery,
        iReturnItemCRUD returnItemCRUD,
        iDamageReportQuery damageReportQuery,
        iDamageReportCRUD damageReportCRUD,
        ReturnOrderControl returnOrderControl,
        ReturnItemControl returnItemControl)
    {
        _returnOrderQuery = returnOrderQuery;
        _returnOrderCRUD = returnOrderCRUD;
        _returnItemQuery = returnItemQuery;
        _returnItemCRUD = returnItemCRUD;
        _damageReportQuery = damageReportQuery;
        _damageReportCRUD = damageReportCRUD;
        _returnOrderControl = returnOrderControl;
        _returnItemControl = returnItemControl;
    }

    [HttpGet("")]
    [HttpGet("index")]
    [HttpGet("DisplayReturnRequests")]
    public IActionResult DisplayReturnRequests()
    {
        try
        {
            var requests = _returnOrderQuery.GetAllReturnRequests();

            var itemMap = new Dictionary<int, List<Returnitem>>();
            var damageReportMap = new Dictionary<int, bool>();

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

            ViewBag.ItemMap = itemMap;
            ViewBag.DamageReportMap = damageReportMap;

            return View("~/Views/Module2/ReturnProcess/ReturnRequest.cshtml", requests);
        }
        catch
        {
            TempData["Message"] = "Unable to load return requests.";
            return View("~/Views/Module2/ReturnProcess/ReturnRequest.cshtml", new List<Returnrequest>());
        }
    }

    [HttpGet("ShowReturnDetails/{requestId:int}")]
    public IActionResult ShowReturnDetails(int requestId)
    {
        try
        {
            var request = _returnOrderQuery.GetReturnRequestById(requestId);
            if (request is null) return NotFound();

            var items = _returnItemQuery.GetReturnItemByRequestId(requestId);

            var damageReportMap = new Dictionary<int, Damagereport?>();
            foreach (var item in items)
            {
                damageReportMap[item.GetReturnItemId()] =
                    _damageReportQuery.GetDamageReportByReturnItem(item.GetReturnItemId());
            }

            ViewBag.Request = request;
            ViewBag.DamageReportMap = damageReportMap;

            var brokenItemIds = new HashSet<int>(
                items.Where(i => i.GetStatus() == ReturnItemStatus.RETURN_TO_INVENTORY
                    && damageReportMap.TryGetValue(i.GetReturnItemId(), out var dr)
                    && dr != null
                    && (dr.GetDescription() ?? "").Contains("Unable to repair"))
                .Select(i => i.GetReturnItemId()));

            ViewBag.BrokenItemIds = brokenItemIds;

            return View("~/Views/Module2/ReturnProcess/ReturnItemDetail.cshtml", items);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    [HttpGet("ReturnForm/{itemId:int}")]
    public IActionResult ReturnForm(int itemId)
    {
        try
        {
            var item = _returnItemQuery.GetReturnItem(itemId);
            if (item is null) return NotFound();

            var damageReport = _damageReportQuery.GetDamageReportByReturnItem(itemId);

            ViewBag.DamageReport = damageReport;
            ViewBag.IsBroken =
                item.GetStatus() == ReturnItemStatus.RETURN_TO_INVENTORY &&
                damageReport != null &&
                (damageReport.GetDescription() ?? "").Contains("Unable to repair");

            if (item.GetStatus() == ReturnItemStatus.REPAIRING)
            {
                var productPrice = _returnItemControl.GetProductPriceForItem(item.GetInventoryItemId());
                var previousCost = damageReport?.GetRepairCost() ?? 0m;

                ViewBag.ProductPrice = productPrice;
                ViewBag.UpdatedRepairCost = previousCost + productPrice;
            }

            return View("~/Views/Module2/ReturnProcess/ReturnItemDamageReport.cshtml", item);
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayReturnRequests));
        }
    }

    [HttpPost("SaveDamageReport/{returnItemId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDamageReport(
        int returnItemId,
        string? description,
        string? severity,
        decimal? repairCost,
        IFormFile? imageFile)
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
            var imagePath = existing?.GetImages() ?? string.Empty;

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "damage");

                Directory.CreateDirectory(uploadsFolder);

                var fileName =
                    $"dmg_{returnItemId}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(imageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imagePath = $"/uploads/damage/{fileName}";
            }

            TempData["Message"] = _damageReportCRUD.SaveDamageReport(
                returnItemId,
                description ?? string.Empty,
                severity ?? string.Empty,
                repairCost ?? 0m,
                imagePath)
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

    [HttpPost("ClearDamageReport/{itemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult ClearDamageReport(int itemId)
    {
        _damageReportCRUD.DeleteDamageReport(itemId);
        return Ok();
    }

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

            if (hasDamage)
                item.ConductRepairing();
            else
                item.ConductServicing();

            TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                ? (hasDamage
                    ? "Damage found. Status set to Repairing."
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

    [HttpPost("SubmitRepairing/{itemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitRepairing(int itemId, bool isFixed)
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

            if (isFixed)
            {
                var report = _damageReportQuery.GetDamageReportByReturnItem(itemId);
                if (report != null)
                {
                    var repairCost = report.GetRepairCost() ?? 0m;
                    _damageReportCRUD.AppendRepairNote(
                        itemId,
                        $"\n\nTotal price for repair is ${repairCost:F2}.");
                }

                item.ConductServicing();

                TempData["Message"] = _returnItemCRUD.UpdateReturnItem(item)
                    ? "Repair complete. Status set to Servicing."
                    : "Failed to submit repair.";
            }
            else
            {
                var report = _damageReportQuery.GetDamageReportByReturnItem(itemId);
                if (report is null)
                {
                    TempData["Message"] = "No damage report found. Cannot mark as broken.";
                    return RedirectToAction(nameof(ReturnForm), new { itemId });
                }

                var broken = _returnItemControl.MarkItemBroken(itemId, report);

                if (broken)
                {
                    _damageReportCRUD.SaveDamageReport(
                        itemId,
                        report.GetDescription() ?? string.Empty,
                        report.GetSeverity() ?? string.Empty,
                        report.GetRepairCost(),
                        report.GetImages());

                    var allItems = _returnItemQuery.GetReturnItemByRequestId(item.GetReturnRequestId());
                    if (allItems.All(i => i.GetStatus() == ReturnItemStatus.RETURN_TO_INVENTORY))
                    {
                        _returnOrderControl.CompleteReturnProcess(item.GetReturnRequestId());
                    }

                    TempData["Message"] =
                        "Item marked as broken. Inventory status updated to BROKEN successfully.";

                    return RedirectToAction(
                        nameof(ShowReturnDetails),
                        new { requestId = item.GetReturnRequestId() });
                }

                TempData["Message"] = "Failed to mark item as broken.";
            }

            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
    }

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

            var itemDone = _returnItemControl.CompleteReturnItemProcess(itemId);
            if (!itemDone)
            {
                TempData["Message"] = "Failed to complete item process.";
                return RedirectToAction(nameof(ReturnForm), new { itemId });
            }

            var allItems = _returnItemQuery.GetReturnItemByRequestId(item.GetReturnRequestId());
            if (allItems.All(i => i.GetStatus() == ReturnItemStatus.RETURN_TO_INVENTORY))
            {
                _returnOrderControl.CompleteReturnProcess(item.GetReturnRequestId());
            }

            TempData["Message"] =
                "Cleaning complete. Inventory status updated to AVAILABLE successfully.";

            return RedirectToAction(
                nameof(ShowReturnDetails),
                new { requestId = item.GetReturnRequestId() });
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(ReturnForm), new { itemId });
        }
    }

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