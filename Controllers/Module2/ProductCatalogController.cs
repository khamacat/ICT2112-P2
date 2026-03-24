using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

public class ProductCatalogController : Controller
{
    private readonly IProductCRUD _productCRUD;
    private readonly IProductQuery _productQuery;
    private readonly ICategoryQuery _categoryQuery;

    public ProductCatalogController(
        IProductCRUD productCRUD,
        IProductQuery productQuery,
        ICategoryQuery categoryQuery)
    {
        _productCRUD = productCRUD;
        _productQuery = productQuery;
        _categoryQuery = categoryQuery;
    }

    public IActionResult Index(string? search, string? searchField, string? sortField, string? sortDir, int page = 1)
    {
        const int pageSize = 10;

        ICollection<Product>? products;

        if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(searchField))
        {
            products = _productQuery.SearchProducts(searchField, search);
        }
        else if (!string.IsNullOrWhiteSpace(sortField))
        {
            products = _productQuery.SortProducts(sortField, sortDir ?? "asc");
        }
        else
        {
            products = _productQuery.GetAllProducts();
        }

        var list = products?.ToList() ?? new List<Product>();
        var totalCount = list.Count;
        var paged = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewBag.Search = search;
        ViewBag.SearchField = searchField;
        ViewBag.SortField = sortField;
        ViewBag.SortDir = sortDir;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return View("~/Views/Module2/ProductCatalog/Index.cshtml", paged);
    }

    public IActionResult Details(int id)
    {
        var product = _productQuery.GetProductById(id);
        if (product == null) return NotFound();

        return View("~/Views/Module2/ProductCatalog/Details.cshtml", product);
    }

    public IActionResult Create()
    {
        PopulateCategoryDropdown();
        return View("~/Views/Module2/ProductCatalog/Create.cshtml", new ProductFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            PopulateCategoryDropdown(vm.CategoryId);
            return View("~/Views/Module2/ProductCatalog/Create.cshtml", vm);
        }

        var (product, detail) = vm.ToDomain();
        var success = _productCRUD.CreateProduct(product, detail);

        if (!success)
        {
            ModelState.AddModelError(string.Empty,
                "Could not create product. The SKU may already exist, the category may be invalid, or some fields are invalid.");
            PopulateCategoryDropdown(vm.CategoryId);
            return View("~/Views/Module2/ProductCatalog/Create.cshtml", vm);
        }

        TempData["SuccessMessage"] = $"Product '{vm.Name}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var product = _productQuery.GetProductById(id);
        if (product == null) return NotFound();

        var vm = ProductFormViewModel.FromDomain(product);
        PopulateCategoryDropdown(vm.CategoryId);
        return View("~/Views/Module2/ProductCatalog/Edit.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, ProductFormViewModel vm)
    {
        if (id != vm.ProductId) return BadRequest();

        if (!ModelState.IsValid)
        {
            PopulateCategoryDropdown(vm.CategoryId);
            return View("~/Views/Module2/ProductCatalog/Edit.cshtml", vm);
        }

        var (product, detail) = vm.ToDomain();
        var success = _productCRUD.UpdateProduct(product, detail);

        if (!success)
        {
            ModelState.AddModelError(string.Empty,
                "Could not update product. The product may not exist, the SKU may already exist, or some fields are invalid.");
            PopulateCategoryDropdown(vm.CategoryId);
            return View("~/Views/Module2/ProductCatalog/Edit.cshtml", vm);
        }

        TempData["SuccessMessage"] = $"Product '{vm.Name}' updated successfully.";
        return RedirectToAction(nameof(Details), new { id = vm.ProductId });
    }

    public IActionResult Delete(int id)
    {
        var product = _productQuery.GetProductById(id);
        if (product == null) return NotFound();

        return View("~/Views/Module2/ProductCatalog/Delete.cshtml", product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var success = _productCRUD.DeleteProduct(id);

        if (!success)
        {
            TempData["ErrorMessage"] = "Could not delete product because it is still being used elsewhere.";
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }


// need to edit this once category is implemented
    private void PopulateCategoryDropdown(int? selectedId = null)
{
    var categories = _categoryQuery.GetAllCategories() ?? new List<Category>();

    var items = categories.Select(c => new SelectListItem
    {
        Value = c.GetCategoryId().ToString(),
        Text = c.GetName(),
        Selected = selectedId.HasValue && selectedId.Value == c.GetCategoryId()
    }).ToList();

    ViewBag.Categories = items;
}
}

public class ProductFormViewModel
{
    public int ProductId { get; set; }

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please select a category.")]
    [System.ComponentModel.DataAnnotations.Display(Name = "Category")]
    public int CategoryId { get; set; }

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "SKU is required.")]
    [System.ComponentModel.DataAnnotations.StringLength(255)]
    [System.ComponentModel.DataAnnotations.Display(Name = "SKU")]
    public string Sku { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Display(Name = "Status")]
    public ProductStatus Status { get; set; } = ProductStatus.AVAILABLE;

    [System.ComponentModel.DataAnnotations.Range(0, 100, ErrorMessage = "Threshold must be between 0 and 1.")]
    [System.ComponentModel.DataAnnotations.Display(Name = "Low Stock Threshold (%)")]
    public decimal Threshold { get; set; } = 10m;

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product name is required.")]
    [System.ComponentModel.DataAnnotations.StringLength(255)]
    [System.ComponentModel.DataAnnotations.Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Display(Name = "Description")]
    public string? Description { get; set; }

    [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue, ErrorMessage = "Total quantity cannot be negative.")]
    [System.ComponentModel.DataAnnotations.Display(Name = "Total Quantity")]
    public int TotalQuantity { get; set; }

    [System.ComponentModel.DataAnnotations.Range(0, double.MaxValue, ErrorMessage = "Weight cannot be negative.")]
    [System.ComponentModel.DataAnnotations.Display(Name = "Weight (kg)")]
    public decimal? Weight { get; set; }

    [System.ComponentModel.DataAnnotations.Display(Name = "Image Filename")]
    public string? Image { get; set; }

    [System.ComponentModel.DataAnnotations.Range(0, double.MaxValue, ErrorMessage = "Price must be zero or more.")]
    [System.ComponentModel.DataAnnotations.Display(Name = "Daily Rental Price (S$)")]
    public decimal Price { get; set; }

    [System.ComponentModel.DataAnnotations.Range(0, 1, ErrorMessage = "Deposit rate must be between 0 and 1.")]
    [System.ComponentModel.DataAnnotations.Display(Name = "Deposit Rate")]
    public decimal? DepositRate { get; set; } = 0.10m;

public (Product product, Productdetail detail) ToDomain()
{
    var product = Product.Create(CategoryId, Sku, Threshold / 100m, Status);

    if (ProductId > 0)
        product.AssignProductId(ProductId);

    var detail = Productdetail.Create(
        Name,
        Description,
        TotalQuantity,
        Weight,
        Image,
        Price,
        DepositRate);

    if (ProductId > 0)
        detail.AssignProductId(ProductId);

    product.AttachProductdetail(detail);

    return (product, detail);
}
    public static ProductFormViewModel FromDomain(Product product)
    {
        var detail = product.GetProductdetail();

        return new ProductFormViewModel
        {
            ProductId = product.GetProductId(),
            CategoryId = product.GetCategoryId(),
            Sku = product.GetSku(),
            Status = product.GetStatus(),
            Threshold = product.GetThreshold() * 100m, // convert back to percentage for the form
            Name = detail?.GetName() ?? string.Empty,
            Description = detail?.GetDescription(),
            TotalQuantity = detail?.GetTotalQuantity() ?? 0,
            Weight = detail?.GetWeight(),
            Image = detail?.GetImage(),
            Price = detail?.GetPrice() ?? 0m,
            DepositRate = detail?.GetDepositRate()
        };
    }
}