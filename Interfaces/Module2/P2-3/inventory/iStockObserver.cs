namespace ProRental.Interfaces.Domain;

public interface iStockObserver
{
    void Update(int productId, int availableCount);
}
