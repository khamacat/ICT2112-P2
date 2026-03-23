using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface IProductCRUD
{
	bool CreateProduct(Product product, Productdetail detail);
	bool UpdateProduct(Product product, Productdetail detail);
	bool DeleteProduct(int productId);
	bool ValidateProduct(Product product, Productdetail detail);
	bool CheckProductConflicts(Product product);

	bool AddToProduct(int productId, int quantity);
}