using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface IProductBulkCommand
{
	bool BulkCreateProducts(ICollection<Product> products, ICollection<Productdetail> details);
	bool BulkUpdateProducts(ICollection<Product> products, ICollection<Productdetail> details);
	bool BulkDeleteProducts(ICollection<int> productIds);
}