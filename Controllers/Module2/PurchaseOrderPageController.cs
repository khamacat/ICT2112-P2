using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces;

namespace ProRental.Controllers
{
    public class PurchaseOrderPageController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderPageController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpGet]
        public IActionResult Index(int reqId = 0, int? poId = null)
        {
            if (reqId <= 0)
            {
                var vm = new PurchaseOrderPageViewModel
                {
                    Requests = _purchaseOrderService.GetAllRequests(),
                    PurchaseOrders = _purchaseOrderService.GetAllPurchaseOrders(),
                    CreatedPoId = poId
                };

                return View("~/Views/Module2/PurchaseOrder.cshtml", vm);
            }

            var loadedVm = _purchaseOrderService.GetPurchaseOrderPageData(reqId);
            loadedVm.Requests = _purchaseOrderService.GetAllRequests();
            loadedVm.PurchaseOrders = _purchaseOrderService.GetAllPurchaseOrders();
            loadedVm.CreatedPoId = poId;

            return View("~/Views/Module2/PurchaseOrder.cshtml", loadedVm);
        }

        [HttpGet]
        public IActionResult PurchaseOrderView(int reqId)
        {
            if (reqId <= 0)
            {
                TempData["Error"] = "Invalid request ID.";
                return RedirectToAction(nameof(Index));
            }

            var loadedVm = _purchaseOrderService.GetPurchaseOrderPageData(reqId);
            loadedVm.Requests = _purchaseOrderService.GetAllRequests();
            loadedVm.PurchaseOrders = _purchaseOrderService.GetAllPurchaseOrders();

            return View("~/Views/Module2/PurchaseOrderView.cshtml", loadedVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmPO(int reqId, int supplierId, DateOnly? expectedDeliveryDate, bool confirmDetails = false)
        {
            if (reqId <= 0)
            {
                TempData["Error"] = "Invalid request ID.";
                return RedirectToAction(nameof(Index));
            }

            if (supplierId <= 0)
            {
                TempData["Error"] = "Please select a supplier.";
                return RedirectToAction(nameof(Index), new { reqId });
            }

            if (!confirmDetails)
            {
                TempData["Error"] = "Please confirm the purchase order details.";
                return RedirectToAction(nameof(Index), new { reqId });
            }

            try
            {
                int poId = _purchaseOrderService.ConfirmPurchaseOrder(reqId, supplierId, expectedDeliveryDate);
                TempData["Success"] = $"Purchase Order #{poId} created successfully.";

                return RedirectToAction(nameof(Index), new { reqId, poId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to create purchase order: {ex.Message}";
                return RedirectToAction(nameof(Index), new { reqId });
            }
        }
    }
}