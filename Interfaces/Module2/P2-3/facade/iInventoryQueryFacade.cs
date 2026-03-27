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

// 1. The Base Query Interface (Shared across modules)
public interface IInventoryQueryFacade
{
    List<Product>? GetAllProducts();
    Product? GetProduct(int productId);
    List<Product>? GetProductByCategory(int categoryId);
    int CheckProductQuantity(int productId, InventoryStatus status);
    ProductStatus CheckProductStatus(int productId);
    List<Inventoryitem>? GetInventoryItemByStatus(InventoryStatus status);
    List<Product>? GetProductsByStatus(ProductStatus status);
}