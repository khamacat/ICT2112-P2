namespace ProRental.Domain.Entities;

public partial class Loanitem
{
	// Factory Method
    public static Loanitem Create(int loanListId, int inventoryItemId, string? remarks = null)
    {
        var item = new Loanitem();
        item.SetLoanListId(loanListId);
        item.SetInventoryItemId(inventoryItemId);
        item.SetRemarks(remarks);
        return item;
    }

	// Business methods
	public void UpdateRemarks(string? remarks) => SetRemarks(remarks);

	// Getters
	public int GetLoanItemId() => _loanitemid;
	public int GetLoanListId() => _loanlistid;
    public int GetInventoryItemId() => _inventoryitemid;
    public string? GetRemarks() => _remarks;

    // Setters
    private void SetLoanListId(int id) => _loanlistid = id;
    private void SetInventoryItemId(int id) => _inventoryitemid = id;
    private void SetRemarks(string? remarks) => _remarks = remarks;
}