namespace ProRental.Interfaces.DTOs;

/// <summary>
/// EXTERNAL READ-ONLY CONTRACT: For Module 1 (e.g., Catalog Navigation Menus)
/// Provides category details without exposing the tracked EF Core entity.
/// </summary>
public record CategoryInfo(
    int CategoryId,
    string Name,
    string? Description
);