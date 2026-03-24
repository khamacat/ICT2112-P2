using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers;

// ── CUSTOM AUTH FILTER (Share this with your team!) ───────────────────────
public class StaffAuthAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = context.HttpContext.Session.GetString("UserRole");
        
        // If they aren't logged in, or aren't STAFF/ADMIN, kick them to login
        if (string.IsNullOrEmpty(role) || 
           (!role.Equals("STAFF", StringComparison.OrdinalIgnoreCase) && 
            !role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)))
        {
            // Redirects to Module1Controller -> StaffLogin
            context.Result = new RedirectToActionResult("StaffLogin", "Module1", null);
        }
        
        base.OnActionExecuting(context);
    }
}
// ──────────────────────────────────────────────────────────────────────────

[StaffAuth] // <-- This single line protects the ENTIRE controller!
public class LoanListController : Controller
{
    private readonly ILoanListQuery _loanListQuery;
    private readonly ILoanListCRUD _loanListCRUD;
    private readonly ILoanItemQuery _loanItemQuery;
    private readonly ILoanItemCRUD _loanItemCRUD;

    public LoanListController(
        ILoanListQuery loanListQuery, 
        ILoanListCRUD loanListCRUD,
        ILoanItemQuery loanItemQuery,
        ILoanItemCRUD loanItemCRUD)
    {
        _loanListQuery = loanListQuery;
        _loanListCRUD = loanListCRUD;
        _loanItemQuery = loanItemQuery;
        _loanItemCRUD = loanItemCRUD;
    }

    // ── View 1: All Loan Lists (Dashboard) ───────────────────────────────────
    
    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index(string? statusFilter)
    {
        var allLoans = _loanListQuery.GetAllLoanList() ?? new List<Loanlist>();

        // Apply Status Filter if provided
        if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse<LoanStatus>(statusFilter, true, out var parsedStatus))
        {
            allLoans = allLoans.Where(l => l.GetStatus() == parsedStatus).ToList();
            ViewData["CurrentFilter"] = parsedStatus.ToString();
        }
        else
        {
            ViewData["CurrentFilter"] = "ALL";
        }

        // Sort latest on top (using LoanDate)
        var sortedLoans = allLoans.OrderByDescending(l => l.GetLoanDate()).ToList();

        return View("~/Views/Module2/Loan/Index.cshtml", sortedLoans);
    }

    // ── View 2: Single Loan List Details ─────────────────────────────────────

    [HttpGet("Details/{id:int}")]
    public IActionResult Details(int id)
    {
        var loanList = _loanListQuery.GetLoanListById(id);
        if (loanList == null)
        {
            TempData["ErrorMessage"] = "Loan list not found.";
            return RedirectToAction(nameof(Index));
        }

        // Fetch the child items to display in the view
        var childItems = _loanItemQuery.GetAllLoanItems()?
            .Where(i => i.GetLoanListId() == id).ToList() ?? new List<Loanitem>();

        ViewBag.ChildItems = childItems;

        return View("~/Views/Module2/Loan/Details.cshtml", loanList);
    }

    [HttpPost("UpdateListRemarks")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateListRemarks(int loanListId, string remarks)
    {
        var loanList = _loanListQuery.GetLoanListById(loanListId);
        if (loanList != null)
        {
            loanList.UpdateRemarks(remarks); // The public method we added earlier
            _loanListCRUD.UpdateLoanList(loanList);
            TempData["SuccessMessage"] = "Remarks updated successfully.";
        }
        
        return RedirectToAction(nameof(Details), new { id = loanListId });
    }

    // ── View 3: Single Loan Item Details ─────────────────────────────────────

    [HttpGet("ItemDetails/{id:int}")]
    public IActionResult ItemDetails(int id)
    {
        var loanItem = _loanItemQuery.GetLoanItemById(id);
        if (loanItem == null)
        {
            TempData["ErrorMessage"] = "Loan item not found.";
            return RedirectToAction(nameof(Index));
        }

        // Fetch the parent list just so the View can display the Order ID or dates if needed
        var parentList = _loanListQuery.GetLoanListById(loanItem.GetLoanListId());
        ViewBag.ParentList = parentList;

        return View("~/Views/Module2/Loan/ItemDetails.cshtml", loanItem);
    }

    [HttpPost("UpdateItemRemarks")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateItemRemarks(int loanItemId, string remarks)
    {
        var loanItem = _loanItemQuery.GetLoanItemById(loanItemId);
        if (loanItem != null)
        {
            loanItem.UpdateRemarks(remarks); // The public method we added earlier
            _loanItemCRUD.UpdateLoanItem(loanItem);
            TempData["SuccessMessage"] = "Item remarks updated successfully.";
        }
        
        return RedirectToAction(nameof(ItemDetails), new { id = loanItemId });
    }
}