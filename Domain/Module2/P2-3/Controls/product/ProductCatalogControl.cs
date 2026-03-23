using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ProductCatalogControl : IProductCRUD, IProductQuery, IProductBulkCommand, IProductStatusControl
{
    private readonly IProductMapper _productMapper;
    private readonly ICategoryQuery _categoryQuery;

    public ProductCatalogControl(
        IProductMapper productMapper,
        ICategoryQuery categoryQuery)
    {
        _productMapper = productMapper;
        _categoryQuery = categoryQuery;
    }

    public bool CreateProduct(Product product, Productdetail detail)
    {
        if (!ValidateProduct(product, detail)) return false;
        if (CheckProductConflicts(product)) return false;

        var category = _categoryQuery.GetCategoryById(product.GetCategoryId());
        if (category == null) return false;

        product.AttachProductdetail(detail);
        _productMapper.Insert(product);
        return true;
    }

    public bool UpdateProduct(Product product, Productdetail detail)
    {
        if (!ValidateProduct(product, detail)) return false;

        var existing = _productMapper.FindById(product.GetProductId());
        if (existing == null) return false;

        var category = _categoryQuery.GetCategoryById(product.GetCategoryId());
        if (category == null) return false;

        if (CheckProductConflicts(product)) return false;

        detail.AssignProductId(product.GetProductId());
        product.AttachProductdetail(detail);

        _productMapper.Update(product);
        return true;
    }

    public bool DeleteProduct(int productId)
    {
        var existing = _productMapper.FindById(productId);
        if (existing == null) return false;

        try
        {
            _productMapper.Delete(existing);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ValidateProduct(Product product, Productdetail detail)
    {
        if (product.GetCategoryId() <= 0) return false;
        if (string.IsNullOrWhiteSpace(product.GetSku())) return false;
        if (product.GetSku().Length > 255) return false;
        if (product.GetThreshold() < 0 || product.GetThreshold() > 1) return false;

        if (string.IsNullOrWhiteSpace(detail.GetName())) return false;
        if (detail.GetName().Length > 255) return false;
        if (detail.GetTotalQuantity() < 0) return false;
        if (detail.GetPrice() < 0) return false;
        if (detail.GetWeight().HasValue && detail.GetWeight()!.Value < 0) return false;

        if (detail.GetDepositRate().HasValue &&
            (detail.GetDepositRate()!.Value < 0 || detail.GetDepositRate()!.Value > 1))
        {
            return false;
        }

        return true;
    }

    public bool CheckProductConflicts(Product product)
    {
        var allProducts = _productMapper.FindAll();
        if (allProducts == null) return false;

        var productId = product.GetProductId();
        var sku = product.GetSku().Trim();

        return allProducts.Any(p =>
            p.GetProductId() != productId &&
            string.Equals(p.GetSku(), sku, StringComparison.OrdinalIgnoreCase));
    }

    public Product? GetProductById(int productId)
    {
        return _productMapper.FindById(productId);
    }

    public ICollection<Product>? GetAllProducts()
    {
        return _productMapper.FindAll();
    }

    public ICollection<Product>? GetProductsByCategoryId(int categoryId)
    {
        return _productMapper.FindByCategoryId(categoryId);
    }

    public ICollection<Product>? GetProductsByStatus(ProductStatus status)
    {
        return _productMapper.FindByStatus(status);
    }

    public ICollection<Product>? SearchProducts(string searchField, string value)
    {
        var allProducts = _productMapper.FindAll();
        if (allProducts == null) return null;
        if (string.IsNullOrWhiteSpace(value)) return allProducts;

        var normalizedField = (searchField ?? string.Empty).Trim().ToLowerInvariant();

        return normalizedField switch
        {
            "sku" => allProducts
                .Where(p => p.GetSku().Contains(value, StringComparison.OrdinalIgnoreCase))
                .ToList(),

            "category" => allProducts
                .Where(p => (p.GetCategoryEntity()?.GetName() ?? string.Empty)
                .Contains(value, StringComparison.OrdinalIgnoreCase))
                .ToList(),

            "status" => Enum.TryParse<ProductStatus>(value, true, out var parsedStatus)
                ? allProducts.Where(p => p.GetStatus() == parsedStatus).ToList()
                : new List<Product>(),

            _ => allProducts
        };
    }

    public ICollection<Product>? SortProducts(string sortField, string direction)
    {
        var allProducts = _productMapper.FindAll();
        if (allProducts == null) return null;

        var normalizedField = (sortField ?? string.Empty).Trim().ToLowerInvariant();
        var isDescending = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);

        return normalizedField switch
        {
            "sku" => isDescending
                ? allProducts.OrderByDescending(p => p.GetSku()).ToList()
                : allProducts.OrderBy(p => p.GetSku()).ToList(),

            "createdat" => isDescending
                ? allProducts.OrderByDescending(p => p.GetCreatedAt()).ToList()
                : allProducts.OrderBy(p => p.GetCreatedAt()).ToList(),

            "threshold" => isDescending
                ? allProducts.OrderByDescending(p => p.GetThreshold()).ToList()
                : allProducts.OrderBy(p => p.GetThreshold()).ToList(),

            _ => isDescending
                ? allProducts.OrderByDescending(p => p.GetProductId()).ToList()
                : allProducts.OrderBy(p => p.GetProductId()).ToList()
        };
    }

    public decimal GetThresholdForProduct(int productId)
    {
        var product = _productMapper.FindById(productId);
        if (product == null)
            throw new InvalidOperationException($"Product {productId} not found.");

        return product.GetThreshold();
    }

    public ProductStatus CheckProductStatus(int productId)
    {
        var product = _productMapper.FindById(productId);
        if (product == null)
            throw new InvalidOperationException($"Product {productId} not found.");

        return product.GetStatus();
    }

    public bool BulkCreateProducts(ICollection<Product> products, ICollection<Productdetail> details)
    {
        var productList = products.ToList();
        var detailList = details.ToList();

        if (productList.Count != detailList.Count) return false;

        for (var i = 0; i < productList.Count; i++)
        {
            if (!CreateProduct(productList[i], detailList[i])) return false;
        }

        return true;
    }

    public bool BulkUpdateProducts(ICollection<Product> products, ICollection<Productdetail> details)
    {
        var productList = products.ToList();
        var detailList = details.ToList();

        if (productList.Count != detailList.Count) return false;

        for (var i = 0; i < productList.Count; i++)
        {
            if (!UpdateProduct(productList[i], detailList[i])) return false;
        }

        return true;
    }

    public bool BulkDeleteProducts(ICollection<int> productIds)
    {
        foreach (var productId in productIds)
        {
            if (!DeleteProduct(productId)) return false;
        }

        return true;
    }

    public bool AddToProduct(int productId, int quantity)
{
    if (quantity <= 0) return false;

    var product = _productMapper.FindById(productId);
    if (product == null) return false;

    var detail = product.GetProductdetail();
    if (detail == null) return false;

    detail.IncreaseTotalQuantity(quantity);
    _productMapper.Update(product);

    return true;
}

public bool UpdateProductStatus(int productId, ProductStatus productStatus)
{
    var product = _productMapper.FindById(productId);
    if (product == null) return false;

    product.UpdateStatus(productStatus);
    _productMapper.Update(product);
    return true;
}

public int GetThresholdQuantityForProduct(int productId)
{
    var product = _productMapper.FindById(productId);
    if (product == null)
        throw new InvalidOperationException($"Product {productId} not found.");

    var detail = product.GetProductdetail();
    if (detail == null)
        throw new InvalidOperationException($"Product detail for product {productId} not found.");

    var totalQuantity = detail.GetTotalQuantity();
    var thresholdPercentage = product.GetThreshold();

    return (int)Math.Ceiling(totalQuantity * thresholdPercentage);
}

}