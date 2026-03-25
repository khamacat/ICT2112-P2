using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Module2;

// Query interface for cross-module access to Replenishment Requests
// Used by PurchaseOrder module to read replenishment request data
// Based on Team 2-2 Class Diagram specification
public interface IReplenishmentRequestQuery
{
    // Get a replenishment request by ID
    // <param name="reqId">The request ID</param>
    // <returns>The ReplenishmentRequest or null if not found</returns>
    Replenishmentrequest? GetRequest(int reqId);

    // Get all line items for a replenishment request
    // <param name="reqId">The request ID</param>
    // <returns>List of line items</returns>
    List<Lineitem> GetLineItems(int reqId);
}
