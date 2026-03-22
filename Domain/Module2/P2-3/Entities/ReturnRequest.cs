using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnrequest
{
    private ReturnRequestStatus _status;
    private ReturnRequestStatus Status { get => _status; set => _status = value; }

    public int GetReturnRequestId() => _returnrequestid;
    public void SetReturnRequestId(int id) => _returnrequestid = id;

    public int GetOrderId() => _orderid;
    public void SetOrderId(int id) => _orderid = id;

    public int GetCustomerId() => _customerid;
    public void SetCustomerId(int id) => _customerid = id;

    public List<int> GetReturnItemsId() => Returnitems.Select(i => i.GetReturnItemId()).ToList();

    public ReturnRequestStatus GetStatus() => _status;
    public void SetStatus(ReturnRequestStatus status) => _status = status;

    public DateTime GetRequestDate() => _requestdate;
    public void SetRequestDate(DateTime date) => _requestdate = date;

    public DateTime? GetCompletionDate() => _completiondate;
    public void SetCompletionDate(DateTime date) => _completiondate = date;

    public void AddReturnItemsToList(List<int> itemIds) { }

    public void CompleteReturn()
    {
        _status = ReturnRequestStatus.COMPLETED;
        _completiondate = DateTime.UtcNow;
    }
}