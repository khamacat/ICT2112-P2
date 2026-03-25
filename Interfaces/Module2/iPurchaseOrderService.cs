namespace ProRental.Interfaces;

/// <summary>
/// Service interface for retrieving Purchase Order data.
/// This interface is owned by your PO teammate — they will implement it
/// in their Control class. You consume it in TransactionLogControl to
/// pull and log PO data.
/// 
/// TODO: Coordinate with your PO teammate to confirm these method signatures.
///       Replace StubPurchaseOrderService with their real implementation when ready.
/// </summary>
public interface IPurchaseOrderService
{
    /// <summary>
    /// Returns all purchase orders.
    /// </summary>
    List<PurchaseOrderDTO> GetAllPurchaseOrders();
}

/// <summary>
/// Lightweight data transfer object for purchase order data.
/// This avoids depending on the scaffolded Purchaseorder entity directly,
/// which has private properties.
/// 
/// Your PO teammate populates this from their data, you consume it for logging.
/// </summary>
public class PurchaseOrderDTO
{
    public int PoId { get; set; }
    public DateTime? PoDate { get; set; }
    public int? SupplierId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ExpectedDeliveryDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? DetailsJson { get; set; }
}