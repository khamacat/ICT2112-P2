using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

[StaffAuth]
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

    [HttpGet]
    public IActionResult Create()
    {
        PopulateCategoryDropdown();
        SeedFormValues();
        return View("~/Views/Module2/ProductCatalog/Create.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(
        int CategoryId,
        string Sku,
        ProductStatus Status,
        decimal Threshold,
        string Name,
        string? Description,
        int TotalQuantity,
        decimal? Weight,
        string? Image,
        decimal Price,
        decimal? DepositRate)
    {
        ValidatePostedFields(CategoryId, Sku, Threshold, Name, TotalQuantity, Weight, Price, DepositRate);

        if (!ModelState.IsValid)
        {
            PopulateCategoryDropdown(CategoryId);
            SeedFormValues(
                categoryId: CategoryId,
                sku: Sku,
                status: Status,
                threshold: Threshold,
                name: Name,
                description: Description,
                totalQuantity: TotalQuantity,
                weight: Weight,
                image: Image,
                price: Price,
                depositRate: DepositRate);
            return View("~/Views/Module2/ProductCatalog/Create.cshtml");
        }

        var (product, detail) = BuildDomainObjects(
            productId: null,
            categoryId: CategoryId,
            sku: Sku,
            status: Status,
            thresholdPercent: Threshold,
            name: Name,
            description: Description,
            totalQuantity: TotalQuantity,
            weight: Weight,
            image: Image,
            price: Price,
            depositRate: DepositRate);

        var success = _productCRUD.CreateProduct(product, detail);

        if (!success)
        {
            ModelState.AddModelError(string.Empty,
                "Could not create product. The SKU may already exist, the category may be invalid, or some fields are invalid.");
            PopulateCategoryDropdown(CategoryId);
            SeedFormValues(
                categoryId: CategoryId,
                sku: Sku,
                status: Status,
                threshold: Threshold,
                name: Name,
                description: Description,
                totalQuantity: TotalQuantity,
                weight: Weight,
                image: Image,
                price: Price,
                depositRate: DepositRate);
            return View("~/Views/Module2/ProductCatalog/Create.cshtml");
        }

        TempData["SuccessMessage"] = $"Product '{Name}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _productQuery.GetProductById(id);
        if (product == null) return NotFound();

        var detail = product.GetProductdetail();

        PopulateCategoryDropdown(product.GetCategoryId());
        SeedFormValues(
            productId: product.GetProductId(),
            categoryId: product.GetCategoryId(),
            sku: product.GetSku(),
            status: product.GetStatus(),
            threshold: product.GetThreshold() * 100m,
            name: detail?.GetName() ?? string.Empty,
            description: detail?.GetDescription(),
            totalQuantity: detail?.GetTotalQuantity() ?? 0,
            weight: detail?.GetWeight(),
            image: detail?.GetImage(),
            price: detail?.GetPrice() ?? 0m,
            depositRate: detail?.GetDepositRate());

        return View("~/Views/Module2/ProductCatalog/Edit.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(
        int ProductId,
        int CategoryId,
        string Sku,
        ProductStatus Status,
        decimal Threshold,
        string Name,
        string? Description,
        int TotalQuantity,
        decimal? Weight,
        string? Image,
        decimal Price,
        decimal? DepositRate)
    {
        ValidatePostedFields(CategoryId, Sku, Threshold, Name, TotalQuantity, Weight, Price, DepositRate);

        if (!ModelState.IsValid)
        {
            PopulateCategoryDropdown(CategoryId);
            SeedFormValues(
                productId: ProductId,
                categoryId: CategoryId,
                sku: Sku,
                status: Status,
                threshold: Threshold,
                name: Name,
                description: Description,
                totalQuantity: TotalQuantity,
                weight: Weight,
                image: Image,
                price: Price,
                depositRate: DepositRate);
            return View("~/Views/Module2/ProductCatalog/Edit.cshtml");
        }

        var (product, detail) = BuildDomainObjects(
            productId: ProductId,
            categoryId: CategoryId,
            sku: Sku,
            status: Status,
            thresholdPercent: Threshold,
            name: Name,
            description: Description,
            totalQuantity: TotalQuantity,
            weight: Weight,
            image: Image,
            price: Price,
            depositRate: DepositRate);

        var success = _productCRUD.UpdateProduct(product, detail);

        if (!success)
        {
            ModelState.AddModelError(string.Empty,
                "Could not update product. The product may not exist, the SKU may already exist, or some fields are invalid.");
            PopulateCategoryDropdown(CategoryId);
            SeedFormValues(
                productId: ProductId,
                categoryId: CategoryId,
                sku: Sku,
                status: Status,
                threshold: Threshold,
                name: Name,
                description: Description,
                totalQuantity: TotalQuantity,
                weight: Weight,
                image: Image,
                price: Price,
                depositRate: DepositRate);
            return View("~/Views/Module2/ProductCatalog/Edit.cshtml");
        }

        TempData["SuccessMessage"] = $"Product '{Name}' updated successfully.";
        return RedirectToAction(nameof(Details), new { id = ProductId });
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

    private (Product product, Productdetail detail) BuildDomainObjects(
        int? productId,
        int categoryId,
        string sku,
        ProductStatus status,
        decimal thresholdPercent,
        string name,
        string? description,
        int totalQuantity,
        decimal? weight,
        string? image,
        decimal price,
        decimal? depositRate)
    {
        var product = Product.Create(categoryId, sku, thresholdPercent / 100m, status);

        if (productId.HasValue && productId.Value > 0)
            product.AssignProductId(productId.Value);

        var detail = Productdetail.Create(
            name,
            description,
            totalQuantity,
            weight,
            image,
            price,
            depositRate);

        if (productId.HasValue && productId.Value > 0)
            detail.AssignProductId(productId.Value);

        product.AttachProductdetail(detail);

        return (product, detail);
    }

    private void ValidatePostedFields(
        int categoryId,
        string sku,
        decimal threshold,
        string name,
        int totalQuantity,
        decimal? weight,
        decimal price,
        decimal? depositRate)
    {
        if (categoryId <= 0)
            ModelState.AddModelError("CategoryId", "Please select a category.");

        if (string.IsNullOrWhiteSpace(sku))
            ModelState.AddModelError("Sku", "SKU is required.");
        else if (sku.Length > 255)
            ModelState.AddModelError("Sku", "SKU must not exceed 255 characters.");

        if (threshold < 0 || threshold > 100)
            ModelState.AddModelError("Threshold", "Threshold must be between 0 and 100.");

        if (string.IsNullOrWhiteSpace(name))
            ModelState.AddModelError("Name", "Product name is required.");
        else if (name.Length > 255)
            ModelState.AddModelError("Name", "Product name must not exceed 255 characters.");

        if (totalQuantity < 0)
            ModelState.AddModelError("TotalQuantity", "Total quantity cannot be negative.");

        if (weight.HasValue && weight.Value < 0)
            ModelState.AddModelError("Weight", "Weight cannot be negative.");

        if (price < 0)
            ModelState.AddModelError("Price", "Price must be zero or more.");

        if (depositRate.HasValue && (depositRate.Value < 0 || depositRate.Value > 1))
            ModelState.AddModelError("DepositRate", "Deposit rate must be between 0 and 1.");
    }

    private void PopulateCategoryDropdown(int? selectedId = null)
    {
        var categories = _categoryQuery.GetAllCategories() ?? new List<Category>();

        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.GetCategoryId().ToString(),
            Text = c.GetName(),
            Selected = selectedId.HasValue && selectedId.Value == c.GetCategoryId()
        }).ToList();
    }

    private void SeedFormValues(
        int productId = 0,
        int categoryId = 0,
        string sku = "",
        ProductStatus status = ProductStatus.AVAILABLE,
        decimal threshold = 10m,
        string name = "",
        string? description = null,
        int totalQuantity = 0,
        decimal? weight = null,
        string? image = null,
        decimal price = 0m,
        decimal? depositRate = 0.10m)
    {
        ViewBag.ProductId = productId;
        ViewBag.CategoryId = categoryId;
        ViewBag.Sku = sku;
        ViewBag.Status = status;
        ViewBag.Threshold = threshold;
        ViewBag.Name = name;
        ViewBag.Description = description;
        ViewBag.TotalQuantity = totalQuantity;
        ViewBag.Weight = weight;
        ViewBag.Image = image;
        ViewBag.Price = price;
        ViewBag.DepositRate = depositRate;
    }
}