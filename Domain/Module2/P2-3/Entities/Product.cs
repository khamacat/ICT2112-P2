using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Product
{
	private ProductStatus _status;
	private ProductStatus Status { get => _status; set => _status = value; }

	public void UpdateStatus(ProductStatus status)
	{
		_status = status;
	}
}