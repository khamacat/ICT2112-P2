using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Returnrequest
{
    // PUBLIC GETTERS

    public static Returnrequest Create(int orderId, int customerId, DateTime requestDate)
    {
        var request = new Returnrequest();
        request.SetOrderId(orderId);
        request.SetCustomerId(customerId);
        request.SetStatus(ReturnRequestStatus.PROCESSING);
        request.SetRequestDate(DateTime.UtcNow);
        request.SetRequestDate(requestDate);
        return request;
    }

    public int GetReturnRequestId() => _returnrequestid;
    public int GetOrderId() => _orderid;
    public int GetCustomerId() => _customerid;
    public DateTime GetRequestDate() => _requestdate;
    public DateTime? GetCompletionDate() => _completiondate;
    public ReturnRequestStatus GetStatus() => _status;

    public List<int> GetReturnItemsId() => Returnitems.Select(i => i.GetReturnItemId()).ToList();

    public void CompleteReturn()
    {
        _status = ReturnRequestStatus.COMPLETED;
        _completiondate = DateTime.UtcNow;
    }

    // PRIVATE SETTERS 
    private ReturnRequestStatus _status;
    private ReturnRequestStatus Status { get => _status; set => _status = value; }

    private void SetReturnRequestId(int id) => _returnrequestid = id;
    private void SetOrderId(int id) => _orderid = id;
    private void SetCustomerId(int id) => _customerid = id;
    private void SetStatus(ReturnRequestStatus status) => _status = status;
    private void SetRequestDate(DateTime date) => _requestdate = date;
    private void SetCompletionDate(DateTime date) => _completiondate = date;
}