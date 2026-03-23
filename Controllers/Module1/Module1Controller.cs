using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Controls;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

/// <summary>
/// ASP.NET HTTP controller for Module 1 (authentication, session, customer validation).
/// This is a thin HTTP boundary only — all business logic lives in the Control classes.
/// </summary>
public class Module1Controller : Controller
{
    private readonly AuthenticationControl _authControl;
    private readonly CustomerIDValidationControl _customerIdValidationControl;

    // private readonly IOrderService _orderService;

    public Module1Controller(
        AuthenticationControl authControl,
        CustomerIDValidationControl customerIdValidationControl)
    {
        _authControl = authControl;
        _customerIdValidationControl = customerIdValidationControl;
        // _orderService = orderService;
    }

    // ── Login ────────────────────────────────────────────────────────────

    // GET /Module1/Login
    public IActionResult Login()
    {
        return View("P2-6/Login");
    }

    // POST /Module1/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password)
    {
        var result = _authControl.AuthenticateUser(email, password);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Login failed.");
            return View("P2-6/Login");
        }

        HttpContext.Session.SetInt32("SessionId", result.Session!.SessionId);
        HttpContext.Session.SetString("UserName", result.UserName ?? email);
        HttpContext.Session.SetString("UserRole", result.Session.RoleString);

        // Redirect to customer success page; action guard handles fallback to Home.
        return RedirectToAction("CustomerLoginSuccess");
    }

    // POST /Module1/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var sessionId = HttpContext.Session.GetInt32("SessionId");
        if (sessionId.HasValue)
            _authControl.Logout(sessionId.Value);

        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    // ── Customer ID Validation ───────────────────────────────────────────

    // GET /Module1/CustomerIdEntry
    public IActionResult CustomerIdEntry()
    {
        return View("P2-6/_CustomerIdEntry");
    }

    // POST /Module1/CustomerIdEntry
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CustomerIdEntry(int customerId)
    {
        var result = _customerIdValidationControl.ValidateCustomer(customerId);

        if (!result.IsValid)
        {
            ViewBag.ValidationMessage = result.ValidationMessage;
            return View("P2-6/CustomerIdEntry");
        }

        // Store the validated customer ID for the downstream checkout flow.
        HttpContext.Session.SetInt32("ValidatedCustomerId", result.CustomerId);
        return RedirectToAction("Index", "Cart");
    }

    // ── Customer Login Success ───────────────────────────────────────────

    // GET /Module1/CustomerLoginSuccess
    public IActionResult CustomerLoginSuccess()
    {
        // Guard: only accessible if a valid customer session exists.
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role) ||
            !role.Equals("CUSTOMER", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction("Login");
        }

        // Attempt to serve the dedicated success view; fall back to Home/Index
        // if the view file has not been created yet.
        var viewPath = "P2-6/CustomerLoginSuccess";
        var viewExists = ViewExists(viewPath);

        return viewExists
            ? View(viewPath)
            : RedirectToAction("Index", "Home");
    }

    // ── Staff Login ──────────────────────────────────────────────────────

    // GET /Module1/StaffLogin
    public IActionResult StaffLogin()
    {
        return View("P2-6/StaffLogin");
    }

    // POST /Module1/StaffLogin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StaffLogin(string StaffEmail, string StaffPassword)
    {
        var result = _authControl.AuthenticateUser(StaffEmail, StaffPassword);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Staff login failed.");
            return View("P2-6/StaffLogin");
        }

        // Reject non-staff accounts that attempt to use the staff portal.
        var roleString = result.Session!.RoleString;
        if (!Enum.TryParse<UserRole>(roleString, ignoreCase: true, out var role) ||
            (role != UserRole.STAFF && role != UserRole.ADMIN))
        {
            ModelState.AddModelError(string.Empty,
                "Access denied. This portal is for staff and administrators only.");
            return View("P2-6/StaffLogin");
        }

        HttpContext.Session.SetInt32("SessionId", result.Session.SessionId);
        HttpContext.Session.SetString("UserName", result.UserName ?? StaffEmail);
        HttpContext.Session.SetString("UserRole", roleString);

        // Redirect staff to the staff success / dashboard page.
        return RedirectToAction("StaffLoginSuccess");
    }

    // GET /Module1/StaffLoginSuccess
    public IActionResult StaffLoginSuccess()
    {
        // Guard: only accessible if a valid staff session exists.
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role) ||
            (!role.Equals("STAFF", StringComparison.OrdinalIgnoreCase) &&
             !role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)))
        {
            return RedirectToAction("StaffLogin");
        }

        return View("P2-6/StaffLoginSuccess");
    }

    // ── Signup ───────────────────────────────────────────────────────────

    // GET /Module1/Signup
    public IActionResult Signup()
    {
        return View("P2-6/Signup");
    }

    // POST /Module1/Signup
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Signup(
        string firstName,
        string lastName,
        string email,
        string? phone,
        string password,
        string confirmPassword,
        bool agreeTerms)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match.");
            return View("P2-6/Signup");
        }

        if (!agreeTerms)
        {
            ModelState.AddModelError(string.Empty, "You must agree to the Terms of Service and Privacy Policy.");
            return View("P2-6/Signup");
        }

        // TODO: wire up SignupControl here when backend is ready.

        TempData["SignupName"] = firstName;
        return RedirectToAction("SignupSuccess");
    }

    // GET /Module1/SignupSuccess
    public IActionResult SignupSuccess()
    {
        return View("P2-6/SignupSuccess");
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    /// <summary>
    /// Returns true if the Razor view at the given path can be located by the view engine.
    /// Used to enable graceful fallback when an optional view has not yet been created.
    /// </summary>
    private bool ViewExists(string viewPath)
    {
        var result = HttpContext.RequestServices
            .GetRequiredService<Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine>()
            .FindView(ControllerContext, viewPath, isMainPage: false);

        return result.Success;
    }
}

//     // ── Order Management ──────────────────────────────────────────────────────

// // GET /Module1/Orders?customerId=1&status=all
// public IActionResult Orders(int customerId = 1, string status = "all")
// {
//     var orders = _orderService.GetOrdersByCustomer(customerId);

//     // Filter by status tab
//     var filtered = status.ToLower() switch
//     {
//         "pending"   => orders.Where(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.PENDING).ToList(),
//         "confirmed" => orders.Where(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.CONFIRMED ||
//                                          o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.PROCESSING).ToList(),
//         "dispatch"  => orders.Where(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.READY_FOR_DISPATCH ||
//                                          o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.DISPATCHED).ToList(),
//         "delivered" => orders.Where(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.DELIVERED).ToList(),
//         "cancelled" => orders.Where(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.CANCELLED).ToList(),
//         _           => orders
//     };

//     ViewBag.CustomerId  = customerId;
//     ViewBag.ActiveTab   = status;
//     ViewBag.AllCount        = orders.Count;
//     ViewBag.PendingCount    = orders.Count(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.PENDING);
//     ViewBag.ConfirmedCount  = orders.Count(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.CONFIRMED ||
//                                                  o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.PROCESSING);
//     ViewBag.DispatchCount   = orders.Count(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.READY_FOR_DISPATCH ||
//                                                  o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.DISPATCHED);
//     ViewBag.DeliveredCount  = orders.Count(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.DELIVERED);
//     ViewBag.CancelledCount  = orders.Count(o => o.CurrentStatus == ProRental.Domain.Enums.OrderStatus.CANCELLED);

//     return View("P2-6/Orders", filtered);
// }

// // GET /Module1/OrderDetail/5
// public IActionResult OrderDetail(int orderId)
// {
//     var order = _orderService.GetOrder(orderId);
//     return View("P2-6/OrderDetail", order);
// }

// // POST /Module1/CancelOrder
// [HttpPost]
// [ValidateAntiForgeryToken]
// public IActionResult CancelOrder(int orderId, int customerId = 1)
// {
//     _orderService.CancelOrder(orderId);
//     return RedirectToAction("Orders", new { customerId, status = "cancelled" });
// }

// // GET /Module1/CreateOrderTest
// public IActionResult CreateOrderTest()
// {
//     return View("P2-6/CreateOrderTest");
// }

// // POST /Module1/CreateOrderTest
// [HttpPost]
// [ValidateAntiForgeryToken]
// public IActionResult CreateOrderTest(int customerId, int checkoutId,
//     string deliveryType, decimal totalAmount,
//     int productId1, int quantity1, decimal unitPrice1,
//     int productId2, int quantity2, decimal unitPrice2)
// {
//     var itemData = new List<(int, int, decimal, DateTime, DateTime)>
//     {
//         (productId1, quantity1, unitPrice1, DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
//         (productId2, quantity2, unitPrice2, DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
//     };

//     var productQuantities = new Dictionary<int, int>
//     {
//         { productId1, quantity1 },
//         { productId2, quantity2 }
//     };

//     var delivery = Enum.Parse<ProRental.Domain.Enums.DeliveryDuration>(deliveryType);

//     var order = _orderService.CreateOrder(customerId, checkoutId, itemData,
//                                            delivery, totalAmount, productQuantities);

//     TempData["CreatedOrderId"]     = order.OrderId;
//     TempData["CreatedOrderStatus"] = order.CurrentStatus?.ToString();

//     return RedirectToAction("OrderDetail", new { orderId = order.OrderId });
// }

// }