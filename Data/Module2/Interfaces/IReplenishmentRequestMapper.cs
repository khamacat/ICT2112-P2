using ProRental.Domain.Entities;

namespace ProRental.Data.Module2.Interfaces;


// Data Mapper interface for ReplenishmentRequest
// Handles database operations for replenishment requests and line items
public interface IReplenishmentRequestMapper
{

    // Insert a new replenishment request into the database
    // <param name="request">The request to insert</param>
    void Insert(Replenishmentrequest request);

    // Update an existing replenishment request in the database
    // <param name="request">The request to update</param>
    void Update(Replenishmentrequest request);

    // Find a replenishment request by ID including its line items
    // <param name="id">The request ID</param>
    // <returns>The ReplenishmentRequest or null if not found</returns>
    Replenishmentrequest? FindById(int id);

    // Find all replenishment requests including their line items
    // <returns>List of all replenishment requests</returns>
    List<Replenishmentrequest> FindAll();

    // Find all line items for a specific replenishment request
    // <param name="requestId">The request ID</param>
    // <returns>List of line items</returns>
    List<Lineitem> FindLineItems(int requestId);
}