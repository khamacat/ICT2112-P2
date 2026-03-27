using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class InventoryService : IInventoryService, IResupplyService
{
    // The Facade injects all the specific subsystem experts it needs to coordinate.
    private readonly IProductQuery _productQuery;
    private readonly IProductActions _productActions;
    private readonly iInventoryCRUDControl _inventoryCRUD;
    private readonly iInventoryQueryControl _inventoryQuery;
    private readonly iInventoryStatusControl _inventoryStatus;
    private readonly ILoanListQuery _loanQuery;
    private readonly ILoanActions _loanActions;
    private readonly ILoanValidation _loanValidation;
    private readonly iReturnProcess _returnProcess;

    public InventoryService(
        IProductQuery productQuery,
        IProductActions productActions,
        iInventoryCRUDControl inventoryCRUD,
        iInventoryQueryControl inventoryQuery,
        iInventoryStatusControl inventoryStatus,
        ILoanListQuery loanQuery,
        ILoanActions loanActions,
        ILoanValidation loanValidation,
        iReturnProcess returnProcess)
    {
        _productQuery = productQuery;
        _productActions = productActions;
        _inventoryCRUD = inventoryCRUD;
        _inventoryQuery = inventoryQuery;
        _inventoryStatus = inventoryStatus;
        _loanQuery = loanQuery;
        _loanActions = loanActions;
        _loanValidation = loanValidation;
        _returnProcess = returnProcess;
    }

    // ── Queries (Pass-Throughs to Subsystems) ─────────────────────────────────
    // The Facade doesn't do the querying; it just routes the request.

    public List<Product>? GetAllProducts() => _productQuery.GetAllProducts()?.ToList();
    public Product? GetProduct(int productId) => _productQuery.GetProductById(productId);
    public List<Product>? GetProductByCategory(int categoryId) => _productQuery.GetProductsByCategoryId(categoryId)?.ToList();
    public int CheckProductQuantity(int productId, InventoryStatus status) => _inventoryQuery.CheckProductQuantityByStatus(productId, status);
    public ProductStatus CheckProductStatus(int productId) => _productQuery.CheckProductStatus(productId);
    public List<Inventoryitem>? GetInventoryItemByStatus(InventoryStatus status) => _inventoryQuery.GetInventoryByStatus(status);
    public List<Product>? GetProductsByStatus(ProductStatus status) => _productQuery.GetProductsByStatus(status)?.ToList();

    // ── Business Coordination Logic ──────────────────────────────────────────

    public bool ResupplyProduct(int productId, int quantity)
    {
        // 1. Update the parent Product quantity
        bool productUpdated = _productActions.AddToProduct(productId, quantity);
        if (!productUpdated) 
            return false;

        // 2. Generate the physical bulk inventory items
        int createdCount = _inventoryCRUD.CreateBulkInventoryItems(productId, quantity);
        
        return createdCount == quantity;
    }

    public bool ProcessLoan(int orderId, int customerId, DateTime startDate, DateTime dueDate, Dictionary<int, int> productQuantities)
    {
        // ── PRE-FLIGHT CHECKS ──

        // 1. Ask the Loan sub-system if the dates and order ID are legal
        if (!_loanValidation.ValidateLoanParameters(orderId, startDate, dueDate))
            return false;

        // 2. Ask the Inventory sub-system if we have enough physical stock
        var masterInventoryList = new List<int>();

        foreach (var kvp in productQuantities)
        {
            int productId = kvp.Key;
            int requiredQuantity = kvp.Value;

            var allocatedIds = _inventoryQuery.AllocateAvailableItems(productId, requiredQuantity);

            // Safety Net against the Teammate's .Take() bug!
            // If they asked for 5 but we only got 3 back, we must abort the entire loan.
            if (allocatedIds == null || allocatedIds.Count < requiredQuantity)
                return false; 

            masterInventoryList.AddRange(allocatedIds);
        }

        // ── EXECUTION ── (All pre-flight checks passed)

        // 3. Mark all the allocated physical items as ON_LOAN
        foreach (var inventoryItemId in masterInventoryList)
        {
            _inventoryStatus.UpdateInventoryStatus(inventoryItemId, InventoryStatus.ON_LOAN);
        }

        // 4. Instruct the Loan sub-system to generate the paperwork and bridge tables
        return _loanActions.ProcessLoan(orderId, customerId, startDate, dueDate, masterInventoryList);
    }

    public bool TriggerReturnProcess(int orderId, DateTime requestDate)
    {
        // 1. Fetch the Loan List to get the necessary data for the Return subsystem
        var loanList = _loanQuery.GetLoanListById(orderId); // Assumes we can find by OrderId or you refactor to pass it
        if (loanList == null)
            return false;

        int customerId = loanList.GetCustomerId();
        var inventoryItemIds = _loanQuery.GetInventoryItemList(orderId);

        if (inventoryItemIds == null || !inventoryItemIds.Any())
            return false;

        // 2. Trigger the Return Process
        bool returnTriggered = _returnProcess.TriggerReturnProcess(orderId, customerId, requestDate, inventoryItemIds);
        if (!returnTriggered)
            return false;

        // 3. Close the Loan
        bool loanClosed = _loanActions.CloseLoan(orderId);
        if (!loanClosed)
            return false;

        // 4. Update the physical items to MAINTENANCE status so they can be inspected
        foreach (var itemId in inventoryItemIds)
        {
            _inventoryStatus.UpdateInventoryStatus(itemId, InventoryStatus.MAINTENANCE);
        }

        return true;
    }
}