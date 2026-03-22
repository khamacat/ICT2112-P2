using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module2.P22.Controls;

namespace ProRental.Controllers.Module2;

// Page Controller for Replenishment Request feature
// Based on Team 2-2 Class Diagram specification
public class ReplenishmentRequesterController : Controller
{
    private readonly ReplenishmentRequestControl _control;
    private readonly ILogger<ReplenishmentRequesterController> _logger;

    public ReplenishmentRequesterController(
        ReplenishmentRequestControl control,
        ILogger<ReplenishmentRequesterController> logger)
    {
        _control = control;
        _logger = logger;
    }

    // GET: /ReplenishmentRequester or /ReplenishmentRequester/List
    // Display list of all replenishment requests (ReplenishmentList.cshtml)
    public IActionResult List()
    {
        try
        {
            var requests = _control.GetAllRequests();
            return View("~/Views/Module2/ReplenishmentList.cshtml", requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading replenishment requests list");
            TempData["ErrorMessage"] = "An error occurred while loading the requests.";
            return View("~/Views/Module2/ReplenishmentList.cshtml", new List<Replenishmentrequest>());
        }
    }

    // Redirect default route to List
    public IActionResult Index()
    {
        return RedirectToAction(nameof(List));
    }

    // GET: /ReplenishmentRequester/Create
    // Create a new replenishment request with current user and redirect to form
    public IActionResult Create()
    {
        try
        {
            // Get current logged-in user's ID from claims/session
            var currentUserId = User.FindFirst("StaffId")?.Value ?? 
                               User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ??
                               "SYSTEM";

            var request = _control.CreateRequest(currentUserId);
            TempData["SuccessMessage"] = $"Replenishment request #{request.RequestId} created. Add items below.";
            return RedirectToAction(nameof(Form), new { id = request.RequestId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating replenishment request");
            TempData["ErrorMessage"] = "An error occurred while creating the request.";
            return RedirectToAction(nameof(List));
        }
    }

    // GET: /ReplenishmentRequester/Form/5
    // Display form to create/edit replenishment request (ReplenishmentForm.cshtml)
    public IActionResult Form(int id)
    {
        try
        {
            var request = _control.GetRequest(id);

            if (request == null)
            {
                TempData["ErrorMessage"] = "Request not found.";
                return RedirectToAction(nameof(List));
            }

            if (!request.CanEdit())
            {
                TempData["ErrorMessage"] = "Only draft requests can be edited.";
                return RedirectToAction(nameof(Detail), new { id });
            }

            return View("~/Views/Module2/ReplenishmentForm.cshtml", request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading request {RequestId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the request.";
            return RedirectToAction(nameof(List));
        }
    }

    // GET: /ReplenishmentRequester/Detail/5
    // Display details of a replenishment request (ReplenishmentDetail.cshtml)
    public IActionResult Detail(int id)
    {
        try
        {
            var request = _control.GetRequest(id);

            if (request == null)
            {
                TempData["ErrorMessage"] = "Request not found.";
                return RedirectToAction(nameof(List));
            }

            return View("~/Views/Module2/ReplenishmentDetail.cshtml", request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading request {RequestId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the request.";
            return RedirectToAction(nameof(List));
        }
    }

    // POST: /ReplenishmentRequester/AddItem
    // Add a line item to a replenishment request
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddItem(int requestId, int productId)
    {
        try
        {
            var lineItem = _control.AddItem(requestId, productId);
            TempData["SuccessMessage"] = "Item added successfully.";
            return RedirectToAction(nameof(Form), new { id = requestId });
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Form), new { id = requestId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding item to request {RequestId}", requestId);
            TempData["ErrorMessage"] = "An error occurred while adding the item.";
            return RedirectToAction(nameof(Form), new { id = requestId });
        }
    }

    // POST: /ReplenishmentRequester/UpdateItem
    // Update a line item (quantity, reason, remarks)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateItem(int requestId, int lineItemId, int quantity, ReplenishmentReason reason, string remarks)
    {
        try
        {
            var success = _control.UpdateItem(requestId, lineItemId, quantity, reason, remarks ?? "");

            if (success)
            {
                TempData["SuccessMessage"] = "Item updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update item.";
            }

            return RedirectToAction(nameof(Form), new { id = requestId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating item {LineItemId}", lineItemId);
            TempData["ErrorMessage"] = "An error occurred while updating the item.";
            return RedirectToAction(nameof(Form), new { id = requestId });
        }
    }

    // POST: /ReplenishmentRequester/RemoveItem
    // Remove a line item from a replenishment request
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveItem(int requestId, int lineItemId)
    {
        try
        {
            var success = _control.RemoveItem(requestId, lineItemId);

            if (success)
            {
                TempData["SuccessMessage"] = "Item removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to remove item.";
            }

            return RedirectToAction(nameof(Form), new { id = requestId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing item {LineItemId}", lineItemId);
            TempData["ErrorMessage"] = "An error occurred while removing the item.";
            return RedirectToAction(nameof(Form), new { id = requestId });
        }
    }

    // POST: /ReplenishmentRequester/UpdateRequestRemarks
    // Update request-level remarks (distinct from line-item remarks)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateRequestRemarks(int requestId, string? remarks)
    {
        try
        {
            var success = _control.UpdateRequestRemarks(requestId, remarks);

            if (success)
            {
                TempData["SuccessMessage"] = "Request remarks updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update request remarks.";
            }

            return RedirectToAction(nameof(Form), new { id = requestId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating request remarks for request {RequestId}", requestId);
            TempData["ErrorMessage"] = "An error occurred while updating request remarks.";
            return RedirectToAction(nameof(Form), new { id = requestId });
        }
    }

    // POST: /ReplenishmentRequester/Submit/5
    // Submit a replenishment request (DRAFT → SUBMITTED)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Submit(int id)
    {
        try
        {
            var success = _control.SubmitRequest(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Request submitted successfully.";
                return RedirectToAction(nameof(Detail), new { id });
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to submit request. Ensure all line items are valid.";
                return RedirectToAction(nameof(Form), new { id });
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Form), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting request {RequestId}", id);
            TempData["ErrorMessage"] = "An error occurred while submitting the request.";
            return RedirectToAction(nameof(Form), new { id });
        }
    }

    // POST: /ReplenishmentRequester/Cancel/5
    // Cancel a replenishment request
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancel(int id)
    {
        try
        {
            var success = _control.CancelRequest(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Request cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel request.";
            }

            return RedirectToAction(nameof(Detail), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling request {RequestId}", id);
            TempData["ErrorMessage"] = "An error occurred while cancelling the request.";
            return RedirectToAction(nameof(Detail), new { id });
        }
    }

    // POST: /ReplenishmentRequester/Complete/5
    // Mark a request as completed (SUBMITTED → COMPLETED)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Complete(int id, string completedBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(completedBy))
            {
                TempData["ErrorMessage"] = "Staff ID is required to complete a request.";
                return RedirectToAction(nameof(Detail), new { id });
            }

            var success = _control.MarkRequestComplete(id, completedBy);

            if (success)
            {
                TempData["SuccessMessage"] = "Request marked as completed.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to complete request. Only submitted requests can be completed.";
            }

            return RedirectToAction(nameof(Detail), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing request {RequestId}", id);
            TempData["ErrorMessage"] = "An error occurred while completing the request.";
            return RedirectToAction(nameof(Detail), new { id });
        }
    }
}
