using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

// ── Product CRUD ──────────────────────────────────────────────────────────────

public interface IProductCRUD
{
	bool CreateProduct(Product product, Productdetail detail);
	bool UpdateProduct(Product product, Productdetail detail);
	bool DeleteProduct(int productId);
	bool ValidateProduct(Product product, Productdetail detail);
	bool CheckProductConflicts(Product product);

	bool AddToProduct(int productId, int quantity);
}

// ── Product Query ─────────────────────────────────────────────────────────────

public interface IProductQuery
{
	Product? GetProductById(int productId);
	ICollection<Product>? GetAllProducts();
	ICollection<Product>? GetProductsByCategoryId(int categoryId);
	ICollection<Product>? GetProductsByStatus(ProductStatus status);
	ICollection<Product>? SearchProducts(string searchField, string value);
	ICollection<Product>? SortProducts(string sortField, string direction);
	decimal GetThresholdForProduct(int productId);
	ProductStatus CheckProductStatus(int productId);
}

// ── Product Bulk Operations ───────────────────────────────────────────────────

public interface IProductBulkCommand
{
	bool BulkCreateProducts(ICollection<Product> products, ICollection<Productdetail> details);
	bool BulkUpdateProducts(ICollection<Product> products, ICollection<Productdetail> details);
	bool BulkDeleteProducts(ICollection<int> productIds);
}