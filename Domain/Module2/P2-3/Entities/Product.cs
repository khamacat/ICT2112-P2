using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Product
{
    private ProductStatus _status;

    // EF still needs this CLR property for mapping
    public ProductStatus Status
    {
        get => _status;
        private set => _status = value;
    }

    public static Product Create(int categoryId, string sku, decimal threshold, ProductStatus status)
    {
        var product = new Product();
        product.AssignCategory(categoryId);
        product.UpdateSku(sku);
        product.UpdateThreshold(threshold);
        product.UpdateStatus(status);
        return product;
    }

    public int GetProductId() => _productid;
    public int GetCategoryId() => _categoryid;
    public string GetSku() => _sku;
    public ProductStatus GetStatus() => _status;
    public decimal GetThreshold() => _threshold;
    public DateTime GetCreatedAt() => _createdat;
    public DateTime GetUpdatedAt() => _updatedat;

    public Productdetail? GetProductdetail() => Productdetail;
    public Category? GetCategoryEntity() => Category;

    public void AssignProductId(int productId) => SetProductId(productId);
    public void AssignCategory(int categoryId) => SetCategoryId(categoryId);
    public void UpdateSku(string sku) => SetSku(sku);
    public void UpdateThreshold(decimal threshold) => SetThreshold(threshold);
    public void UpdateStatus(ProductStatus status) => SetStatus(status);
    public void AttachProductdetail(Productdetail? detail) => SetProductdetail(detail);

    private void SetProductId(int productId) => _productid = productId;
    private void SetCategoryId(int categoryId) => _categoryid = categoryId;
    private void SetSku(string sku) => _sku = sku;
    private void SetThreshold(decimal threshold) => _threshold = threshold;
    private void SetStatus(ProductStatus status) => Status = status;
    private void SetProductdetail(Productdetail? detail) => Productdetail = detail;
}