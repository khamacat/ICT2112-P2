using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface IProductStatusControl
{
    bool UpdateProductStatus(int productId, ProductStatus productStatus);
    int GetThresholdQuantityForProduct(int productId);
}