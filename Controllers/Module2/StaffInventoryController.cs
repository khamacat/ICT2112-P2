using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Module2.P2_3;

namespace ProRental.Controllers.Module2;

[Route("module2/[controller]")]
public class StaffInventoryController : Controller
{
    private readonly iInventoryCRUDControl _crudControl;
    private readonly iInventoryStatusControl _statusControl;
    private readonly iInventoryQueryControl _queryControl;

    public StaffInventoryController(iInventoryCRUDControl crudControl, iInventoryStatusControl statusControl, iInventoryQueryControl queryControl)
    {
        _crudControl = crudControl;
        _statusControl = statusControl;
        _queryControl = queryControl;
    }

    [HttpGet("")]
    [HttpGet("index")]
    [HttpGet("DisplayInventoryList")]
    public async Task<IActionResult> DisplayInventoryList()
    {
        try
        {
            var items = _queryControl.GetAllInventoryItems();
            return View("~/Views/Module2/StaffInventory.cshtml", items);
        }
        catch
        {
            TempData["Message"] = "Unable to load inventory items. Please verify database access permissions for the application user.";
            return View("~/Views/Module2/StaffInventory.cshtml", new List<Inventoryitem>());
        }
    }

    [HttpGet("ShowProductDetails/{inventoryItemId:int}")]
    public IActionResult ShowProductDetails(int inventoryItemId)
    {
        var item = _crudControl.GetInventoryItemById(inventoryItemId);

        if (item is null)
        {
            return NotFound();
        }

        return View("~/Views/Module2/StaffInventoryItem.cshtml", item);
    }

    [HttpPost("HandleBulkOperation")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleBulkOperation(int[] inventoryItemIds, string status)
    {
        try
        {
            // Validate checkboxes
            if (inventoryItemIds == null || inventoryItemIds.Length == 0)
            {
                TempData["Message"] = "Please select at least one item.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }

            // Validate status
            if (string.IsNullOrWhiteSpace(status))
            {
                TempData["Message"] = "Please select a valid status.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }

            // Try to parse the status string to enum
            if (!Enum.TryParse<InventoryStatus>(status, out var parsedStatus))
            {
                TempData["Message"] = $"Invalid status value: {status}";
                return RedirectToAction(nameof(DisplayInventoryList));
            }

            int updatedCount = 0;
            int failedCount = 0;

            foreach (var itemId in inventoryItemIds)
            {
                if (_statusControl.UpdateInventoryStatus(itemId, parsedStatus))
                {
                    updatedCount++;
                }
                else
                {
                    failedCount++;
                }
            }

            if (failedCount > 0)
            {
                TempData["Message"] = $"Updated {updatedCount} item(s) to {parsedStatus}. {failedCount} item(s) failed to update.";
            }
            else
            {
                TempData["Message"] = $"Updated {updatedCount} item(s) to {parsedStatus}.";
            }

            return RedirectToAction(nameof(DisplayInventoryList));
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayInventoryList));
        }
    }

    [HttpGet("HandleSearch")]
    public IActionResult HandleSearch(string? query)
    {
        try
        {
            var items = _queryControl.SearchInventoryItems(query ?? "");
            ViewData["Query"] = query;
            return View("~/Views/Module2/StaffInventory.cshtml", items);
        }
        catch
        {
            TempData["Message"] = "Unable to search inventory items. Please verify database access permissions for the application user.";
            ViewData["Query"] = query;
            return View("~/Views/Module2/StaffInventory.cshtml", new List<Inventoryitem>());
        }
    }

    [HttpGet("CreateInventoryItem")]
    public IActionResult CreateInventoryItem()
    {
        var inventoryItem = new Inventoryitem();
        inventoryItem.SetCreatedDate(DateTime.UtcNow);
        inventoryItem.SetUpdatedDate(DateTime.UtcNow);
        
        return PartialView("~/Views/Module2/Partials/CreateInventoryItemForm.cshtml", inventoryItem);
    }

    [HttpPost("CreateInventoryItem")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateInventoryItemPost(int productId, string serialNumber, InventoryStatus? status, DateTime? expiryDate)
    {
        try
        {
            if (productId <= 0)
            {
                TempData["Message"] = "Product ID must be greater than 0.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }

            if (_crudControl.CreateInventoryItem(productId, serialNumber, status ?? InventoryStatus.AVAILABLE, expiryDate))
            {
                TempData["Message"] = "Inventory item created successfully.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }
            else
            {
                TempData["Message"] = "Failed to create inventory item. Please verify the product ID, serial number format, and ensure no duplicate serial number exists.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayInventoryList));
        }
    }

    [HttpGet("UpdateInventoryItem/{inventoryItemId:int}")]
    public IActionResult UpdateInventoryItem(int inventoryItemId)
    {
        var item = _crudControl.GetInventoryItemById(inventoryItemId);

        if (item is null)
        {
            return NotFound();
        }

        return PartialView("~/Views/Module2/Partials/UpdateInventoryItemForm.cshtml", item);
    }

    [HttpPost("UpdateInventoryItem")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateInventoryItemPost(int inventoryItemId, int productId, string serialNumber, InventoryStatus? status, DateTime? expiryDate)
    {
        try
        {
            var existingItem = _crudControl.GetInventoryItemById(inventoryItemId);

            if (existingItem is null)
            {
                TempData["Message"] = "Inventory item not found.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }

            if (productId <= 0)
            {
                TempData["Message"] = "Product ID must be greater than 0.";
                return RedirectToAction(nameof(ShowProductDetails), new { inventoryItemId });
            }

            if (_crudControl.UpdateInventoryItem(inventoryItemId, productId, serialNumber, status ?? InventoryStatus.AVAILABLE, expiryDate))
            {
                TempData["Message"] = "Inventory item updated successfully.";
                return RedirectToAction(nameof(ShowProductDetails), new { inventoryItemId });
            }
            else
            {
                TempData["Message"] = "Failed to update inventory item. Please verify the product ID, serial number format, and ensure no duplicate serial number exists.";
                return RedirectToAction(nameof(ShowProductDetails), new { inventoryItemId });
            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayInventoryList));
        }
    }

    [HttpPost("DeleteInventoryItem/{inventoryItemId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteInventoryItem(int inventoryItemId)
    {
        try
        {
            if (_crudControl.DeleteInventoryItem(inventoryItemId))
            {
                TempData["Message"] = "Inventory item deleted successfully.";
            }
            else
            {
                TempData["Message"] = "Inventory item not found or could not be deleted.";
            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
        }

        return RedirectToAction(nameof(DisplayInventoryList));
    }

    [HttpPost("DeleteMultipleInventoryItems")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteMultipleInventoryItems(int[] inventoryItemIds)
    {
        if (inventoryItemIds.Length == 0)
        {
            TempData["Message"] = "Please select at least one item to delete.";
            return RedirectToAction(nameof(DisplayInventoryList));
        }

        try
        {
            int deletedCount = 0;
            int failedCount = 0;

            foreach (var itemId in inventoryItemIds)
            {
                if (_crudControl.DeleteInventoryItem(itemId))
                {
                    deletedCount++;
                }
                else
                {
                    failedCount++;
                }
            }

            if (failedCount > 0)
            {
                TempData["Message"] = $"Deleted {deletedCount} item(s). {failedCount} item(s) failed to delete.";
            }
            else
            {
                TempData["Message"] = $"Deleted {deletedCount} item(s).";
            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
        }

        return RedirectToAction(nameof(DisplayInventoryList));
    }
}

