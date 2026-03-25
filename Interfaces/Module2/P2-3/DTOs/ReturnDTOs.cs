namespace ProRental.Interfaces.DTOs;

/// <summary>
/// Nested DTO: Represents a single item inside a return request.
/// </summary>
public record ReturnItemBasicInfo(
    int ReturnItemId,
    int InventoryItemId,
    string Status,        // Converted from ReturnItemStatus enum
    decimal ItemRepairCost, // Get from Damage report if have damage report to this item
    DateTime? CompletionDate
);

/// <summary>
/// EXTERNAL READ-ONLY CONTRACT: For Module 1 (e.g., Customer Order History / Returns Page)
/// </summary>
public record ReturnRequestInfo(
    int ReturnRequestId,
    int OrderId,
    decimal RequestTotalRepairCost, //Get from each returnitems
    DateTime RequestDate,
    DateTime? CompletionDate,
    string Status,        // Converted from ReturnRequestStatus enum
    List<ReturnItemBasicInfo> ReturnedItems // The nested list of items
);