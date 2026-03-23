using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class InventoryManagementControl : iInventoryCRUDControl, iInventoryQueryControl, iInventoryStatusControl, iStockSubject
{
    private readonly IInventoryItemMapper _inventoryItemMapper;
    private readonly IProductQuery _productQuery;
    private readonly IProductCRUD _productCRUD;
    private readonly List<iStockObserver> _observers = new();

    public InventoryManagementControl(IInventoryItemMapper inventoryItemMapper, IEnumerable<iStockObserver> observers, IProductQuery productQuery, IProductCRUD productCRUD)
    {
        _inventoryItemMapper = inventoryItemMapper ?? throw new ArgumentNullException(nameof(inventoryItemMapper));
        _productQuery = productQuery ?? throw new ArgumentNullException(nameof(productQuery));
        _productCRUD = productCRUD ?? throw new ArgumentNullException(nameof(productCRUD));
        _observers = observers.ToList();
    }

    // ─────────────────────────────────────────────────────────────────────────────────
    // CRUD Methods
    // ─────────────────────────────────────────────────────────────────────────────────

    public bool CreateInventoryItem(int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate)
    {
        var inventoryItem = new Inventoryitem();
        inventoryItem.SetProductId(productId);
        inventoryItem.SetSerialNumber(serialNumber);
        inventoryItem.SetStatus(status);
        inventoryItem.SetCreatedDate(DateTime.UtcNow);
        inventoryItem.SetUpdatedDate(DateTime.UtcNow);
        inventoryItem.SetExpiryDate(expiryDate);

        if (!ValidateInventoryItem(inventoryItem) || !CheckInventoryConflicts(inventoryItem))
        {
            return false;
        }

        try
        {
            _inventoryItemMapper.Insert(inventoryItem);
            int availableCount = CheckProductQuantityByStatus(productId, InventoryStatus.AVAILABLE);
            NotifyObservers(productId, availableCount);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public int CreateBulkInventoryItems(int productId, int quantity)
    {
        if (productId <= 0 || quantity <= 0 || quantity > 100)
        {
            Console.WriteLine($"CreateBulkInventoryItems: Validation failed - ProductId={productId}, Quantity={quantity}");
            return 0;
        }

        int createdCount = 0;

        try
        {
            Console.WriteLine($"CreateBulkInventoryItems: Starting creation of {quantity} items for ProductId={productId}");
            
            for (int i = 0; i < quantity; i++)
            {
                var inventoryItem = new Inventoryitem();
                inventoryItem.SetProductId(productId);
                // Use temp serial number with UUID to ensure uniqueness for bulk items
                inventoryItem.SetSerialNumber($"TEMP-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}");
                inventoryItem.SetStatus(InventoryStatus.RESERVED);  // Mark as reserved/incomplete
                inventoryItem.SetCreatedDate(DateTime.UtcNow);
                inventoryItem.SetUpdatedDate(DateTime.UtcNow);
                inventoryItem.SetExpiryDate(null);

                try
                {
                    _inventoryItemMapper.Insert(inventoryItem);
                    createdCount++;
                    Console.WriteLine($"  Item {i + 1}/{quantity} created successfully with serial: {inventoryItem.GetSerialNumber()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Item {i + 1}/{quantity} FAILED: {ex.GetType().Name} - {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"    Inner Exception: {ex.InnerException.Message}");
                    }
                }
            }

            Console.WriteLine($"CreateBulkInventoryItems: Completed - {createdCount}/{quantity} items created");

            if (createdCount > 0)
            {
                int availableCount = CheckProductQuantityByStatus(productId, InventoryStatus.AVAILABLE);
                NotifyObservers(productId, availableCount);
            }

            return createdCount;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CreateBulkInventoryItems: CRITICAL ERROR - {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
            }
            return createdCount;
        }
    }

    public Inventoryitem? GetInventoryItemById(int inventoryItemId)
    {
        return _inventoryItemMapper.FindById(inventoryItemId);
    }

    public bool UpdateInventoryItem(int inventoryItemId, int productId, string serialNumber, InventoryStatus status, DateTime? expiryDate)
    {
        var existingItem = _inventoryItemMapper.FindById(inventoryItemId);
        if (existingItem is null)
        {
            return false;
        }

        existingItem.SetProductId(productId);
        existingItem.SetSerialNumber(serialNumber);
        existingItem.SetStatus(status);
        existingItem.SetExpiryDate(expiryDate);
        existingItem.SetUpdatedDate(DateTime.UtcNow);

        if (!ValidateInventoryItem(existingItem) || !CheckInventoryConflicts(existingItem))
        {
            return false;
        }

        try
        {
            _inventoryItemMapper.Update(existingItem);
            int availableCount = CheckProductQuantityByStatus(productId, InventoryStatus.AVAILABLE);
            NotifyObservers(productId, availableCount);
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
            Console.WriteLine($"DeleteInventoryItem: Item {inventoryItemId} not found");
            return false;
        }

        var productId = existingItem.GetProductId();
        var serialNumber = existingItem.GetSerialNumber();

        try
        {
            Console.WriteLine($"DeleteInventoryItem: Deleting item {inventoryItemId} (ProductId={productId}, Serial={serialNumber}, Status={existingItem.GetStatus()})");
            _inventoryItemMapper.Delete(existingItem);
            int availableCount = CheckProductQuantityByStatus(productId, InventoryStatus.AVAILABLE);
            NotifyObservers(productId, availableCount);
            Console.WriteLine($"DeleteInventoryItem: Item {inventoryItemId} deleted successfully");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DeleteInventoryItem: FAILED to delete item {inventoryItemId}: {ex.GetType().Name} - {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
            }
            return false;
        }
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
        // Sort to show RESERVED items at top
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

        var results = allItems.Where(item =>
            item.GetSerialNumber().ToUpper().Contains(normalized))
        .OrderByDescending(i => i.GetStatus() == InventoryStatus.RESERVED)
        .ToList();

        return results;
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

    // ─────────────────────────────────────────────────────────────────────────────────
    // Business Logic Methods
    // ─────────────────────────────────────────────────────────────────────────────────

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
            int productId = item.GetProductId();
            
            // Check available quantity for this product
            int availableCount = CheckProductQuantityByStatus(productId, InventoryStatus.AVAILABLE);
            
            // Sync product status based on availability
            var product = _productQuery.GetProductById(productId);
            if (product is not null)
            {
                ProductStatus newProductStatus = availableCount == 0 
                    ? ProductStatus.UNAVAILABLE 
                    : ProductStatus.AVAILABLE;

                // Only update product if status change is needed
                if (product.GetStatus() != newProductStatus)
                {
                    product.UpdateStatus(newProductStatus);
                    if (product.Productdetail is not null)
                    {
                        _productCRUD.UpdateProduct(product, product.Productdetail);
                    }
                }
            }

            NotifyObservers(productId, availableCount);
            return true;
        }
        catch
        {
            return false;
        }
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

    private bool CheckInventoryConflicts(Inventoryitem inventoryItem)
    {
        if (inventoryItem is null)
        {
            return false;
        }

        var serialNumber = inventoryItem.GetSerialNumber();
        
        // RESERVED items with empty serial numbers bypass duplicate check
        if (inventoryItem.GetStatus() == InventoryStatus.RESERVED && string.IsNullOrWhiteSpace(serialNumber))
        {
            return true;
        }

        // For other items, serial number is required
        if (string.IsNullOrWhiteSpace(serialNumber))
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

    // ─────────────────────────────────────────────────────────────────────────────────
    // Observer Pattern Methods
    // ─────────────────────────────────────────────────────────────────────────────────

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

    public void NotifyObservers(int productId, int availableCount)
    {
        foreach (var observer in _observers)
        {
            observer.Update(productId, availableCount);
        }
    }
}