using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Clearanceitem
{
    private ClearanceStatus _status;
    private ClearanceStatus Status { get => _status; set => _status = value; }

    // ── Public Accessors ───────────────────────────────────────────────────────

    public int GetClearanceItemId() => _clearanceitemid;
    internal void SetClearanceItemId(int value) => _clearanceitemid = value;

    public int GetClearanceBatchId() => _clearancebatchid;
    internal void SetClearanceBatchId(int value) => _clearancebatchid = value;

    public int GetInventoryItemId() => _inventoryitemid;
    internal void SetInventoryItemId(int value) => _inventoryitemid = value;

    public decimal? GetFinalPrice() => _finalprice;
    internal void SetFinalPrice(decimal? value) => _finalprice = value;

    public decimal? GetRecommendedPrice() => _recommendedprice;
    internal void SetRecommendedPrice(decimal? value) => _recommendedprice = value;

    public DateTime? GetSaleDate() => _saledate;
    internal void SetSaleDate(DateTime? value) => _saledate = value;

    public ClearanceStatus GetStatus() => _status;
    internal void UpdateStatus(ClearanceStatus newValue) => _status = newValue;
}