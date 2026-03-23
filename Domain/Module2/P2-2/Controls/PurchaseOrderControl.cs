using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProRental.Controllers;
using ProRental.Data.UnitOfWork;
using ProRental.Interfaces;
using System.Data;

namespace ProRental.Domain.Control
{
    public class PurchaseOrderControl : IPurchaseOrderService
    {
        private readonly AppDbContext _context;

        public PurchaseOrderControl(AppDbContext context)
        {
            _context = context;
        }

        public PurchaseOrderPageViewModel GetPurchaseOrderPageData(int reqId)
        {
            var vm = new PurchaseOrderPageViewModel
            {
                RequestId = reqId
            };

            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using (var cmd = new NpgsqlCommand(@"
                SELECT requestid, requestedby, createdat, remarks, status::text
                FROM replenishmentrequest
                WHERE requestid = @reqId;", conn))
            {
                cmd.Parameters.AddWithValue("@reqId", reqId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    vm.RequestId = reader.GetInt32(0);
                    vm.RequestedBy = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    vm.CreatedAt = reader.IsDBNull(2) ? null : reader.GetDateTime(2);
                    vm.Remarks = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    vm.Status = reader.IsDBNull(4) ? "" : reader.GetString(4);
                }
            }

            using (var cmd = new NpgsqlCommand(@"
                SELECT 
                    li.lineitemid,
                    li.productid,
                    COALESCE(pd.name, 'Unknown Product') AS productname,
                    li.quantityrequest,
                    li.remarks
                FROM lineitem li
                LEFT JOIN product p ON p.productid = li.productid
                LEFT JOIN productdetails pd ON pd.productid = p.productid
                WHERE li.requestid = @reqId
                ORDER BY li.lineitemid;", conn))
            {
                cmd.Parameters.AddWithValue("@reqId", reqId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vm.Items.Add(new PurchaseOrderItemViewModel
                    {
                        LineItemId = reader.GetInt32(0),
                        ProductId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                        ProductName = reader.IsDBNull(2) ? "Unknown Product" : reader.GetString(2),
                        Qty = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Remarks = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    });
                }
            }

            using (var cmd = new NpgsqlCommand(@"
                SELECT supplierid, name, details, creditperiod, avgturnaroundtime, isverified
                FROM supplier
                WHERE isverified = true
                ORDER BY name;", conn))
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vm.Suppliers.Add(new PurchaseOrderSupplierViewModel
                    {
                        SupplierId = reader.GetInt32(0),
                        SupplierName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        Details = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        CreditPeriod = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        AvgTurnaroundTime = reader.IsDBNull(4) ? null : reader.GetDouble(4),
                        IsVerified = !reader.IsDBNull(5) && reader.GetBoolean(5)
                    });
                }
            }

            return vm;
        }

        public List<PurchaseOrderRequestListItemViewModel> GetAllRequests()
        {
            var requests = new List<PurchaseOrderRequestListItemViewModel>();

            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var cmd = new NpgsqlCommand(@"
                SELECT requestid, requestedby, createdat, status::text, remarks
                FROM replenishmentrequest
                ORDER BY createdat DESC, requestid DESC;", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                requests.Add(new PurchaseOrderRequestListItemViewModel
                {
                    RequestId = reader.GetInt32(0),
                    RequestedBy = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    CreatedAt = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    Status = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Remarks = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }

            return requests;
        }

        public List<PurchaseOrderListItemViewModel> GetAllPurchaseOrders()
        {
            var purchaseOrders = new List<PurchaseOrderListItemViewModel>();

            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var cmd = new NpgsqlCommand(@"
                SELECT poid, supplierid, podate, expecteddeliverydate, status::text, totalamount
                FROM purchaseorder
                ORDER BY poid DESC;", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                purchaseOrders.Add(new PurchaseOrderListItemViewModel
                {
                    PoId = reader.GetInt32(0),
                    SupplierId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    PoDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    ExpectedDeliveryDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    Status = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    TotalAmount = reader.IsDBNull(5) ? null : reader.GetDecimal(5)
                });
            }

            return purchaseOrders;
        }

        public int ConfirmPurchaseOrder(int reqId, int supplierId, DateOnly? expectedDeliveryDate)
        {
            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                int poId;

                using (var cmd = new NpgsqlCommand(@"
                    INSERT INTO purchaseorder (supplierid, podate, status, expecteddeliverydate, totalamount)
                    VALUES (@supplierId, CURRENT_DATE, 'CONFIRMED'::po_status_enum, @expectedDeliveryDate, 0)
                    RETURNING poid;", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@supplierId", supplierId);
                    cmd.Parameters.AddWithValue(
                        "@expectedDeliveryDate",
                        expectedDeliveryDate.HasValue
                            ? expectedDeliveryDate.Value.ToDateTime(TimeOnly.MinValue)
                            : DBNull.Value);

                    poId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = new NpgsqlCommand(@"
                    INSERT INTO polineitem (poid, productid, qty, unitprice, linetotal)
                    SELECT @poId, productid, quantityrequest, 0, 0
                    FROM lineitem
                    WHERE requestid = @reqId;", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@poId", poId);
                    cmd.Parameters.AddWithValue("@reqId", reqId);
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
                return poId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}