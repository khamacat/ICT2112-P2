using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ReturnOrderControl : iReturnOrderCRUD, iReturnOrderQuery, iReturnProcess
{
    private readonly IReturnRequestMapper _returnRequestMapper;
    private readonly IReturnItemMapper _returnItemMapper;

    public ReturnOrderControl(
        IReturnRequestMapper returnRequestMapper,
        IReturnItemMapper returnItemMapper)
    {
        _returnRequestMapper = returnRequestMapper ?? throw new ArgumentNullException(nameof(returnRequestMapper));
        _returnItemMapper    = returnItemMapper    ?? throw new ArgumentNullException(nameof(returnItemMapper));
    }

    // -- iReturnOrderQuery ------------------------------------------------

    public ICollection<Returnrequest> GetAllReturnRequests()
    {
        return _returnRequestMapper.FindAll() ?? new List<Returnrequest>();
    }

    public ICollection<Returnrequest> GetProcessingReturnRequests()
    {
        return _returnRequestMapper.FindByStatus(ReturnRequestStatus.PROCESSING)
               ?? new List<Returnrequest>();
    }

    public ICollection<Returnrequest> GetCompletedReturnRequests()
    {
        return _returnRequestMapper.FindByStatus(ReturnRequestStatus.COMPLETED)
               ?? new List<Returnrequest>();
    }

    public Returnrequest? GetReturnRequestById(int returnRequestId)
    {
        return _returnRequestMapper.FindById(returnRequestId);
    }

    public ReturnRequestStatus GetReturnStatus(int returnRequestId)
    {
        var request = _returnRequestMapper.FindById(returnRequestId);
        return request?.GetStatus() ?? ReturnRequestStatus.PROCESSING;
    }

    public Returnrequest? GetReturnRequestByOrderId(int orderId)
    {
        return _returnRequestMapper.FindByOrderId(orderId);
    }

    // -- iReturnOrderCRUD -------------------------------------------------

    public bool CreateReturnRequest(Returnrequest returnRequest)
    {
        if (returnRequest is null)
        {
            return false;
        }

        try
        {
            _returnRequestMapper.Insert(returnRequest);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateReturnStatus(int returnRequestId, string status)
    {
        var request = _returnRequestMapper.FindById(returnRequestId);
        if (request is null)
        {
            return false;
        }

        if (!Enum.TryParse<ReturnRequestStatus>(status, out var parsedStatus))
        {
            return false;
        }

        request.SetStatus(parsedStatus);

        try
        {
            _returnRequestMapper.Update(request);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AcknowledgeReturn(int returnRequestId, int staffId)
    {
        var request = _returnRequestMapper.FindById(returnRequestId);
        if (request is null)
        {
            return false;
        }

        try
        {
            _returnRequestMapper.Update(request);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RejectReturn(int returnRequestId, int staffId, string reason)
    {
        var request = _returnRequestMapper.FindById(returnRequestId);
        if (request is null || string.IsNullOrWhiteSpace(reason))
        {
            return false;
        }

        request.SetStatus(ReturnRequestStatus.COMPLETED);

        try
        {
            _returnRequestMapper.Update(request);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Called when all items in a request reach RETURN_TO_INVENTORY
    public bool CompleteReturnProcess(int returnRequestId)
    {
        var request = _returnRequestMapper.FindById(returnRequestId);
        if (request is null)
        {
            return false;
        }

        var items = _returnItemMapper.FindByReturnRequest(returnRequestId);
        if (items is null || items.Count == 0)
        {
            return false;
        }

        // Only complete if every item is RETURN_TO_INVENTORY
        if (!items.All(i => i.GetStatus() == ReturnItemStatus.RETURN_TO_INVENTORY))
        {
            return false;
        }

        request.CompleteReturn();

        try
        {
            _returnRequestMapper.Update(request);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ValidateReturnRequest(Returnrequest returnRequest)
    {
        if (returnRequest is null)
        {
            return false;
        }

        if (returnRequest.GetOrderId() <= 0)
        {
            return false;
        }

        if (returnRequest.GetCustomerId() <= 0)
        {
            return false;
        }

        var existing = _returnRequestMapper.FindByOrderId(returnRequest.GetOrderId());
        if (existing != null)
        {
            return false;
        }

        return true;
    }

    // -- iReturnProcess ---------------------------------------------------

    public bool TriggerReturnProcess(int orderId)
    {
        var existing = _returnRequestMapper.FindByOrderId(orderId);
        return existing is null;
    }
}