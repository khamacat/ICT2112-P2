namespace ProRental.Interfaces.DTOs;

/// <summary>
/// EXTERNAL READ-ONLY CONTRACT: For Module 1 (e.g., Order Tracking / Dispatch details)
/// Provides details of a specific physical item without exposing the tracked EF Core entity.
/// </summary>
public record InventoryItemInfo(
    int InventoryItemId,
    int ProductId,
    string ProductName,
    string SerialNumber,
    string Status,          // Converted from InventoryStatus enum
    DateTime? ExpiryDate
);