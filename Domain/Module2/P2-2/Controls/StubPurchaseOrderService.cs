namespace ProRental.Domain.Module2.P2_2.Controls;

using ProRental.Interfaces;

public class StubPurchaseOrderService : IPurchaseOrderService
{
    public List<PurchaseOrderDTO> GetAllPurchaseOrders()
    {
        return new List<PurchaseOrderDTO>
        {
            new PurchaseOrderDTO
            {
                PoId = 1,
                PoDate = DateTime.SpecifyKind(new DateTime(2026, 3, 1), DateTimeKind.Utc),
                SupplierId = 1,
                Status = "APPROVED",
                ExpectedDeliveryDate = DateTime.SpecifyKind(new DateTime(2026, 3, 5), DateTimeKind.Utc),
                TotalAmount = 5500.00m,
                DetailsJson = "{\"status\":\"APPROVED\",\"supplierName\":\"Camera Supply Co\",\"lineItems\":[{\"productId\":1,\"productName\":\"Canon R5 Body\",\"qty\":5,\"unitPrice\":1100.00,\"lineTotal\":5500.00}]}"
            },
            new PurchaseOrderDTO
            {
                PoId = 2,
                PoDate = DateTime.SpecifyKind(new DateTime(2026, 3, 10), DateTimeKind.Utc),
                SupplierId = 2,
                Status = "PENDING",
                ExpectedDeliveryDate = DateTime.SpecifyKind(new DateTime(2026, 3, 20), DateTimeKind.Utc),
                TotalAmount = 2700.00m,
                DetailsJson = "{\"status\":\"PENDING\",\"supplierName\":\"Lens Warehouse Ltd\",\"lineItems\":[{\"productId\":2,\"productName\":\"Canon RF 24-70mm f/2.8\",\"qty\":3,\"unitPrice\":900.00,\"lineTotal\":2700.00}]}"
            },
            new PurchaseOrderDTO
            {
                PoId = 3,
                PoDate = DateTime.SpecifyKind(new DateTime(2026, 3, 15), DateTimeKind.Utc),
                SupplierId = 1,
                Status = "APPROVED",
                ExpectedDeliveryDate = DateTime.SpecifyKind(new DateTime(2026, 3, 25), DateTimeKind.Utc),
                TotalAmount = 1800.00m,
                DetailsJson = "{\"status\":\"APPROVED\",\"supplierName\":\"Camera Supply Co\",\"lineItems\":[{\"productId\":3,\"productName\":\"Sony A7IV Body\",\"qty\":2,\"unitPrice\":900.00,\"lineTotal\":1800.00}]}"
            }
        };
    }
}