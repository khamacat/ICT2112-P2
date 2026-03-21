using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Product
{
    private ProductStatus _status;
    private ProductStatus Status { get => _status; set => _status = value; }

    public static Product Create(int categoryId, string sku, decimal threshold, ProductStatus status)
    {
        var product = new Product();
        product.Categoryid = categoryId;
        product.Sku = sku;
        product.Threshold = threshold;
        product.Status = status;
        return product;
    }

    public int GetProductId() => Productid;
    public int GetCategoryId() => Categoryid;
    public string GetSku() => Sku;
    public ProductStatus GetStatus() => Status;
    public decimal GetThreshold() => Threshold;
    public DateTime GetCreatedAt() => Createdat;
    public DateTime GetUpdatedAt() => Updatedat;
    public Productdetail? GetProductdetail() => Productdetail;
    public Category? GetCategoryEntity() => Category;

    public void SetProductId(int productId) => Productid = productId;
    public void SetCategoryId(int categoryId) => Categoryid = categoryId;
    public void SetSku(string sku) => Sku = sku;
    public void SetThreshold(decimal threshold) => Threshold = threshold;
    public void SetStatus(ProductStatus status) => Status = status;
    public void UpdateStatus(ProductStatus status) => Status = status;
    public void SetProductdetail(Productdetail? detail) => Productdetail = detail;
}