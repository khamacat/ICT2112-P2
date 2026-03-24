using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

[Route("CategoryCatalog")]
public class CategoryCatalogController : Controller
{
    private readonly ICategoryCRUD _categoryCRUD;
    private readonly ICategoryQuery _categoryQuery;

    public CategoryCatalogController(ICategoryCRUD categoryCRUD, ICategoryQuery categoryQuery)
    {
        _categoryCRUD = categoryCRUD;
        _categoryQuery = categoryQuery;
    }

    [HttpGet]
    [Route("")] 
    public IActionResult Index()
    {
        var list = _categoryQuery.GetAllCategories() ?? new List<Category>();
        return View("~/Views/Module2/CategoryCatalog/CategoryCatalog.cshtml", list);
    }

    // POST: /CategoryCatalog/Create
    [HttpPost]
    [Route("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string categoryName, string categoryDescription)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            var newCat = new Category();
            newCat.SetName(categoryName);
            newCat.SetDescription(categoryDescription);

            _categoryCRUD.CreateCategory(newCat);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /CategoryCatalog/Update
    [HttpPost]
    [Route("Update")]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id, string categoryName, string categoryDescription)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            var categoryToUpdate = new Category();
            
            // Using Reflection to set the private _categoryid field
            typeof(Category).GetField("_categoryid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(categoryToUpdate, id); 

            categoryToUpdate.SetName(categoryName);
            categoryToUpdate.SetDescription(categoryDescription);

            _categoryCRUD.UpdateCategory(categoryToUpdate);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /CategoryCatalog/Delete
    [HttpPost]
    [Route("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _categoryCRUD.DeleteCategory(id);
        return RedirectToAction(nameof(Index));
    }
}