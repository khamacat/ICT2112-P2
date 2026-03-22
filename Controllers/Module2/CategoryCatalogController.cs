using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data; // Ensure this is imported for ICategoryMapper

namespace ProRental.Controllers;

[Route("CategoryCatalog")]
public class CategoryCatalogController : Controller
{
    // Switch from CategoryControl to the Mapper to enable Database persistence
    private readonly ICategoryMapper _categoryMapper;

    // Use Constructor Injection (Standard for ProductCatalog layout)
    public CategoryCatalogController(ICategoryMapper categoryMapper)
    {
        _categoryMapper = categoryMapper;
    }

    // GET: /CategoryCatalog
    [HttpGet]
    [Route("")] 
    public IActionResult Index()
    {
        // Fetch real data from PostgreSQL via the Mapper
        var list = _categoryMapper.FindAll() ?? new List<Category>();
        
        return View("~/Views/Module2/CategoryCatalog/CategoryCatalog.cshtml", list);
    }

    // POST: /CategoryCatalog/HandleCategoryCRUD
    [HttpPost]
    [Route("HandleCategoryCRUD")]
    [ValidateAntiForgeryToken]
    public IActionResult HandleCategoryCRUD(string categoryName)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            // 1. Instantiate the Entity
            var newCat = new Category();

            // 2. Use DDD methods to set data
            newCat.SetName(categoryName);
            newCat.SetIsActive(true);

            // 3. Persist to Database via Mapper
            _categoryMapper.Insert(newCat);
        }

        return RedirectToAction(nameof(Index));
    }
}