using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Domain.Module2.P2_2.Controls;

namespace ProRental.Controllers.Module2;

[Route("module2/[controller]/[action]")]
[StaffAuth]
public class SupplierRegistryPageController : Controller
{
    private readonly SupplierControl _supplierControl;
    private readonly SupplierCategoryChangeLogControl _categoryChangeLogControl;
    private string _currentPage = string.Empty;
    private Dictionary<string, string> _requestParams = new();

    public SupplierRegistryPageController(
        SupplierControl supplierControl,
        SupplierCategoryChangeLogControl categoryChangeLogControl)
    {
        _supplierControl = supplierControl;
        _categoryChangeLogControl = categoryChangeLogControl;
    }

    public void handleRequest(Dictionary<string, string> paramsDict)
    {
        _requestParams = paramsDict;
        _currentPage = _requestParams.TryGetValue("page", out var page) ? page : string.Empty;
    }

    public IActionResult renderView(string viewName, object model)
    {
        return View($"~/Views/Module2/{viewName}.cshtml", model);
    }

    public void updateModel(string action, object data) { }

    public object getModelData(string query) => new { query };

    [HttpGet]
    public IActionResult Index()
    {
        var requestParams = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
        handleRequest(requestParams);
        var filter = _requestParams.TryGetValue("filter", out var f) ? f : "all";

        var suppliers = filter switch
        {
            "vetted"     => _supplierControl.getVettedSuppliers(),
            "unverified" => _supplierControl.getUnverifiedSuppliers(),
            _            => _supplierControl.getAllSuppliers()
        };

        var vettingNotes = suppliers
            .Where(s => s.VettingResult != VettingDecision.PENDING)
            .ToDictionary(
                s => s.SupplierID,
                s => _supplierControl.getLatestVettingNote(s.SupplierID) ?? string.Empty);
        ViewData["VettingNotes"] = vettingNotes;

        return renderView("supplierList", suppliers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return renderView("supplierForm", _supplierControl.createEmptySupplier());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string name, string details, int creditPeriod, float avgTurnaroundTime)
    {
        try
        {
            _supplierControl.createSupplier(name, details, creditPeriod, avgTurnaroundTime);
            TempData["Success"] = "Supplier created successfully.";
        }
        catch
        {
            TempData["Error"] = "Failed to create supplier.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var supplier = _supplierControl.getSupplierById(id);
        ViewData["ChangeLogs"] = _categoryChangeLogControl.getLogsBySupplier(id);
        return renderView("supplierForm", supplier);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, string newDetails)
    {
        _supplierControl.editSupplier(id, newDetails);
        TempData["Success"] = "Supplier updated.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _supplierControl.deleteSupplier(id);
        TempData["Success"] = "Supplier deleted.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Categorize(int id, SupplierCategory newCategory, string reason)
    {
        var supplier = _supplierControl.getSupplierById(id);
        var previousCategory = supplier.SupplierCategory;
        _supplierControl.categorizeSupplier(id, newCategory);
        _categoryChangeLogControl.createLog(id, previousCategory, newCategory, reason);
        TempData["Success"] = "Supplier re-categorised.";
        return RedirectToAction(nameof(Edit), new { id });
    }
}