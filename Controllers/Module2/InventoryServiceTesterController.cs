using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module2;

[Route("[controller]")]
public class InventoryServiceTesterController : Controller
{
    private readonly IInventoryService _inventoryService;

    // Inject your beautiful Facade!
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
        // 1. Setup fake requirements
        var productQuantities = new Dictionary<int, int> 
        { 
            { productId, quantity } 
        };
        var startDate = DateTime.UtcNow;
        var dueDate = DateTime.UtcNow.AddDays(7);

        // 2. FIRE THE FACADE
        bool result = _inventoryService.ProcessLoan(orderId, customerId, startDate, dueDate, productQuantities);

        // 3. Report Results
        if (result)
        {
            TempData["SuccessMessage"] = $"✅ SUCCESS: ProcessLoan completed! Order {orderId} created. Items allocated and status updated to ON_LOAN.";
        }
        else
        {
            TempData["ErrorMessage"] = $"❌ FAILED: ProcessLoan aborted. Reasons could include: Not enough AVAILABLE stock for Product {productId}, or Order {orderId} already exists in the Loan list.";
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
            TempData["SuccessMessage"] = $"✅ SUCCESS: TriggerReturnProcess completed! Loan for Order {orderId} closed, Return triggered, and all physical items set to MAINTENANCE.";
        }
        else
        {
            TempData["ErrorMessage"] = $"❌ FAILED: TriggerReturnProcess aborted. Check if Order {orderId} actually has an active loan.";
        }

        return RedirectToAction(nameof(Index));
    }
}