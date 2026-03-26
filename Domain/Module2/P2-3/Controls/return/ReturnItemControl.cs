using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ReturnItemControl : iReturnItemCRUD, iReturnItemQuery
{
    private readonly IReturnItemMapper        _returnItemMapper;
    private readonly iInventoryStatusControl  _inventoryStatusControl;
    private readonly iInventoryQueryControl   _inventoryQueryControl;
    private readonly IProductQuery            _productQuery;

    public ReturnItemControl(
        IReturnItemMapper        returnItemMapper,
        iInventoryStatusControl  inventoryStatusControl,
        iInventoryQueryControl   inventoryQueryControl,
        IProductQuery            productQuery)
    {
        _returnItemMapper       = returnItemMapper       ?? throw new ArgumentNullException(nameof(returnItemMapper));
        _inventoryStatusControl = inventoryStatusControl ?? throw new ArgumentNullException(nameof(inventoryStatusControl));
        _inventoryQueryControl  = inventoryQueryControl  ?? throw new ArgumentNullException(nameof(inventoryQueryControl));
        _productQuery           = productQuery           ?? throw new ArgumentNullException(nameof(productQuery));
    }

    public Returnitem? GetReturnItem(int returnItemId)
    {
        return _returnItemMapper.FindById(returnItemId);
    }

    public List<Returnitem> GetReturnItemByRequestId(int returnRequestId)
    {
        return _returnItemMapper.FindByReturnRequest(returnRequestId)?.ToList()
               ?? new List<Returnitem>();
    }

    public bool CreateReturnItem(Returnitem returnItem)
    {
        if (returnItem is null) return false;
        try { _returnItemMapper.Insert(returnItem); return true; }
        catch { return false; }
    }

    public bool UpdateReturnItem(Returnitem returnItem)
    {
        if (returnItem is null) return false;
        try
        {
            var fresh = _returnItemMapper.FindById(returnItem.GetReturnItemId());
            if (fresh is null) return false;
            fresh.SetStatus(returnItem.GetStatus());
            if (returnItem.GetCompletionDate().HasValue)
                fresh.SetCompletionDate(returnItem.GetCompletionDate()!.Value);
            _returnItemMapper.Update(fresh);
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// Repair is NOT fixed — update damage report repair cost to
    /// previous cost + product price, then set item RETURN_TO_INVENTORY
    /// and inventory to BROKEN.
    /// </summary>
    public bool MarkItemBroken(int returnItemId, Damagereport damageReport)
    {
        var fresh = _returnItemMapper.FindById(returnItemId);
        if (fresh is null) return false;

        try
        {
            decimal productPrice = 0m;
            var inventoryItem = _inventoryQueryControl.GetInventoryItemById(fresh.GetInventoryItemId());
            if (inventoryItem != null)
            {
                var product = _productQuery.GetProductById(inventoryItem.GetProductId());
                if (product?.GetProductdetail() != null)
                    productPrice = product.GetProductdetail()!.GetPrice();
            }

            var previousCost = damageReport.GetRepairCost() ?? 0m;
            var finalCost    = previousCost + productPrice;
            damageReport.SetRepairCost(finalCost);

            // Append price breakdown to description
            var originalDesc = damageReport.GetDescription() ?? string.Empty;
            var breakdown = $"\n\nUnable to repair.\nPrice breakdown:\n  - Product price: ${productPrice:F2}\n  - Repair cost: ${previousCost:F2}\nFinal cost: ${finalCost:F2}";
            damageReport.SetDescription(originalDesc + breakdown);

            // Set item completed + inventory BROKEN
            fresh.CompleteReturn();
            _returnItemMapper.Update(fresh);
            _inventoryStatusControl.UpdateInventoryStatus(
                fresh.GetInventoryItemId(),
                InventoryStatus.BROKEN);

            return true;
        }
        catch { return false; }
    }
    /// <summary>
    /// Repair is fixed — sets item RETURN_TO_INVENTORY, inventory AVAILABLE.
    /// </summary>
    public bool CompleteReturnItemProcess(int returnItemId)
    {
        var fresh = _returnItemMapper.FindById(returnItemId);
        if (fresh is null) return false;

        fresh.CompleteReturn();

        try
        {
            _returnItemMapper.Update(fresh);
            _inventoryStatusControl.UpdateInventoryStatus(
                fresh.GetInventoryItemId(),
                InventoryStatus.AVAILABLE);
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// Gets product price for a given inventory item (used for repair cost calc).
    /// </summary>
    public decimal GetProductPriceForItem(int inventoryItemId)
    {
        var inventoryItem = _inventoryQueryControl.GetInventoryItemById(inventoryItemId);
        if (inventoryItem is null) return 0m;
        var product = _productQuery.GetProductById(inventoryItem.GetProductId());
        return product?.GetProductdetail()?.GetPrice() ?? 0m;
    }
}