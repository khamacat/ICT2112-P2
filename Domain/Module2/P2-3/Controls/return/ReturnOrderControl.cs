using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ReturnOrderControl : iReturnOrderCRUD, iReturnOrderQuery, iReturnProcess
{
    private readonly IReturnRequestMapper _returnRequestMapper;
    private readonly iReturnItemCRUD      _returnItemCRUD;

    public ReturnOrderControl(
        IReturnRequestMapper returnRequestMapper,
        iReturnItemCRUD      returnItemCRUD)
    {
        _returnRequestMapper = returnRequestMapper ?? throw new ArgumentNullException(nameof(returnRequestMapper));
        _returnItemCRUD      = returnItemCRUD      ?? throw new ArgumentNullException(nameof(returnItemCRUD));
    }

    // -- iReturnOrderQuery ------------------------------------------------

    public ReturnRequestStatus GetReturnStatus(int returnRequestId)
    {
        var request = _returnRequestMapper.FindById(returnRequestId);
        return request?.GetStatus() ?? ReturnRequestStatus.PROCESSING;
    }

    public Returnrequest? GetReturnRequestByOrderId(int orderId)
    {
        return _returnRequestMapper.FindByOrderId(orderId);
    }

    public ICollection<Returnrequest> GetAllReturnRequests()
    {
        return _returnRequestMapper.FindAll() ?? new List<Returnrequest>();
    }

    public Returnrequest? GetReturnRequestById(int returnRequestId)
    {
        return _returnRequestMapper.FindById(returnRequestId);
    }

    // -- iReturnOrderCRUD -------------------------------------------------

    public bool CreateReturnRequest(Returnrequest returnRequest)
    {
        if (!ValidateReturnRequest(returnRequest)) return false;
        try { _returnRequestMapper.Insert(returnRequest); return true; }
        catch { return false; }
    }

    public bool CompleteReturnProcess(int returnRequestId)
    {
        var fresh = _returnRequestMapper.FindById(returnRequestId);
        if (fresh is null) return false;
        fresh.CompleteReturn();
        try { _returnRequestMapper.Update(fresh); return true; }
        catch { return false; }
    }

    // -- Control-level methods (from diagram) -----------------------------

    public bool UpdateReturnStatus(int returnRequestId, string status)
    {
        var fresh = _returnRequestMapper.FindById(returnRequestId);
        if (fresh is null) return false;
        if (!Enum.TryParse<ReturnRequestStatus>(status, out var parsedStatus)) return false;
        fresh.SetStatus(parsedStatus);
        try { _returnRequestMapper.Update(fresh); return true; }
        catch { return false; }
    }

    public bool AcknowledgeReturn(int returnRequestId, int staffId)
    {
        var fresh = _returnRequestMapper.FindById(returnRequestId);
        if (fresh is null) return false;
        try { _returnRequestMapper.Update(fresh); return true; }
        catch { return false; }
    }

    public bool RejectReturn(int returnRequestId, int staffId, string reason)
    {
        var fresh = _returnRequestMapper.FindById(returnRequestId);
        if (fresh is null || string.IsNullOrWhiteSpace(reason)) return false;
        fresh.SetStatus(ReturnRequestStatus.COMPLETED);
        try { _returnRequestMapper.Update(fresh); return true; }
        catch { return false; }
    }

    public bool ValidateReturnRequest(Returnrequest returnRequest)
    {
        if (returnRequest is null) return false;
        if (returnRequest.GetOrderId() <= 0) return false;
        if (returnRequest.GetCustomerId() <= 0) return false;
        // Check no existing return request for this order
        if (_returnRequestMapper.FindByOrderId(returnRequest.GetOrderId()) != null) return false;
        return true;
    }

    // -- iReturnProcess ---------------------------------------------------

    /// <summary>
    /// Creates ReturnRequest, then creates one ReturnItem per inventoryItemId
    /// via iReturnItemCRUD (<<uses>> link in diagram).
    /// </summary>
    public bool TriggerReturnProcess(int orderId, int customerId, DateTime requestDate, List<int> inventoryItemIds)
    {
        if (orderId <= 0 || customerId <= 0 || inventoryItemIds is null || inventoryItemIds.Count == 0) return false;
        if (_returnRequestMapper.FindByOrderId(orderId) != null) return false;

        try
        {
            // 1. Create ReturnRequest
            var returnRequest = new Returnrequest();
            returnRequest.SetOrderId(orderId);
            returnRequest.SetCustomerId(customerId);
            returnRequest.SetStatus(ReturnRequestStatus.PROCESSING);
            returnRequest.SetRequestDate(DateTime.UtcNow);
            _returnRequestMapper.Insert(returnRequest);

            // 2. Get the inserted request to retrieve generated ID
            var inserted = _returnRequestMapper.FindByOrderId(orderId);
            if (inserted is null) return false;

            // 3. Create one ReturnItem per inventory item via iReturnItemCRUD
            foreach (var inventoryItemId in inventoryItemIds)
            {
                var returnItem = new Returnitem();
                returnItem.SetReturnRequestId(inserted.GetReturnRequestId());
                returnItem.SetInventoryItemId(inventoryItemId);
                returnItem.SetStatus(ReturnItemStatus.DAMAGE_INSPECTION);
                _returnItemCRUD.CreateReturnItem(returnItem);
            }

            return true;
        }
        catch { return false; }
    }
}