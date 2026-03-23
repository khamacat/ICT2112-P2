using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

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