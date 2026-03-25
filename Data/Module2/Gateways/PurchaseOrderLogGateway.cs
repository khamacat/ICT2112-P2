namespace ProRental.Data.Module2.Gateways;

using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Data.Module2.Interfaces;
using ProRental.Data.UnitOfWork;

public class PurchaseOrderLogGateway : IPurchaseOrderLogGateway
{
    private readonly AppDbContext context;

    public PurchaseOrderLogGateway(AppDbContext context)
    {
        this.context = context;
    }

public Purchaseorderlog Insert(Purchaseorderlog log)
{
    var conn = (Npgsql.NpgsqlConnection)context.Database.GetDbConnection();
    if (conn.State != System.Data.ConnectionState.Open)
        conn.Open();

    using var cmd = new Npgsql.NpgsqlCommand(
        @"INSERT INTO purchaseorderlog 
            (purchaseorderlogid, poid, podate, supplierid, expecteddeliverydate, totalamount, detailsjson)
          OVERRIDING SYSTEM VALUE
          VALUES 
            (@id, @poid, @podate, @supplierid, @expecteddeliverydate, @totalamount, @detailsjson)", conn);

    cmd.Parameters.AddWithValue("id", log.purchaseorder_logid);
    cmd.Parameters.AddWithValue("poid", log.po_id);
    cmd.Parameters.AddWithValue("podate", (object?)log.po_date ?? DBNull.Value);
    cmd.Parameters.AddWithValue("supplierid", (object?)log.supplier_id ?? DBNull.Value);
    cmd.Parameters.AddWithValue("expecteddeliverydate", (object?)log.expected_deliverydate ?? DBNull.Value);
    cmd.Parameters.AddWithValue("totalamount", (object?)log.total_amount ?? DBNull.Value);
    cmd.Parameters.AddWithValue("detailsjson", (object?)log.details_json ?? DBNull.Value);

    cmd.ExecuteNonQuery();
    return log;
}

    public List<Purchaseorderlog> GetAll()
    {
        return context.Purchaseorderlogs
            .Include(p => p.PurchaseorderlogNavigation)
            .OrderByDescending(p => EF.Property<DateTime?>(p.PurchaseorderlogNavigation, "Createdat"))
            .ToList();
    }

    public Purchaseorderlog? GetById(int purchaseOrderLogId)
    {
        return context.Purchaseorderlogs
            .Include(p => p.PurchaseorderlogNavigation)
            .FirstOrDefault(p => EF.Property<int>(p, "Purchaseorderlogid") == purchaseOrderLogId);
    }

    public bool ExistsByPoId(int poId)
    {
        return context.Purchaseorderlogs.Any(p => EF.Property<int>(p, "Poid") == poId);
    }
}