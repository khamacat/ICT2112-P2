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
        public IActionResult Index(int reqId = 0)
        {
            if (reqId != 0)
            {
                var po = _purchaseOrderService.GetPOByRequestId(reqId);
                if (po != null)
                {
                    ViewBag.RequestId = reqId;
                    return View("~/Views/Module2/PurchaseOrder.cshtml", po);
                }
            }

            ViewBag.RequestId = reqId;
            return View("~/Views/Module2/PurchaseOrder.cshtml");
        }

        [HttpGet]
        public IActionResult EligibleSuppliers(int stockId, int qty)
        {
            var suppliers = _purchaseOrderService.GetEligibleSuppliers(stockId, qty);
            return Json(suppliers);
        }

        [HttpPost]
        public IActionResult BuildDraft(int reqId, int supplierId)
        {
            try
            {
                var po = _purchaseOrderService.BuildPODraft(reqId, supplierId);
                ViewBag.RequestId = reqId;
                return View("~/Views/Module2/PurchaseOrder.cshtml", po);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index), new { reqId });
            }
        }

        [HttpPost]
        public IActionResult Confirm(int reqId, int supplierId, DateOnly? expectedDeliveryDate)
        {
            try
            {
                var po = _purchaseOrderService.BuildPODraft(reqId, supplierId);
                int poId = _purchaseOrderService.ConfirmPO(reqId, po);

                if (expectedDeliveryDate.HasValue)
                {
                    _purchaseOrderService.RecordExpectedDeliveryDate(poId, expectedDeliveryDate.Value);
                }

                return RedirectToAction(nameof(Details), new { poId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index), new { reqId });
            }
        }

        [HttpGet]
        public IActionResult Details(int poId)
        {
            var po = _purchaseOrderService.GetPOById(poId);
            if (po == null) return NotFound();

            return View("~/Views/Module2/PurchaseOrder.cshtml", po);
        }
        
    }
}