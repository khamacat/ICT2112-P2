using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

[Route("CategoryCatalog")]
public class CategoryCatalogController : Controller
{
    private readonly ICategoryCRUD _categoryCRUD;
    private readonly ICategoryQuery _categoryQuery;

    // Use Constructor Injection for the Domain Interfaces
    public CategoryCatalogController(ICategoryCRUD categoryCRUD, ICategoryQuery categoryQuery)
    {
        _categoryCRUD = categoryCRUD;
        _categoryQuery = categoryQuery;
    }

    // GET: /CategoryCatalog
    [HttpGet]
    [Route("")] 
    public IActionResult Index()
    {
        // Use the Query interface to fetch data
        var list = _categoryQuery.GetAllCategories() ?? new List<Category>();
        
        return View("~/Views/Module2/CategoryCatalog/CategoryCatalog.cshtml", list);
    }

    // POST: /CategoryCatalog/HandleCategoryCRUD
    [HttpPost]
    [Route("HandleCategoryCRUD")]
    [ValidateAntiForgeryToken]
    public IActionResult HandleCategoryCRUD(string categoryName, string categoryDescription)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            // 1. Instantiate the Entity
            var newCat = new Category();

            // 2. Use DDD methods from your partial class to set data
            newCat.SetName(categoryName);
            
            // Set description from form
            newCat.SetDescription(categoryDescription);

            // 3. Use the CRUD interface for business logic and persistence
            _categoryCRUD.CreateCategory(newCat);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /CategoryCatalog/Delete
    [HttpPost]
    [Route("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        // Call the Domain Interface (ICategoryCRUD) to handle the deletion logic
        bool success = _categoryCRUD.DeleteCategory(id);
        
        return RedirectToAction(nameof(Index));
    }
}