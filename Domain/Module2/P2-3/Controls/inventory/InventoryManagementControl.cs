using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Module2.P2_3;

namespace ProRental.Domain.Module2.P2_3.Controls;

public class InventoryManagementControl : iInventoryCRUDControl, iInventoryQueryControl, iInventoryStatusControl, iStockSubject
{
    private readonly ProRental.Interfaces.Data.IInventoryItemMapper _inventoryItemMapper;
    private readonly List<iStockObserver> _observers = new();

    public InventoryManagementControl(ProRental.Interfaces.Data.IInventoryItemMapper inventoryItemMapper)
    {
        _inventoryItemMapper = inventoryItemMapper ?? throw new ArgumentNullException(nameof(inventoryItemMapper));
    }

    public bool CreateInventoryItem(Inventoryitem inventoryItem)
    {
        if (!ValidateInventoryItem(inventoryItem) || !CheckInventoryConflicts(inventoryItem))
        {
            return false;
        }

        try
        {
            _inventoryItemMapper.Insert(inventoryItem);
            NotifyObservers(inventoryItem.GetProductId());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Inventoryitem? GetInventoryItemById(int inventoryItemId)
    {
        return _inventoryItemMapper.FindById(inventoryItemId);
    }

    public bool UpdateInventoryItem(Inventoryitem inventoryItem)
    {
        if (!ValidateInventoryItem(inventoryItem))
        {
            return false;
        }

        var existingItem = _inventoryItemMapper.FindById(inventoryItem.GetInventoryItemId());
        if (existingItem is null || !CheckInventoryConflicts(inventoryItem))
        {
            return false;
        }

        inventoryItem.SetUpdatedDate(DateTime.UtcNow);

        try
        {
            _inventoryItemMapper.Update(inventoryItem);
            NotifyObservers(inventoryItem.GetProductId());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteInventoryItem(int inventoryItemId)
    {
        var existingItem = _inventoryItemMapper.FindById(inventoryItemId);
        if (existingItem is null)
        {
            return false;
        }

        var productId = existingItem.GetProductId();

        try
        {
            _inventoryItemMapper.Delete(existingItem);
            NotifyObservers(productId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<Inventoryitem> GetInventoryByProduct(int productId)
    {
        return _inventoryItemMapper.FindByProductId(productId)?.ToList() ?? new List<Inventoryitem>();
    }

    public List<Inventoryitem> GetInventoryByStatus(InventoryStatus status)
    {
        return _inventoryItemMapper.FindByStatus(status)?.ToList() ?? new List<Inventoryitem>();
    }

    public int GetTotalStockCount(int productId)
    {
        return _inventoryItemMapper.FindByProductId(productId)?.Count() ?? 0;
    }

    public bool ValidateInventoryItem(Inventoryitem inventoryItem)
    {
        if (inventoryItem is null)
        {
            return false;
        }

        if (inventoryItem.GetInventoryItemId() < 0)
        {
            return false;
        }

        if (inventoryItem.GetProductId() <= 0)
        {
            return false;
        }

        var serialNumber = inventoryItem.GetSerialNumber();
        if (string.IsNullOrWhiteSpace(serialNumber) || serialNumber.Length > 255)
        {
            return false;
        }

        var status = inventoryItem.GetStatus();
        if (status.HasValue && !Enum.IsDefined(typeof(InventoryStatus), status.Value))
        {
            return false;
        }

        var createdDate = inventoryItem.GetCreatedDate();
        var updatedDate = inventoryItem.GetUpdatedDate();
        var expiryDate = inventoryItem.GetExpiryDate();

        if (expiryDate.HasValue && createdDate != default && expiryDate.Value < createdDate)
        {
            return false;
        }

        if (createdDate != default && updatedDate != default && updatedDate < createdDate)
        {
            return false;
        }

        return true;
    }

    public bool CheckInventoryConflicts(Inventoryitem inventoryItem)
    {
        if (inventoryItem is null || string.IsNullOrWhiteSpace(inventoryItem.GetSerialNumber()))
        {
            return false;
        }

        var allItems = _inventoryItemMapper.FindAll();
        if (allItems is null)
        {
            return true;
        }

        var hasDuplicateSerial = allItems.Any(item =>
            item.GetInventoryItemId() != inventoryItem.GetInventoryItemId() &&
            string.Equals(item.GetSerialNumber(), inventoryItem.GetSerialNumber(), StringComparison.OrdinalIgnoreCase));

        return !hasDuplicateSerial;
    }

    public int CheckProductQuantityByStatus(int productId, InventoryStatus status)
    {
        var items = _inventoryItemMapper.FindByProductId(productId);
        if (items is null)
        {
            return 0;
        }

        return items.Count(item => item.GetStatus() == status);
    }

    public bool UpdateInventoryStatus(int inventoryItemId, InventoryStatus status)
    {
        var item = _inventoryItemMapper.FindById(inventoryItemId);
        if (item is null)
        {
            return false;
        }

        item.SetStatus(status);
        item.SetUpdatedDate(DateTime.UtcNow);

        try
        {
            _inventoryItemMapper.Update(item);
            NotifyObservers(item.GetProductId());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void AttachObserver(iStockObserver observer)
    {
        if (observer is null || _observers.Contains(observer))
        {
            return;
        }

        _observers.Add(observer);
    }

    public void RemoveObserver(iStockObserver observer)
    {
        if (observer is null)
        {
            return;
        }

        _observers.Remove(observer);
    }

    public void NotifyObservers(int productId)
    {
        foreach (var observer in _observers)
        {
            observer.Update(productId);
        }
    }
}