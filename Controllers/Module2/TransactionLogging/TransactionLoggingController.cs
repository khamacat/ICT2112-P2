using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces;
using ProRental.Data.Module2.Interfaces;
using ProRental.Domain.Entities;

namespace ProRental.Controllers;

/// <summary>
/// Controller for the Transaction Logging feature.
/// Displays all transaction logs with filtering and inline detail expansion.
/// </summary>
public class TransactionLoggingController : Controller
{
    private readonly ITransactionLoggingUI _loggingUI;
    private readonly ILoanLogGateway _loanLogGateway;
    private readonly IReturnLogGateway _returnLogGateway;       

    public TransactionLoggingController(
        ITransactionLoggingUI loggingUI  ,  
        ILoanLogGateway loanLogGateway,
    IReturnLogGateway returnLogGateway)
    {
            _loggingUI = loggingUI;
    _loanLogGateway = loanLogGateway;
    _returnLogGateway = returnLogGateway;
    }

    /// <summary>
    /// Main page — shows all transaction logs in a table.
    /// Also triggers PO pull on load.
    /// </summary>
    public IActionResult Index()
    {
        var logs = _loggingUI.GetAllLogs();
        return View("~/Views/Module2/TransactionLogging/Index.cshtml", logs);
    }

    /// <summary>
    /// Handles filter form submission.
    /// Returns the same Index view with filtered results.
    /// </summary>
    [HttpGet]
    public IActionResult Filter(string filterType, string filterValue,
                                 string? startDate, string? endDate)
    {
        // For date range, combine start and end into pipe-delimited string
        if (filterType == "daterange" && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        {
            filterValue = $"{startDate}|{endDate}";
        }

        // If no filter value provided, show all logs
        if (string.IsNullOrWhiteSpace(filterValue))
        {
            var allLogs = _loggingUI.GetAllLogs();
            ViewBag.FilterMessage = "No filter value provided. Showing all logs.";
            return View("~/Views/Module2/TransactionLogging/Index.cshtml", allLogs);
        }

        var logs = _loggingUI.GetFilteredLogs(filterType, filterValue);

        // Pass filter info to the view for display
        ViewBag.FilterType = filterType;
        ViewBag.FilterValue = filterValue;
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.FilterMessage = $"Showing {logs.Count} result(s) filtered by {filterType}.";

        return View("~/Views/Module2/TransactionLogging/Index.cshtml", logs);
    }

    /// <summary>
    /// Returns the detail data for a single log entry as a partial view.
    /// Called via AJAX when a user clicks a row to expand details.
    /// </summary>
[HttpGet]
public IActionResult Details(int id)
{
    var log = _loggingUI.GetLogDetails(id);
    if (log == null)
        return NotFound();

    // If this is a Rental Order, fetch linked loans and returns
    if (log.Rentalorderlog != null)
    {
        ViewBag.LinkedLoans = _loanLogGateway.GetByRentalOrderLogId(log.transaction_logid);
        ViewBag.LinkedReturns = _returnLogGateway.GetByRentalOrderLogId(log.transaction_logid);
    }

    return View("~/Views/Module2/TransactionLogging/Details.cshtml", log);
}
}