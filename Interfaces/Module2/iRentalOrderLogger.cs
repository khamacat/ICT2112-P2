namespace ProRental.Interfaces;

/// <summary>
/// Push interface for P2-6 (Order module) to log rental orders.
/// Implemented by TransactionLogControl.
/// 
/// Usage by P2-6:
///   _rentalOrderLogger.pushRentalOrderData(orderId, customerId, orderDate, ...);
/// </summary>
public interface IRentalOrderLogger
{
    /// <summary>
    /// Logs a rental order transaction. Creates a TransactionLog parent row
    /// and a RentalOrderLog child row.
    /// </summary>
    void pushRentalOrderData(int orderId, int customerId, DateTime orderDate,
                             decimal totalAmount, string deliveryType, string status,
                             string? detailsJson);
}