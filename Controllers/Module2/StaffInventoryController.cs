using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Controllers.Module2;

[Route("module2/[controller]")]
public class StaffInventoryController : Controller
{
    private readonly AppDbContext _dbContext;

    public StaffInventoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    [HttpGet("index")]
    [HttpGet("DisplayInventoryList")]
    public async Task<IActionResult> DisplayInventoryList()
    {
        try
        {
            var items = await _dbContext.Inventoryitems
                .AsNoTracking()
                .OrderBy(i => EF.Property<int>(i, "Inventoryid"))
                .ToListAsync();

            return View("~/Views/Module2/StaffInventory.cshtml", items);
        }
        catch
        {
            TempData["Message"] = "Unable to load inventory items. Please verify database access permissions for the application user.";
            return View("~/Views/Module2/StaffInventory.cshtml", new List<Inventoryitem>());
        }
    }

    [HttpGet("ShowProductDetails/{inventoryItemId:int}")]
    public async Task<IActionResult> ShowProductDetails(int inventoryItemId)
    {
        var item = await _dbContext.Inventoryitems
            .AsNoTracking()
            .FirstOrDefaultAsync(i => EF.Property<int>(i, "Inventoryid") == inventoryItemId);

        if (item is null)
        {
            return NotFound();
        }

        return View("~/Views/Module2/StaffInventoryItem.cshtml", item);
    }

    [HttpPost("HandleBulkOperation")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleBulkOperation(int[] inventoryItemIds, InventoryStatus status)
    {
        if (inventoryItemIds.Length == 0)
        {
            TempData["Message"] = "Please select at least one item.";
            return RedirectToAction(nameof(DisplayInventoryList));
        }

        var items = await _dbContext.Inventoryitems
            .Where(i => inventoryItemIds.Contains(EF.Property<int>(i, "Inventoryid")))
            .ToListAsync();

        foreach (var item in items)
        {
            item.SetStatus(status);
            item.SetUpdatedDate(DateTime.UtcNow);
        }

        await _dbContext.SaveChangesAsync();
        TempData["Message"] = $"Updated {items.Count} item(s) to {status}.";

        return RedirectToAction(nameof(DisplayInventoryList));
    }

    [HttpGet("HandleSearch")]
    public async Task<IActionResult> HandleSearch(string? query)
    {
        try
        {
            var inventoryQuery = _dbContext.Inventoryitems.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var normalized = query.Trim();
                var hasNumericQuery = int.TryParse(normalized, out var numericQuery);

                inventoryQuery = inventoryQuery.Where(i =>
                    EF.Property<string>(i, "Serialnumber").Contains(normalized) ||
                    (hasNumericQuery && EF.Property<int>(i, "Productid") == numericQuery) ||
                    (hasNumericQuery && EF.Property<int>(i, "Inventoryid") == numericQuery));
            }

            var items = await inventoryQuery
                .OrderBy(i => EF.Property<int>(i, "Inventoryid"))
                .ToListAsync();

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
}
