using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class InventoryManagementControl : iInventoryCRUDControl, iInventoryQueryControl, iInventoryStatusControl, iStockSubject
{
    private readonly IInventoryItemMapper _inventoryItemMapper;
    // SRP FIX: We only inject the specific Status Control interface, no more deep CRUD/Query access
    private readonly IProductStatusControl _productStatusControl; 
    private readonly List<iStockObserver> _observers = new();

    public InventoryManagementControl(
        IInventoryItemMapper inventoryItemMapper, 
        IEnumerable<iStockObserver> observers, 
        IProductStatusControl productStatusControl)
    {
        _inventoryItemMapper = inventoryItemMapper ?? throw new ArgumentNullException(nameof(inventoryItemMapper));
        _productStatusControl = productStatusControl ?? throw new ArgumentNullException(nameof(productStatusControl));
        _observers = observers.ToList();
    }

    // ─────────────────────────────────────────────────────────────────────────────────
    // CRUD Methods
    // ─────────────────────────────────────────────────────────────────────────────────

    public bool CreateInventoryItem(int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate)
    {
        var inventoryItem = Inventoryitem.Create(productId, serialNumber, status, expiryDate);

        if (!ValidateInventoryItem(inventoryItem) || !CheckInventoryConflicts(inventoryItem))
            return false;

        try
        {
            _inventoryItemMapper.Insert(inventoryItem);
            SyncProductStockLevels(productId); // SRP FIX
            return true;
        }
        catch { return false; }
    }

    public int CreateBulkInventoryItems(int productId, int quantity)
    {
        if (productId <= 0 || quantity <= 0 || quantity > 100) return 0;

        int createdCount = 0;
        try
        {
            for (int i = 0; i < quantity; i++)
            {
                var inventoryItem = Inventoryitem.CreateReserved(productId);
                try
                {
                    _inventoryItemMapper.Insert(inventoryItem);
                    createdCount++;
                }
                catch { /* Handle individual failure */ }
            }

            if (createdCount > 0)
            {
                SyncProductStockLevels(productId); // SRP FIX
            }
            return createdCount;
        }
        catch { return createdCount; }
    }

    public Inventoryitem? GetInventoryItemById(int inventoryItemId)
    {
        return _inventoryItemMapper.FindById(inventoryItemId);
    }

    public bool UpdateInventoryItem(int inventoryItemId, int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate)
    {
        var existingItem = _inventoryItemMapper.FindById(inventoryItemId);
        if (existingItem is null) return false;

        existingItem.Update(productId, serialNumber, status, expiryDate);

        if (!ValidateInventoryItem(existingItem) || !CheckInventoryConflicts(existingItem))
            return false;

        try
        {
            _inventoryItemMapper.Update(existingItem);
            SyncProductStockLevels(productId); // SRP FIX
            return true;
        }
        catch { return false; }
    }

    public bool DeleteInventoryItem(int inventoryItemId)
    {
        var existingItem = _inventoryItemMapper.FindById(inventoryItemId);
        if (existingItem is null) return false;

        var productId = existingItem.GetProductId();

        try
        {
            _inventoryItemMapper.Delete(existingItem);
            SyncProductStockLevels(productId); // SRP FIX
            return true;
        }
        catch { return false; }
    }

    // ─────────────────────────────────────────────────────────────────────────────────
    // Query/Read Methods
    // ─────────────────────────────────────────────────────────────────────────────────

    public List<Inventoryitem> GetInventoryByProduct(int productId)
    {
        return _inventoryItemMapper.FindByProductId(productId)?.ToList() ?? new List<Inventoryitem>();
    }

    public List<Inventoryitem> GetInventoryByStatus(InventoryStatus status)
    {
        return _inventoryItemMapper.FindByStatus(status)?.ToList() ?? new List<Inventoryitem>();
    }

    public List<Inventoryitem> GetAllInventoryItems()
    {
        var items = _inventoryItemMapper.FindAll()?.ToList() ?? new List<Inventoryitem>();
        return items.OrderByDescending(i => i.GetStatus() == InventoryStatus.RESERVED).ToList();
    }

    public List<Inventoryitem> SearchInventoryItems(string query)
    {
        var allItems = _inventoryItemMapper.FindAll();
        if (allItems is null || string.IsNullOrWhiteSpace(query))
        {
            var items = allItems?.ToList() ?? new List<Inventoryitem>();
            return items.OrderByDescending(i => i.GetStatus() == InventoryStatus.RESERVED).ToList();
        }

        var normalized = query.Trim().ToUpper();

        return allItems.Where(item =>
            item.GetSerialNumber().ToUpper().Contains(normalized))
        .OrderByDescending(i => i.GetStatus() == InventoryStatus.RESERVED)
        .ToList();
    }

    public int CheckProductQuantityByStatus(int productId, InventoryStatus status)
    {
        var items = _inventoryItemMapper.FindByProductId(productId);
        if (items is null) return 0;
        return items.Count(item => item.GetStatus() == status);
    }

    // ─────────────────────────────────────────────────────────────────────────────────
    // Business Logic Methods
    // ─────────────────────────────────────────────────────────────────────────────────

    public bool UpdateInventoryStatus(int inventoryItemId, InventoryStatus status)
    {
        var item = _inventoryItemMapper.FindById(inventoryItemId);
        if (item is null) return false;

        if (item.GetSerialNumber().Contains("TEMP")) return false;

        item.UpdateStatusAndDate(status);

        try
        {
            _inventoryItemMapper.Update(item);
            
            // SRP FIX: We no longer manipulate the Product entity here! 
            // We just update the DB, and let the sync method pass the message.
            SyncProductStockLevels(item.GetProductId());
            
            return true;
        }
        catch { return false; }
    }

    public List<int> AllocateAvailableItems(int productId, int quantity)
    {
        var availableItems = GetInventoryByProduct(productId)
            .Where(item => item.GetStatus() == InventoryStatus.AVAILABLE)
            .Take(quantity)
            .ToList();

        return availableItems.Select(item => item.GetInventoryItemId()).ToList();
    }

    // ─────────────────────────────────────────────────────────────────────────────────
    // Validation Methods (Private)
    // ─────────────────────────────────────────────────────────────────────────────────

    private bool ValidateInventoryItem(Inventoryitem inventoryItem)
    {
        if (inventoryItem is null) return false;
        if (inventoryItem.GetInventoryItemId() < 0) return false;
        if (inventoryItem.GetProductId() <= 0) return false;

        var serialNumber = inventoryItem.GetSerialNumber();
        if (string.IsNullOrWhiteSpace(serialNumber) || serialNumber.Length > 255) return false;

        var status = inventoryItem.GetStatus();
        if (status.HasValue && !Enum.IsDefined(typeof(InventoryStatus), status.Value)) return false;

        var createdDate = inventoryItem.GetCreatedDate();
        var updatedDate = inventoryItem.GetUpdatedDate();
        var expiryDate = inventoryItem.GetExpiryDate();

        if (expiryDate.HasValue && createdDate != default && expiryDate.Value < createdDate) return false;
        if (createdDate != default && updatedDate != default && updatedDate < createdDate) return false;

        return true;
    }

    private bool CheckInventoryConflicts(Inventoryitem inventoryItem)
    {
        if (inventoryItem is null) return false;

        var serialNumber = inventoryItem.GetSerialNumber();
        
        if (inventoryItem.GetStatus() == InventoryStatus.RESERVED && string.IsNullOrWhiteSpace(serialNumber))
            return true;

        if (string.IsNullOrWhiteSpace(serialNumber)) return false;

        var allItems = _inventoryItemMapper.FindAll();
        if (allItems is null) return true;

        var hasDuplicateSerial = allItems.Any(item =>
            item.GetInventoryItemId() != inventoryItem.GetInventoryItemId() &&
            string.Equals(item.GetSerialNumber(), inventoryItem.GetSerialNumber(), StringComparison.OrdinalIgnoreCase));

        return !hasDuplicateSerial;
    }

    // ─────────────────────────────────────────────────────────────────────────────────
    // Observer Pattern Methods
    // ─────────────────────────────────────────────────────────────────────────────────

    public void AttachObserver(iStockObserver observer)
    {
        if (observer is null || _observers.Contains(observer)) return;
        _observers.Add(observer);
    }

    public void RemoveObserver(iStockObserver observer)
    {
        if (observer is null) return;
        _observers.Remove(observer);
    }

    void iStockSubject.NotifyObservers(int productId, int availableCount)
    {
        NotifyObservers(productId, availableCount);
    }

    protected void NotifyObservers(int productId, int availableCount)
    {
        foreach (var observer in _observers)
        {
            observer.Update(productId, availableCount);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────────
    // SRP FIX: The Master Sync Method
    // ─────────────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Calculates physical quantities and passes the message to Product Catalog via interface.
    /// </summary>
    private void SyncProductStockLevels(int productId)
    {
        try
        {
            var items = _inventoryItemMapper.FindByProductId(productId);
            if (items is null) return;

            // 1. Calculate Available Count
            int availableCount = items.Count(item => item.GetStatus() == InventoryStatus.AVAILABLE);

            // 2. Calculate Total Active Count (User's Exact Definition)
            int activeInventoryCount = items.Count(item =>
                item.GetStatus() == InventoryStatus.AVAILABLE ||
                item.GetStatus() == InventoryStatus.ON_LOAN ||
                item.GetStatus() == InventoryStatus.RESERVED ||
                item.GetStatus() == InventoryStatus.MAINTENANCE);

            // 3. Delegate to Product Catalog to apply its own business rules
            _productStatusControl.SyncProductStock(productId, availableCount, activeInventoryCount);
            
            // 4. Notify Observers (Low Stock Alert)
            NotifyObservers(productId, availableCount);
        }
        catch
        {
            // Log failure silently to avoid disrupting inventory operations
        }
    }
}