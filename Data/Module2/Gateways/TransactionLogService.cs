using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Interfaces;
using System.Text.Json;

namespace ProRental.Data.Gateways;

public class TransactionLogService : ITransactionLogService
{
    private readonly AppDbContext _db;
    public TransactionLogService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<TransactionLogDto>> GetAllTransactionLogsAsync()
        => await FetchLogsAsync();

    public async Task<IEnumerable<TransactionLogDto>> GetTransactionLogsByDateRangeAsync(
        DateTime start, DateTime end)
        => (await FetchLogsAsync()).Where(t => t.CreatedAt >= start && t.CreatedAt <= end);

    private async Task<List<TransactionLogDto>> FetchLogsAsync()
    {
        var results = await _db.Database
            .SqlQueryRaw<TransactionLogRaw>(@"
                SELECT
                    t.transactionlogid AS Id,
                    t.logtype          AS LogType,
                    t.createdat        AS CreatedAt,
                    COALESCE(
                        rol.detailsjson,
                        ll.detailsjson,
                        rl.detailsjson,
                        pol.detailsjson,
                        cl.detailsjson
                    ) AS DetailsJson
                FROM transactionlog t
                LEFT JOIN rentalorderlog   rol ON rol.rentalorderlogid   = t.transactionlogid
                LEFT JOIN loanlog          ll  ON ll.loanlogid           = t.transactionlogid
                LEFT JOIN returnlog        rl  ON rl.returnlogid         = t.transactionlogid
                LEFT JOIN purchaseorderlog pol ON pol.purchaseorderlogid = t.transactionlogid
                LEFT JOIN clearancelog     cl  ON cl.clearancelogid      = t.transactionlogid
                ORDER BY t.transactionlogid")
            .ToListAsync();

        return results.Select(r =>
        {
            var (supplier, products) = ExtractFromJson(r.DetailsJson);
            var groupedProducts = products
                .GroupBy(p => p)
                .Select(g => g.Count() > 1 ? $"{g.Key} x{g.Count()}" : g.Key)
                .ToList();
            return new TransactionLogDto
            {
                LogID        = r.Id,
                LogType      = r.LogType ?? "UNKNOWN",
                CreatedAt    = r.CreatedAt ?? DateTime.UtcNow,
                SupplierName = supplier,
                ProductNames = groupedProducts,
                Summary      = $"{r.LogType ?? "LOG"} #{r.Id}"
            };
        }).ToList();
    }

    /// <summary>
    /// Extracts first supplier and first product from detailsjson.
    /// Format: {"suppliers": ["Apple"], "products": ["Canon R5"], ...}
    /// Falls back to "items" if "products" not found.
    /// Returns null for each if key absent or JSON malformed.
    /// </summary>
    private static (string? supplier, List<string> products) ExtractFromJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return (null, new());
        try
        {
            using var doc    = JsonDocument.Parse(json);
            var root         = doc.RootElement;
            string? supplier = null;
            var products     = new List<string>();

            // "supplierName": "Acme Corp"
            if (root.TryGetProperty("supplierName", out var sName) && sName.ValueKind == JsonValueKind.String)
                supplier = sName.GetString();
            // "suppliers": ["Acme Corp", ...]
            else if (root.TryGetProperty("suppliers", out var sArr)
                && sArr.ValueKind == JsonValueKind.Array && sArr.GetArrayLength() > 0)
                supplier = sArr[0].GetString();

            // "lineItems": [{"productName": "Canon R5", ...}, ...]
            if (root.TryGetProperty("lineItems", out var lineItems)
                && lineItems.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in lineItems.EnumerateArray())
                    if (item.TryGetProperty("productName", out var p) && p.ValueKind == JsonValueKind.String)
                        products.Add(p.GetString()!);
            }
            // "items": [{"productName": "Canon R5", ...}, ...] or ["Canon R5", ...]
            else if (root.TryGetProperty("items", out var iArr)
                && iArr.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in iArr.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object
                        && item.TryGetProperty("productName", out var p)
                        && p.ValueKind == JsonValueKind.String)
                        products.Add(p.GetString()!);
                    else if (item.ValueKind == JsonValueKind.String)
                        products.Add(item.GetString()!);
                }
            }
            // "products": ["Canon R5", ...]
            else if (root.TryGetProperty("products", out var pArr)
                && pArr.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in pArr.EnumerateArray())
                    if (item.ValueKind == JsonValueKind.String)
                        products.Add(item.GetString()!);
            }

            return (supplier, products);
        }
        catch { return (null, new()); }
    }
}

internal class TransactionLogRaw
{
    public int       Id          { get; set; }
    public string?   LogType     { get; set; }
    public DateTime? CreatedAt   { get; set; }
    public string?   DetailsJson { get; set; }
}