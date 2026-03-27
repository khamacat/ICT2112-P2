using ProRental.Data.Module2.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module2.P22.Controls;


// Control class for managing Replenishment Request business logic
// Based on Team 2-2 Class Diagram specification

public class ReplenishmentRequestControl
{
    private readonly IReplenishmentRequestMapper _mapper;
    private readonly IResupplyService _resupplyService;

    public ReplenishmentRequestControl(
        IReplenishmentRequestMapper mapper,
        IResupplyService resupplyService)
    {
        _mapper = mapper;
        _resupplyService = resupplyService;
    }

    // Create a new replenishment request in DRAFT status
    // <param name="requestedByStaffId">Staff ID who created the request</param>
    // <returns>The newly created ReplenishmentRequest</returns>
    public Replenishmentrequest CreateRequest(string requestedByStaffId)
    {
        var request = new Replenishmentrequest();
        request.InitializeDraft(requestedByStaffId, DateTime.UtcNow);

        _mapper.Insert(request);
        return request;
    }

    // Add a line item to a replenishment request
    // <param name="requestId">The request ID</param>
    // <param name="productId">The product ID to add</param>
    // <returns>The newly created line item</returns>
    public Lineitem AddItem(int requestId, int productId)
    {
        var request = _mapper.FindById(requestId);

        if (request == null)
        {
            throw new InvalidOperationException($"Replenishment request {requestId} not found");
        }

        if (!request.CanEdit())
        {
            throw new InvalidOperationException("Cannot add items to a non-draft request");
        }

        var lineItem = request.AddLineItem(productId);
        _mapper.Update(request);

        return lineItem;
    }

    // Update an existing line item
    // <param name="requestId">The request ID</param>
    // <param name="lineItemId">The line item ID to update</param>
    // <param name="quantity">New quantity</param>
    // <param name="reason">Replenishment reason</param>
    // <param name="remarks">Optional remarks</param>
    // <returns>True if updated successfully</returns>
    public bool UpdateItem(int requestId, int lineItemId, int quantity, ReplenishmentReason reason, string remarks)
    {
        var request = _mapper.FindById(requestId);

        if (request == null)
        {
            return false;
        }

        if (!request.CanEdit())
        {
            return false;
        }

        var lineItem = request.FindLineItem(lineItemId);
        if (lineItem == null)
        {
            return false;
        }

        // Update line item properties
        if (!lineItem.SetQuantity(quantity))
        {
            return false;
        }

        lineItem.SetReason(reason);
        lineItem.SetRemarks(remarks);

        _mapper.Update(request);
        return true;
    }

    // Update request-level remarks
    // <param name="requestId">The request ID</param>
    // <param name="remarks">Overall request note</param>
    // <returns>True if updated successfully</returns>
    public bool UpdateRequestRemarks(int requestId, string? remarks)
    {
        var request = _mapper.FindById(requestId);

        if (request == null)
        {
            return false;
        }

        if (!request.CanEdit())
        {
            return false;
        }

        request.SetRemarks(remarks?.Trim() ?? string.Empty);
        _mapper.Update(request);
        return true;
    }


    // Remove a line item from a request
    // <param name="requestId">The request ID</param>
    // <param name="lineItemId">The line item ID to remove</param>
    // <returns>True if removed successfully</returns>
    public bool RemoveItem(int requestId, int lineItemId)
    {
        var request = _mapper.FindById(requestId);

        if (request == null)
        {
            return false;
        }

        bool removed = request.RemoveLineItem(lineItemId);

        if (removed)
        {
            _mapper.Update(request);
        }

        return removed;
    }

    // Submit a replenishment request (change status from DRAFT to SUBMITTED)
    // <param name="requestId">The request ID to submit</param>
    // <returns>True if submitted successfully</returns>
    public bool SubmitRequest(int requestId)
    {
        var request = _mapper.FindById(requestId);

        if (request == null)
        {
            return false;
        }

        try
        {
            bool submitted = request.Submit();

            if (submitted)
            {
                _mapper.Update(request);
            }

            return submitted;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    // Cancel a replenishment request
    // <param name="requestId">The request ID to cancel</param>
    // <returns>True if cancelled successfully</returns>
    public bool CancelRequest(int requestId)
    {
        var request = _mapper.FindById(requestId);

        if (request == null)
        {
            return false;
        }

        bool cancelled = request.Cancel();

        if (cancelled)
        {
            _mapper.Update(request);
        }

        return cancelled;
    }

    // Get a replenishment request by ID
    // <param name="requestId">The request ID</param>
    // <returns>The ReplenishmentRequest or null if not found</returns>
    public Replenishmentrequest? GetRequest(int requestId)
    {
        return _mapper.FindById(requestId);
    }

    // Get all replenishment requests
    // <returns>List of all replenishment requests</returns>
    public List<Replenishmentrequest> GetAllRequests()
    {
        return _mapper.FindAll();
    }


    // Mark a request as completed
    // <param name="requestId">The request ID to complete</param>
    // <param name="staffId">The staff ID who completed the request</param>
    // <returns>True if marked complete successfully</returns>
    public bool MarkRequestComplete(int requestId, string staffId)
    {
        var request = _mapper.FindById(requestId);

        if (request == null)
        {
            return false;
        }

        foreach (var lineItem in request.Lineitems)
        {
            var productId = lineItem.GetProductId();
            var quantity = lineItem.GetQuantityRequest();

            if (!productId.HasValue || productId.Value <= 0 || !quantity.HasValue || quantity.Value <= 0)
            {
                return false;
            }

            var resupplied = _resupplyService.ResupplyProduct(productId.Value, quantity.Value);
            if (!resupplied)
            {
                return false;
            }
        }

        if (!request.MarkComplete(staffId, DateTime.UtcNow))
        {
            return false;
        }

        _mapper.Update(request);
        return true;
    }
}
