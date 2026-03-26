using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module2;

[Route("[controller]")]
public class InventoryServiceTesterController : Controller
{
    private readonly IInventoryService _inventoryService;

    public InventoryServiceTesterController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        return View("~/Views/Module2/Tester/Index.cshtml");
    }

    [HttpPost("TestProcessLoan")]
    public IActionResult TestProcessLoan(int orderId, int customerId, int productId, int quantity)
    {
        // 1. Setup requirements mapped to the demo seed parameters
        var productQuantities = new Dictionary<int, int> 
        { 
            { productId, quantity } 
        };
        var startDate = DateTime.UtcNow.AddDays(1);
        var dueDate = DateTime.UtcNow.AddDays(5);

        // 2. FIRE THE FACADE
        bool result = _inventoryService.ProcessLoan(orderId, customerId, startDate, dueDate, productQuantities);

        // 3. Report Results
        if (result)
        {
            TempData["SuccessMessage"] = $"ProcessLoan completed! Order {orderId} processed. Items allocated, status updated to ON_LOAN, and cross-domain Logs generated. (If you used Order 4, a Low Stock Alert was just triggered!)";
        }
        else
        {
            TempData["ErrorMessage"] = $"ProcessLoan aborted. Reasons: Not enough AVAILABLE stock, or Order {orderId} already has an active Loan.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("TestTriggerReturn")]
    public IActionResult TestTriggerReturn(int orderId)
    {
        // 1. FIRE THE FACADE
        bool result = _inventoryService.TriggerReturnProcess(orderId, DateTime.UtcNow);

        // 2. Report Results
        if (result)
        {
            TempData["SuccessMessage"] = $"TriggerReturnProcess completed! Loan for Order {orderId} closed, Return Request generated, and items sent to Damage Inspection Pipeline.";
        }
        else
        {
            TempData["ErrorMessage"] = $"TriggerReturnProcess aborted. Check if Order {orderId} actually has an active loan.";
        }

        return RedirectToAction(nameof(Index));
    }
}