namespace ProRental.Controllers
{
    public class PurchaseOrderPageViewModel
    {
        public int RequestId { get; set; }
        public string RequestedBy { get; set; } = "";
        public DateTime? CreatedAt { get; set; }
        public string Remarks { get; set; } = "";
        public string Status { get; set; } = "";

        public List<PurchaseOrderItemViewModel> Items { get; set; } = new();
        public List<PurchaseOrderSupplierViewModel> Suppliers { get; set; } = new();
        public List<PurchaseOrderRequestListItemViewModel> Requests { get; set; } = new();

        public List<PurchaseOrderListItemViewModel> PurchaseOrders { get; set; } = new();

        public int? CreatedPoId { get; set; }
    }

    public class PurchaseOrderItemViewModel
    {
        public int LineItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Qty { get; set; }
        public string Remarks { get; set; } = "";
    }

    public class PurchaseOrderSupplierViewModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = "";
        public string Details { get; set; } = "";
        public int? CreditPeriod { get; set; }
        public double? AvgTurnaroundTime { get; set; }
        public bool IsVerified { get; set; }
    }

    public class PurchaseOrderRequestListItemViewModel
    {
        public int RequestId { get; set; }
        public string RequestedBy { get; set; } = "";
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; } = "";
        public string Remarks { get; set; } = "";
    }

    public class PurchaseOrderListItemViewModel
    {
        public int PoId { get; set; }
        public int SupplierId { get; set; }
        public DateTime? PoDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = "";
        public decimal? TotalAmount { get; set; }
    }
}