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

                // 1. Create PO first with temporary total 0
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

                // 2. Insert PO line items using ProductDetails.Price
                using (var cmd = new NpgsqlCommand(@"
            INSERT INTO polineitem (poid, productid, qty, unitprice, linetotal)
            SELECT
                @poId,
                li.productid,
                li.quantityrequest,
                COALESCE(pd.price, 0),
                li.quantityrequest * COALESCE(pd.price, 0)
            FROM lineitem li
            LEFT JOIN productdetails pd
                ON pd.productid = li.productid
            WHERE li.requestid = @reqId;", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@poId", poId);
                    cmd.Parameters.AddWithValue("@reqId", reqId);
                    cmd.ExecuteNonQuery();
                }

                // 3. Sum all PO line totals
                decimal totalAmount = 0;
                using (var cmd = new NpgsqlCommand(@"
            SELECT COALESCE(SUM(linetotal), 0)
            FROM polineitem
            WHERE poid = @poId;", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@poId", poId);
                    totalAmount = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // 4. Update PO total amount
                using (var cmd = new NpgsqlCommand(@"
            UPDATE purchaseorder
            SET totalamount = @totalAmount
            WHERE poid = @poId;", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@poId", poId);
                    cmd.ExecuteNonQuery();
                }
                // 5. Update replenishment request status → SUBMITTED
                using (var cmd = new NpgsqlCommand(@"
    UPDATE replenishmentrequest
    SET status = 'SUBMITTED'::replenishment_status_enum
    WHERE requestid = @reqId;", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@reqId", reqId);
                    cmd.ExecuteNonQuery();
                }
                // 6. INSERT into TransactionLog (parent)
int transactionLogId;

using (var cmd = new NpgsqlCommand(@"
    INSERT INTO transactionlog (logtype)
    VALUES ('PURCHASE_ORDER'::log_type_enum)
    RETURNING transactionlogid;", conn, tx))
{
    transactionLogId = Convert.ToInt32(cmd.ExecuteScalar());
}

// 7. INSERT into PurchaseOrderLog (child)
using (var cmd = new NpgsqlCommand(@"
    INSERT INTO purchaseorderlog (
        purchaseorderlogid,
        poid,
        podate,
        supplierid,
        status,
        expecteddeliverydate,
        totalamount,
        detailsjson
    )
    OVERRIDING SYSTEM VALUE
    VALUES (
        @logId,
        @poId,
        CURRENT_TIMESTAMP,
        @supplierId,
        'CONFIRMED'::rental_status_enum,
        @expectedDeliveryDate,
        @totalAmount,
        @detailsJson
    );", conn, tx))
{
    cmd.Parameters.AddWithValue("@logId", transactionLogId);
    cmd.Parameters.AddWithValue("@poId", poId);
    cmd.Parameters.AddWithValue("@supplierId", supplierId);
    cmd.Parameters.AddWithValue("@totalAmount", totalAmount);

    cmd.Parameters.AddWithValue(
        "@expectedDeliveryDate",
        expectedDeliveryDate.HasValue
            ? expectedDeliveryDate.Value.ToDateTime(TimeOnly.MinValue)
            : DBNull.Value
    );

    string detailsJson = $@"{{
        ""poId"": {poId},
        ""supplierId"": {supplierId},
        ""totalAmount"": {totalAmount},
        ""status"": ""CONFIRMED""
    }}";

    cmd.Parameters.AddWithValue("@detailsJson", detailsJson);

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
        public void ApprovePurchaseOrder(int poId)
        {
            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var cmd = new NpgsqlCommand(@"
        UPDATE purchaseorder
        SET status = 'APPROVED'::po_status_enum
        WHERE poid = @poId
        AND status = 'CONFIRMED'::po_status_enum;", conn);

            cmd.Parameters.AddWithValue("@poId", poId);
            cmd.ExecuteNonQuery();
        }

        public void CompletePurchaseOrder(int poId)
        {
            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                // 1. Update PO → COMPLETED
                using (var cmd = new NpgsqlCommand(@"
            UPDATE purchaseorder
            SET status = 'COMPLETED'::po_status_enum
            WHERE poid = @poId
            AND status = 'APPROVED'::po_status_enum;", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@poId", poId);
                    cmd.ExecuteNonQuery();
                }

                // 2. ALSO mark related replenishment request as COMPLETED
                using (var cmd = new NpgsqlCommand(@"
            UPDATE replenishmentrequest
            SET status = 'COMPLETED'::replenishment_status_enum,
                completedat = CURRENT_TIMESTAMP,
                completedby = 'system'
            WHERE requestid IN (
                SELECT li.requestid
                FROM polineitem pli
                JOIN lineitem li ON li.productid = pli.productid
                WHERE pli.poid = @poId
            );", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@poId", poId);
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
        
        public void CancelPurchaseOrder(int poId)
        {
            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var cmd = new NpgsqlCommand(@"
        UPDATE purchaseorder
        SET status = 'CANCELLED'::po_status_enum
        WHERE poid = @poId
          AND status IN ('CONFIRMED'::po_status_enum, 'APPROVED'::po_status_enum);", conn);

            cmd.Parameters.AddWithValue("@poId", poId);
            cmd.ExecuteNonQuery();
        }

        public void CancelReplenishmentRequest(int reqId)
        {
            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var cmd = new NpgsqlCommand(@"
        UPDATE replenishmentrequest
        SET status = 'CANCELLED'::replenishment_status_enum
        WHERE requestid = @reqId
          AND status = 'DRAFT'::replenishment_status_enum;", conn);

            cmd.Parameters.AddWithValue("@reqId", reqId);
            cmd.ExecuteNonQuery();
        }
    }
}