namespace ProRental.Interfaces.DTOs;

/// <summary>
/// EXTERNAL READ-ONLY CONTRACT: Module 1 & Customer Views
/// Flattens Product and ProductDetail into a single, immutable snapshot.
/// </summary>
public record ProductInfo(
    int ProductId,
    string Name,
    string Sku,
    string Status,       // Converted from enum to string for external safety
    decimal Price,
    string? Description,
    string? Image,
    int? AvailableQuantity // Calculated by InventoryManagementControl and inventoryservice
);