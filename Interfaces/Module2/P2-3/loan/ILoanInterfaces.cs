using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ILoanActions
{
	public bool ProcessLoan(int orderId, int customerId, DateTime startDate, DateTime dueDate, List<int> inventoryItemIds);
	public bool CloseLoan(int orderId);
}

public interface ILoanValidation
{
    public bool ValidateLoanParameters(int orderId, DateTime startDate, DateTime dueDate);
}

public interface ILoanListQuery
{
	public Loanlist? GetLoanListById(int loanlistid);
	public List<Loanlist>? GetAllLoanList();
	public List<int>? GetInventoryItemList(int orderid);
}

public interface ILoanListCRUD
{
	public bool CreateLoanList(int orderId, int customerId, DateTime startDate, DateTime dueDate);
	public bool UpdateLoanList(Loanlist loan);
	public bool DeleteLoanList(int loanlistid);
}

public interface ILoanItemQuery
{
	public Loanitem? GetLoanItemById(int loanitemid);
	public List<Loanitem>? GetAllLoanItems();
	public List<int>? GetInventoryItemIdByList(int loanlistid);
}

public interface ILoanItemCRUD
{
	public bool CreateLoanItem(int loanlistid, int inventoryitemid);
	public bool UpdateLoanItem(Loanitem loanitem);
	public bool DeleteLoanItem(int loanitemid);
}