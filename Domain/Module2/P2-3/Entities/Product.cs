using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Product
{
	public ProductStatus Status { get; private set; }
}