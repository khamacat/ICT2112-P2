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
            var (supplier, product) = ExtractFromJson(r.DetailsJson);
            return new TransactionLogDto
            {
                LogID        = r.Id,
                LogType      = r.LogType ?? "UNKNOWN",
                CreatedAt    = r.CreatedAt ?? DateTime.UtcNow,
                SupplierName = supplier,
                ProductName  = product,
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
    private static (string? supplier, string? product) ExtractFromJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return (null, null);
        try
        {
            using var doc  = JsonDocument.Parse(json);
            var root       = doc.RootElement;
            string? supplier = null, product = null;

            if (root.TryGetProperty("suppliers", out var sArr)
                && sArr.ValueKind == JsonValueKind.Array && sArr.GetArrayLength() > 0)
                supplier = sArr[0].GetString();

            if (root.TryGetProperty("products", out var pArr)
                && pArr.ValueKind == JsonValueKind.Array && pArr.GetArrayLength() > 0)
                product = pArr[0].GetString();
            else if (root.TryGetProperty("items", out var iArr)
                && iArr.ValueKind == JsonValueKind.Array && iArr.GetArrayLength() > 0)
                product = iArr[0].GetString();

            return (supplier, product);
        }
        catch { return (null, null); }
    }
}

internal class TransactionLogRaw
{
    public int       Id          { get; set; }
    public string?   LogType     { get; set; }
    public DateTime? CreatedAt   { get; set; }
    public string?   DetailsJson { get; set; }
}