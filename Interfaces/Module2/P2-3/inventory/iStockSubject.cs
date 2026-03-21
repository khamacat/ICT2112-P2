namespace ProRental.Interfaces.Module2.P2_3;

public interface iStockSubject
{
    void AttachObserver(iStockObserver observer);
    void RemoveObserver(iStockObserver observer);
    void NotifyObservers(int productId);
}
