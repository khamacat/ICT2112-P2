using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ClearanceItemControl : iClearanceItemQuery, iClearanceItemControl
{
    private readonly IClearanceItemMapper _itemMapper;
    private readonly IClearanceBatchMapper _batchMapper;
    private readonly iInventoryCRUDControl _inventoryCRUD;
    private readonly iInventoryStatusControl _inventoryStatus;
    private readonly IProductQuery _productQuery;

    // Default discount rate for recommended price calculation (30% off retail)
    private const decimal DefaultDiscountRate = 0.30m;

    public ClearanceItemControl(
        IClearanceItemMapper itemMapper,
        IClearanceBatchMapper batchMapper,
        iInventoryCRUDControl inventoryCRUD,
        iInventoryStatusControl inventoryStatus,
        IProductQuery productQuery)
    {
        _itemMapper = itemMapper;
        _batchMapper = batchMapper;
        _inventoryCRUD = inventoryCRUD;
        _inventoryStatus = inventoryStatus;
        _productQuery = productQuery;
    }

    // ── Item CRUD ──────────────────────────────────────────────────────────────

    public bool CreateClearanceItem(Clearanceitem clearanceItem)
    {
        try
        {
            if (!ValidateClearanceItem(clearanceItem))
                return false;

            if (CheckClearanceItemConflict(clearanceItem))
                return false;

            clearanceItem.UpdateStatus(ClearanceStatus.CLEARANCE);
            _itemMapper.Insert(clearanceItem);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Clearanceitem GetClearanceItemById(int clearanceItemId)
    {
        return _itemMapper.FindById(clearanceItemId)!;
    }

    public List<Clearanceitem> GetClearanceItemsByBatch(int batchId)
    {
        var result = _itemMapper.FindByBatchId(batchId);
        return result?.ToList() ?? new List<Clearanceitem>();
    }

    public List<Clearanceitem> GetClearanceItemsByStatus(string status)
    {
        if (!Enum.TryParse<ClearanceStatus>(status, true, out var parsed))
            return new List<Clearanceitem>();

        var result = _itemMapper.FindByStatus(parsed);
        return result?.ToList() ?? new List<Clearanceitem>();
    }

    public bool UpdateClearanceItem(Clearanceitem clearanceItem)
    {
        try
        {
            _itemMapper.Update(clearanceItem);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteClearanceItem(int clearanceItemId)
    {
        try
        {
            var item = _itemMapper.FindById(clearanceItemId);
            if (item == null)
                return false;

            _itemMapper.Delete(item);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ── Item Assignment to Batch ───────────────────────────────────────────────

    public bool AddItemToBatch(int batchId, int inventoryItemId)
    {
        try
        {
            // Verify batch exists
            var batch = _batchMapper.FindById(batchId);
            if (batch == null)
                return false;

            // Verify inventory item exists via InventoryManagementControl
            var inventoryItem = _inventoryCRUD.GetInventoryItemById(inventoryItemId);
            if (inventoryItem == null)
                return false;

            // Check eligibility
            if (!CheckItemEligibility(inventoryItemId))
                return false;

            // Create the clearance item
            var clearanceItem = new Clearanceitem();
            clearanceItem.SetClearanceBatchId(batchId);
            clearanceItem.SetInventoryItemId(inventoryItemId);
            clearanceItem.UpdateStatus(ClearanceStatus.CLEARANCE);

            // Calculate and set recommended price
            decimal recommendedPrice = CalculateRecommendedPriceForInventoryItem(inventoryItemId);
            clearanceItem.SetRecommendedPrice(recommendedPrice);

            // Check for conflicts before inserting
            if (CheckClearanceItemConflict(clearanceItem))
                return false;

            _itemMapper.Insert(clearanceItem);

            // Update inventory item status to CLEARANCE via InventoryManagementControl
            _inventoryStatus.UpdateInventoryStatus(inventoryItemId, InventoryStatus.CLEARANCE);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AddItemsToBatch(int batchId, List<int> inventoryItemIds)
    {
        // Add each item individually — if any fails, the rest still get processed
        bool allSucceeded = true;
        foreach (int inventoryItemId in inventoryItemIds)
        {
            if (!AddItemToBatch(batchId, inventoryItemId))
                allSucceeded = false;
        }
        return allSucceeded;
    }

    public bool RemoveItemFromBatch(int clearanceItemId)
    {
        try
        {
            var item = _itemMapper.FindById(clearanceItemId);
            if (item == null)
                return false;

            // Only allow removal if item hasn't been sold
            if (item.GetStatus() == ClearanceStatus.SOLD)
                return false;

            // Revert inventory item status back to AVAILABLE via InventoryManagementControl
            int inventoryItemId = item.GetInventoryItemId();
            _inventoryStatus.UpdateInventoryStatus(inventoryItemId, InventoryStatus.AVAILABLE);

            _itemMapper.Delete(item);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ── Item Status & Pricing ─────────────────────────────────────────────────

    public bool UpdateClearanceItemStatus(int clearanceItemId, string status)
    {
        try
        {
            if (!Enum.TryParse<ClearanceStatus>(status, true, out var parsed))
                return false;

            var item = _itemMapper.FindById(clearanceItemId);
            if (item == null)
                return false;

            item.UpdateStatus(parsed);
            _itemMapper.Update(item);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public decimal CalculateClearancePrice(int clearanceItemId)
    {
        var item = _itemMapper.FindById(clearanceItemId);
        if (item == null)
            return 0m;

        return CalculateRecommendedPriceForInventoryItem(item.GetInventoryItemId());
    }

    public bool RecordSale(int clearanceItemId, decimal finalPrice, int staffId)
    {
        try
        {
            var item = _itemMapper.FindById(clearanceItemId);
            if (item == null)
                return false;

            // Cannot re-sell an already sold item
            if (item.GetStatus() == ClearanceStatus.SOLD)
                return false;

            // Record sale details
            item.SetFinalPrice(finalPrice);
            item.SetSaleDate(DateTime.UtcNow);
            item.UpdateStatus(ClearanceStatus.SOLD);

            _itemMapper.Update(item);

            // Update inventory item status to SOLD via InventoryManagementControl
            _inventoryStatus.UpdateInventoryStatus(item.GetInventoryItemId(), InventoryStatus.SOLD);

            return true;
        }
        catch
        {
            return false;
        }
    }

    // ── Item Validation ───────────────────────────────────────────────────────

    public bool ValidateClearanceItem(Clearanceitem clearanceItem)
    {
        // Batch ID must be set
        if (clearanceItem.GetClearanceBatchId() <= 0)
            return false;

        // Inventory item ID must be set
        if (clearanceItem.GetInventoryItemId() <= 0)
            return false;

        // Verify the referenced batch exists
        var batch = _batchMapper.FindById(clearanceItem.GetClearanceBatchId());
        if (batch == null)
            return false;

        return true;
    }

    public bool CheckItemEligibility(int inventoryItemId)
    {
        // Verify the inventory item exists via InventoryManagementControl
        var inventoryItem = _inventoryCRUD.GetInventoryItemById(inventoryItemId);
        if (inventoryItem == null)
            return false;

        return true;

        // Check that the item is in AVAILABLE status using public accessor
        var currentStatus = inventoryItem.GetStatus();
        if (currentStatus != InventoryStatus.AVAILABLE)
            return false;

        // Check inactivity: item must have been updated more than 24 months ago
        var lastUpdated = inventoryItem.GetUpdatedDate();
        var inactivityThreshold = DateTime.UtcNow.AddMonths(-24);
        if (lastUpdated > inactivityThreshold)
            return false;

        // Check that item is not already in a clearance batch
        var existingClearanceItem = _itemMapper.FindByInventoryItemId(inventoryItemId);
        if (existingClearanceItem != null)
            return false;

        return true;
    }

    public bool CheckClearanceItemConflict(Clearanceitem clearanceItem)
    {
        // Check if this inventory item is already in another active batch
        var existing = _itemMapper.FindByInventoryItemId(clearanceItem.GetInventoryItemId());
        if (existing == null)
            return false; // No conflict

        // If updating the same clearance item, not a conflict
        if (existing.GetClearanceItemId() == clearanceItem.GetClearanceItemId()
            && clearanceItem.GetClearanceItemId() != 0)
            return false;

        // Check if the existing clearance item's batch is still active or scheduled
        var existingBatch = _batchMapper.FindById(existing.GetClearanceBatchId());
        if (existingBatch != null && existingBatch.GetStatus() != ClearanceBatchStatus.CLOSED)
            return true; // Conflict: item is in a non-closed batch

        return false; // No conflict
    }

    // ── Private Helpers ───────────────────────────────────────────────────────

    private decimal CalculateRecommendedPriceForInventoryItem(int inventoryItemId)
    {
        // Look up the product's retail price via inventory item → product → product detail
        var inventoryItem = _inventoryCRUD.GetInventoryItemById(inventoryItemId);
        if (inventoryItem == null)
            return 0m;

        int productId = inventoryItem.GetProductId();

        // Query product via IProductQuery (includes Productdetail composite)
        var product = _productQuery.GetProductById(productId);
        if (product == null || product.Productdetail == null)
            return 0m;

        decimal retailPrice = product.Productdetail.GetPrice();

        // Apply the clearance discount (30% off retail price)
        return Math.Round(retailPrice * (1 - DefaultDiscountRate), 2);
    }
}
