using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

// Facade Design pattern, Coordinator between different internal subsystem
//
// What does it use?
// Subsystem: Interfaces
// Product subsystem: IProductQuery, IProductCRUD
// Inventory subsystem: IInventoryCRUDControl, IInventoryQueryControl, IInventoryStatusControl
// Loan subsystem: ILoanListQuery, ILoanActions
// Return subsytem: IReturnProcess
//
// Users of this service:
// Module 1(Order Processing System) (Both teams): querying for products, or availability or any other conditional querying services,
// 										triggers the loaning process once order and checkout is successful, and triggers the return prcoess once order has been recieved by staff
// Module 2(InventoryManagement System) (Team P2-2): Will use this service for the add to product method, where needs coordination between the product updating quantity and bulk
//													and Bulk creating inventoryitems with no serial number or expiry date but with product id
//
// Logical Responsibilities:
// Pre-flight checks to be done here as well, checking for available quantity of inventoryitems per product line before compiling them into a list for loan subsystem to start loaning and updating inventoryitem status to on loan upon succcessful creation.
// triggering of return process will he be here, where it will close the loanlist, and once the returnrequest creation is successful, will update the inventoryitem status to maintenance

public interface IInventoryService
{
	// Querying services
	public List<Product>? GetAllProducts();

	public Product? GetProduct(int productId);

	public List<Product>? GetProductByCategory(int categoryId);

	public int CheckProductQuantity(int productId, InventoryStatus status);

	public ProductStatus CheckProductStatus(int productId);

	public List<Inventoryitem>? GetInventoryItemByStatus(InventoryStatus status);

	public List<Product>? GetProductsByStatus(ProductStatus status);

	// Business Coordination logic
	
	// User: Team P2-2 Purpose: When resupply is received, and staff clicks received resupply, the product quantity will automatically be updated and inventoryitems with productid but no serial numbers and expirdate will be created (with reserved status)
	// Coordinates: IProductCRUD (AddToProduct method), IInventoryCRUD (BulkCreateInventoryItems method)
	public bool AddToProduct(int productId, int quantity);

	// User: Module 1. Purpose: Once Module 1 checksout and creates an order, they will call this method and pass in the necessary details, while waiting for a return successful (true) or not, must handle pre flight checking etc
	// Coordinates: IInventoryQueryControl, ILoanActions and IInventoryStatusControl
	public bool ProcessLoan(int orderId, int customerId, DateTime startDate, DateTime dueDate, Dictionary<int, int> productQuantities);

	// User: Module 1. Purpose: Once staff receives the rental order return, will trigger our process to open a returnrequest, reutnring true or false.
	// Coordinates: IInvenotryStatusControl, ILoanQuery(to get the inventoryitems and customerid attached to an orderid), ILoanActions(closing) and IReturnProcess(trigger)
	public bool TriggerReturnProcess(int orderId, DateTime requestDate);
}