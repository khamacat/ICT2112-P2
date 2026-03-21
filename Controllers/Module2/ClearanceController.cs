using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Module2;

namespace ProRental.Controllers;

public class ClearanceController : Controller
{
    private readonly iClearanceBatchControl _batchControl;
    private readonly iClearanceBatchQuery _batchQuery;
    private readonly iClearanceItemControl _itemControl;
    private readonly iClearanceItemQuery _itemQuery;

    public ClearanceController(
        iClearanceBatchControl batchControl,
        iClearanceBatchQuery batchQuery,
        iClearanceItemControl itemControl,
        iClearanceItemQuery itemQuery)
    {
        _batchControl = batchControl;
        _batchQuery = batchQuery;
        _itemControl = itemControl;
        _itemQuery = itemQuery;
    }

    // ── Batch List (Dashboard) ─────────────────────────────────────────────────

    public IActionResult Index()
    {
        var batches = _batchControl.GetAllBatches();
        return View(batches);
    }

    // ── Create Batch ───────────────────────────────────────────────────────────

    [HttpGet]
    public IActionResult CreateBatch()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBatch(string batchName, DateTime clearanceDate)
    {
        var batch = new Clearancebatch();
        batch.SetBatchName(batchName);
        // Npgsql requires DateTimes to be strictly UTC when saving to TIMESTAMPTZ columns
        batch.SetClearanceDate(DateTime.SpecifyKind(clearanceDate, DateTimeKind.Utc));

        bool success = _batchControl.CreateClearanceBatch(batch);
        if (!success)
        {
            TempData["Error"] = "Failed to create batch. Check that the name is unique and the date is valid.";
            return View();
        }

        TempData["Success"] = "Clearance batch created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // ── Batch Details ──────────────────────────────────────────────────────────

    public IActionResult BatchDetails(int id)
    {
        var batch = _batchQuery.GetClearanceBatchById(id);
        if (batch == null)
            return NotFound();

        var items = _itemQuery.GetClearanceItemsByBatch(id);
        ViewBag.ClearanceItems = items;

        return View(batch);
    }

    // ── Batch Lifecycle Actions ────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActivateBatch(int id)
    {
        bool success = _batchControl.ActivateBatch(id);
        TempData[success ? "Success" : "Error"] = success
            ? "Batch activated successfully."
            : "Cannot activate this batch. It must be in SCHEDULED status.";
        return RedirectToAction(nameof(BatchDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CloseBatch(int id)
    {
        bool success = _batchControl.CloseBatch(id);
        TempData[success ? "Success" : "Error"] = success
            ? "Batch closed successfully."
            : "Cannot close this batch. It must be in ACTIVE status.";
        return RedirectToAction(nameof(BatchDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteBatch(int id)
    {
        bool success = _batchControl.DeleteClearanceBatch(id);
        TempData[success ? "Success" : "Error"] = success
            ? "Batch deleted successfully."
            : "Cannot delete this batch. Active batches cannot be deleted.";
        return RedirectToAction(nameof(Index));
    }

    // ── Add Items to Batch ─────────────────────────────────────────────────────

    [HttpGet]
    public IActionResult AddItems(int batchId)
    {
        var batch = _batchQuery.GetClearanceBatchById(batchId);
        if (batch == null)
            return NotFound();

        ViewBag.BatchId = batchId;
        ViewBag.BatchName = batch.GetBatchName();

        // Get available inventory items for selection
        // Note: the eligibility check runs per-item during AddItemToBatch
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddItems(int batchId, List<int> inventoryItemIds)
    {
        if (inventoryItemIds == null || inventoryItemIds.Count == 0)
        {
            TempData["Error"] = "No items selected.";
            return RedirectToAction(nameof(AddItems), new { batchId });
        }

        bool success = _itemControl.AddItemsToBatch(batchId, inventoryItemIds);
        TempData[success ? "Success" : "Warning"] = success
            ? $"{inventoryItemIds.Count} item(s) added to the batch."
            : "Some items could not be added. They may be ineligible or already in another batch.";

        return RedirectToAction(nameof(BatchDetails), new { id = batchId });
    }

    // ── Remove Item from Batch ─────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveItem(int clearanceItemId, int batchId)
    {
        bool success = _itemControl.RemoveItemFromBatch(clearanceItemId);
        TempData[success ? "Success" : "Error"] = success
            ? "Item removed from batch."
            : "Cannot remove this item. It may have been sold.";
        return RedirectToAction(nameof(BatchDetails), new { id = batchId });
    }

    // ── Record Sale ────────────────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RecordSale(int clearanceItemId, decimal finalPrice, int batchId)
    {
        // Using staffId = 1 as a placeholder since auth is not yet implemented
        bool success = _itemControl.RecordSale(clearanceItemId, finalPrice, 1);
        TempData[success ? "Success" : "Error"] = success
            ? "Sale recorded successfully."
            : "Failed to record sale.";
        return RedirectToAction(nameof(BatchDetails), new { id = batchId });
    }

    // ── Calculate Price (AJAX endpoint) ────────────────────────────────────────

    [HttpGet]
    public IActionResult CalculatePrice(int clearanceItemId)
    {
        decimal price = _itemControl.CalculateClearancePrice(clearanceItemId);
        return Json(new { recommendedPrice = price });
    }
}
