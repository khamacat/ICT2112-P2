using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Loanlist
{
    private LoanStatus _status;
    private LoanStatus Status { get => _status; set => _status = value; }

    public static Loanlist Create(int orderId, int customerId, DateTime startDate, DateTime dueDate, string? remarks = null)
    {
        // Enforce UTC
        var startUtc = startDate.Kind == DateTimeKind.Utc ? startDate : startDate.ToUniversalTime();
        var dueUtc = dueDate.Kind == DateTimeKind.Utc ? dueDate : dueDate.ToUniversalTime();

        // Intrinsic Validation: Start date cannot be after Due date
        if (startUtc > dueUtc)
            throw new ArgumentException("Start date cannot be after the due date.");

        var loanList = new Loanlist();
        loanList.SetOrderId(orderId);
        loanList.SetCustomerId(customerId);
        loanList.SetLoanDate(startUtc); 
        loanList.SetDueDate(dueUtc);
        loanList.SetRemarks(remarks);
        loanList.UpdateStatus(LoanStatus.OPEN);

        return loanList;
    }

    // Getters
    public int GetLoanListId() => _loanlistid;
    public int GetOrderId() => _orderid;
    public int GetCustomerId() => _customerid;
    public DateTime GetLoanDate() => _loandate;
    public DateTime GetDueDate() => _duedate;
    public DateTime? GetReturnDate() => _returndate;
    public string? GetRemarks() => _remarks;
    public LoanStatus GetStatus() => _status;

    // Setters / Assingers
    public void UpdateRemarks(string? remarks) => SetRemarks(remarks);
    public void UpdateStatus(LoanStatus newValue) => _status = newValue;
    public void ProcessLoan() => UpdateStatus(LoanStatus.ON_LOAN);
    public void ProcessReturn()
    {
        SetReturnDate(DateTime.UtcNow);
        UpdateStatus(LoanStatus.RETURNED);
    }

    private void SetOrderId(int orderId) => _orderid = orderId;
    private void SetCustomerId(int customerId) => _customerid = customerId;
    private void SetLoanDate(DateTime date) => _loandate = date;
    private void SetDueDate(DateTime date) => _duedate = date;
    private void SetReturnDate(DateTime? date) => _returndate = date;
    private void SetRemarks(string? remarks) => _remarks = remarks;
}