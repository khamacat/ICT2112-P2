namespace ProRental.Domain.Module2.P2_2.Controls;

using ProRental.Interfaces;

/// <summary>
/// TEMPORARY stub implementation of IPurchaseOrderService.
/// Returns hardcoded PO data so you can test the full Transaction Logging flow
/// without waiting for your PO teammate's real implementation.
/// 
/// DELETE THIS CLASS once your teammate's real implementation is ready.
/// </summary>
public class StubPurchaseOrderService : IPurchaseOrderService
{
    public List<PurchaseOrderDTO> GetAllPurchaseOrders()
    {
        return new List<PurchaseOrderDTO>
        {
            new PurchaseOrderDTO
            {
                PoId = 1,
                PoDate = new DateTime(2026, 3, 1),
                SupplierId = 1,
                Status = "COMPLETED",
                ExpectedDeliveryDate = new DateTime(2026, 3, 5),
                TotalAmount = 5500.00m,
                DetailsJson = "{\"note\": \"Initial spring stock\", \"items\": [{\"product\": \"Canon R5 Body\", \"qty\": 5, \"unitPrice\": 1100.00}]}"
            },
            new PurchaseOrderDTO
            {
                PoId = 2,
                PoDate = new DateTime(2026, 3, 10),
                SupplierId = 2,
                Status = "CONFIRMED",
                ExpectedDeliveryDate = new DateTime(2026, 3, 20),
                TotalAmount = 2700.00m,
                DetailsJson = "{\"note\": \"Lens restock\", \"items\": [{\"product\": \"Sony 24-70mm f/2.8\", \"qty\": 3, \"unitPrice\": 900.00}]}"
            },
            new PurchaseOrderDTO
            {
                PoId = 3,
                PoDate = new DateTime(2026, 3, 15),
                SupplierId = 1,
                Status = "PENDING",
                ExpectedDeliveryDate = new DateTime(2026, 3, 25),
                TotalAmount = 1800.00m,
                DetailsJson = "{\"note\": \"Accessory bundle\", \"items\": [{\"product\": \"DJI RS3 Gimbal\", \"qty\": 3, \"unitPrice\": 600.00}]}"
            }
        };
    }
}