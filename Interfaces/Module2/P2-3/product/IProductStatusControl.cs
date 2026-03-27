using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface IProductActions
{
    int GetThresholdQuantityForProduct(int productId);
    bool SyncProductStock(int productId, int availableQuantity, int totalActiveQuantity);
    bool AddToProduct(int productId, int quantity);
}