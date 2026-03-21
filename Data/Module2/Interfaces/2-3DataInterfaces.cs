using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Data;

public interface IProductMapper
{
	public Product? FindById(int productId);

	public ICollection<Product>? FindAll();

	public ICollection<Product>? FindByCategoryId(int categoryId);

	public ICollection<Product>? FindByStatus(ProductStatus status);

	public void Insert(Product product);

	public void Update(Product product);

	public void Delete(Product product);
}

public interface IAlertMapper
{
	public Alert? FindById(int alertId);

	public ICollection<Alert>? FindAll();

	public ICollection<Alert>? FindByProductId(int productId);

	public void Insert(Alert alert);

	public void Update(Alert alert);

	public void Delete(Alert alert);
}

public interface IInventoryItemMapper
{
	public Inventoryitem? FindById(int inventoryItemId);

	public ICollection<Inventoryitem>? FindAll();

	public ICollection<Inventoryitem>? FindByProductId(int productId);

	public ICollection<Inventoryitem>? FindByStatus(InventoryStatus status);

	public void Insert(Inventoryitem item);

	public void Update(Inventoryitem item);

	public void Delete(Inventoryitem item);
}

public interface ICategoryMapper
{
	public Category? FindById(int categoryId);

	public ICollection<Category>? FindAll();

	public void Insert(Category category);

	public void Update(Category category);

	public void Delete(Category category);
}

public interface IReturnItemMapper
{
	public Returnitem? FindById(int itemId);

	public ICollection<Returnitem>? FindAll();

	public ICollection<Returnitem>? FindByReturnRequest(int requestId);

	public ICollection<Returnitem>? FindByStatus(ReturnItemStatus status);

	public void Insert(Returnitem item);

	public void Update(Returnitem item);

	public void Delete(Returnitem item);
}

public interface IReturnRequestMapper
{
	public Returnrequest? FindById(int requestId);

	public Returnrequest? FindByOrderId(int orderId);

	public ICollection<Returnrequest>? FindAll();

	public ICollection<Returnrequest>? FindByCustomerId(int customerId);

	public ICollection<Returnrequest>? FindByStatus(ReturnRequestStatus status);

	public void Insert(Returnrequest request);

	public void Update(Returnrequest request);

	public void Delete(Returnrequest request);
}

public interface IDamageReportMapper
{
	public Damagereport? FindById(int damageReportId);

	public ICollection<Damagereport>? FindAll();

	public Damagereport? FindByReturnItemId(int returnItemId);

	public void Insert(Damagereport damageReport);

	public void Update(Damagereport damageReport);

	public void Delete(Damagereport damageReport);
}

public interface IClearanceBatchMapper
{
	public Clearancebatch? FindById(int batchId);

	public ICollection<Clearancebatch>? FindAll();

	public ICollection<Clearancebatch>? FindByStatus(ClearanceBatchStatus status);

	public void Insert(Clearancebatch batch);

	public void Update(Clearancebatch batch);

	public void Delete(Clearancebatch batch);
}

public interface IClearanceItemMapper
{
	public Clearanceitem? FindById(int itemId);

	public Clearanceitem? FindByInventoryItemId(int inventoryItemId);

	public ICollection<Clearanceitem>? FindByBatchId(int batchId);

	public ICollection<Clearanceitem>? FindByStatus(ClearanceStatus status);

	public ICollection<Clearanceitem>? FindAll();

	public void Insert(Clearanceitem item);

	public void Update(Clearanceitem item);

	public void Delete(Clearanceitem item);
}

public interface ILoanItemMapper
{
	public Loanitem? FindById(int itemId);

	public ICollection<Loanitem>? FindByLoanListId(int listId);

	public ICollection<Loanitem>? FindAll();

	public void Insert(Loanitem item);

	public void Update(Loanitem item);

	public void Delete(Loanitem item);
}

public interface ILoanListMapper
{
	public Loanlist? FindById(int listId);

	public Loanlist? FindByOrderId(int orderId);

	public ICollection<Loanlist>? FindAll();

	public ICollection<Loanlist>? FindByBorrowerId(int borrowerId);

	public ICollection<Loanlist>? FindByDate(DateTime loanDate);

	public ICollection<Loanlist>? FindByStatus(LoanStatus status);

	public void Insert(Loanlist list);

	public void Update(Loanlist list);

	public void Delete(Loanlist list);
}
