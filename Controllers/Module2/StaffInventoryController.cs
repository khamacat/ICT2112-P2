using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

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
            return View("~/Views/Module2/Inventory/StaffInventory.cshtml", items);
        }
        catch
        {
            TempData["Message"] = "Unable to load inventory items. Please verify database access permissions for the application user.";
            return View("~/Views/Module2/Inventory/StaffInventory.cshtml", new List<Inventoryitem>());
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

        return View("~/Views/Module2/Inventory/StaffInventoryItem.cshtml", item);
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
    public IActionResult HandleSearch(string? searchType, string? searchValue)
    {
        try
        {
            List<Inventoryitem> items = new();

            if (!string.IsNullOrWhiteSpace(searchType) && !string.IsNullOrWhiteSpace(searchValue))
            {
                if (string.Equals(searchType, "serialNumber", StringComparison.OrdinalIgnoreCase))
                {
                    items = _queryControl.SearchInventoryItems(searchValue.ToUpper());
                }
                else if (string.Equals(searchType, "productId", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(searchValue, out var productId) && productId > 0)
                    {
                        items = _queryControl.GetInventoryByProduct(productId);
                    }
                }
                else if (string.Equals(searchType, "inventoryId", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(searchValue, out var inventoryId) && inventoryId > 0)
                    {
                        var item = _crudControl.GetInventoryItemById(inventoryId);
                        items = item != null ? new List<Inventoryitem> { item } : new();
                    }
                }
                else if (string.Equals(searchType, "status", StringComparison.OrdinalIgnoreCase))
                {
                    if (Enum.TryParse<InventoryStatus>(searchValue.ToUpper(), ignoreCase: true, out var status))
                    {
                        items = _queryControl.GetInventoryByStatus(status);
                    }
                }
                else
                {
                    items = _queryControl.GetAllInventoryItems();
                }
            }
            else
            {
                items = _queryControl.GetAllInventoryItems();
            }

            ViewData["SearchType"] = searchType;
            ViewData["SearchValue"] = searchValue;
            return View("~/Views/Module2/Inventory/StaffInventory.cshtml", items);
        }
        catch
        {
            TempData["Message"] = "Unable to search inventory items. Please verify database access permissions for the application user.";
            ViewData["SearchType"] = searchType;
            return View("~/Views/Module2/Inventory/StaffInventory.cshtml", new List<Inventoryitem>());
        }
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

    [HttpPost("CreateBulkInventoryItemsPost")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBulkInventoryItemsPost(int productId, int quantity)
    {
        try
        {
            if (productId <= 0)
            {
                TempData["Message"] = "Product ID must be greater than 0.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }

            if (quantity <= 0 || quantity > 100)
            {
                TempData["Message"] = "Quantity must be between 1 and 100.";
                return RedirectToAction(nameof(DisplayInventoryList));
            }

            int createdCount = _crudControl.CreateBulkInventoryItems(productId, quantity);

            if (createdCount > 0)
            {
                TempData["Message"] = $"Successfully created {createdCount} inventory item(s) with RESERVED status and temporary serial numbers. Click 'View' on each item to replace the temporary serial number with the actual one.";
            }
            else
            {
                TempData["Message"] = $"Failed to create inventory items. Please check console output for details. Common causes: Product ID {productId} may not exist, or database connectivity issues.";
            }

            return RedirectToAction(nameof(DisplayInventoryList));
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(DisplayInventoryList));
        }
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

            var finalStatus = status ?? InventoryStatus.AVAILABLE;

            if (_crudControl.UpdateInventoryItem(inventoryItemId, productId, serialNumber, finalStatus, expiryDate))
            {
                // Sync product availability status based on inventory
                _statusControl.UpdateInventoryStatus(inventoryItemId, finalStatus);

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

    [HttpPost("DeleteInventoryItem")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteInventoryItem(int inventoryItemId)
    {
        try
        {
            Console.WriteLine($"DeleteInventoryItem POST: Attempting to delete item {inventoryItemId}");
            if (_crudControl.DeleteInventoryItem(inventoryItemId))
            {
                TempData["Message"] = "Inventory item deleted successfully.";
                Console.WriteLine($"DeleteInventoryItem POST: Item {inventoryItemId} deletion succeeded");
            }
            else
            {
                TempData["Message"] = "Inventory item not found or could not be deleted.";
                Console.WriteLine($"DeleteInventoryItem POST: Item {inventoryItemId} deletion failed (returned false)");
            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
            Console.WriteLine($"DeleteInventoryItem POST: Exception - {ex.GetType().Name}: {ex.Message}");
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

