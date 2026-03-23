namespace ProRental.Interfaces.Domain;

public interface iStockSubject
{
    void AttachObserver(iStockObserver observer);
    void RemoveObserver(iStockObserver observer);
    void NotifyObservers(int productId, int availableCount);
}
