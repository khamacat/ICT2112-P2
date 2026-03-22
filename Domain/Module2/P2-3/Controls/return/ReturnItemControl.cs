using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class ReturnItemControl : iReturnItemCRUD, iReturnItemQuery
{
    private readonly IReturnItemMapper _returnItemMapper;
    private readonly IReturnRequestMapper _returnRequestMapper;

    public ReturnItemControl(
        IReturnItemMapper returnItemMapper,
        IReturnRequestMapper returnRequestMapper)
    {
        _returnItemMapper    = returnItemMapper    ?? throw new ArgumentNullException(nameof(returnItemMapper));
        _returnRequestMapper = returnRequestMapper ?? throw new ArgumentNullException(nameof(returnRequestMapper));
    }

    // -- iReturnItemQuery -------------------------------------------------

    public Returnitem? GetReturnItem(int returnItemId)
    {
        return _returnItemMapper.FindById(returnItemId);
    }

    public List<Returnitem> GetReturnItemByRequestId(int returnRequestId)
    {
        return _returnItemMapper.FindByReturnRequest(returnRequestId)?.ToList() ?? new List<Returnitem>();
    }

    // -- iReturnItemCRUD --------------------------------------------------

    public bool CreateReturnItem(Returnitem returnItem)
    {
        if (returnItem is null)
        {
            return false;
        }

        try
        {
            _returnItemMapper.Insert(returnItem);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateReturnItem(Returnitem returnItem)
    {
        if (returnItem is null)
        {
            return false;
        }

        try
        {
            _returnItemMapper.Update(returnItem);
            TryCompleteRequest(returnItem.GetReturnRequestId());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateReturnItemStatus(int returnItemId, string status)
    {
        var item = _returnItemMapper.FindById(returnItemId);
        if (item is null)
        {
            return false;
        }

        if (!Enum.TryParse<ReturnItemStatus>(status, out var parsedStatus))
        {
            return false;
        }

        item.SetStatus(parsedStatus);

        try
        {
            _returnItemMapper.Update(item);
            TryCompleteRequest(item.GetReturnRequestId());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AcknowledgeReturn(int returnItemId, int staffId)
    {
        var item = _returnItemMapper.FindById(returnItemId);
        if (item is null)
        {
            return false;
        }

        item.ConductInspection();

        try
        {
            _returnItemMapper.Update(item);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RejectReturn(int returnItemId, int staffId, string reason)
    {
        var item = _returnItemMapper.FindById(returnItemId);
        if (item is null || string.IsNullOrWhiteSpace(reason))
        {
            return false;
        }

        try
        {
            _returnItemMapper.Update(item);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool CompleteReturnItemProcess(int returnItemId)
    {
        var item = _returnItemMapper.FindById(returnItemId);
        if (item is null)
        {
            return false;
        }

        item.CompleteReturn();

        try
        {
            _returnItemMapper.Update(item);
            TryCompleteRequest(item.GetReturnRequestId());
            return true;
        }
        catch
        {
            return false;
        }
    }

    // -- Private helpers --------------------------------------------------

    private void TryCompleteRequest(int requestId)
    {
        var items = _returnItemMapper.FindByReturnRequest(requestId);
        if (items is null || items.Count == 0)
        {
            return;
        }

        if (!items.All(i => i.GetStatus() == ReturnItemStatus.RETURN_TO_INVENTORY))
        {
            return;
        }

        var request = _returnRequestMapper.FindById(requestId);
        if (request is null)
        {
            return;
        }

        request.CompleteReturn();
        _returnRequestMapper.Update(request);
    }
}