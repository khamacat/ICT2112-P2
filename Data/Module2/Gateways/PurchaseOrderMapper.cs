using System.Reflection;
using Npgsql;
using ProRental.Data.Interfaces;
using ProRental.Domain.Entities;

namespace ProRental.Data.Gateways
{
    public class PurchaseOrderMapper : IPurchaseOrderMapper
    {
        private readonly string _connectionString;

        public PurchaseOrderMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Default connection string not found.");
        }

        public int Insert(Purchaseorder po)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            const string sql = @"
                INSERT INTO purchaseorder (supplierid, podate, expecteddeliverydate, totalamount)
                VALUES (@supplierid, @podate, @expecteddeliverydate, @totalamount)
                RETURNING poid;
            ";

            using var cmd = new NpgsqlCommand(sql, conn);

            var supplierId = GetPrivateFieldValue<int?>(po, "_supplierid");
            var poDate = GetPrivateFieldValue<DateOnly?>(po, "_podate");
            var expectedDeliveryDate = GetPrivateFieldValue<DateOnly?>(po, "_expecteddeliverydate");
            var totalAmount = GetPrivateFieldValue<decimal?>(po, "_totalamount");

            cmd.Parameters.AddWithValue("@supplierid", (object?)supplierId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@podate",
                poDate.HasValue ? poDate.Value.ToDateTime(TimeOnly.MinValue) : DBNull.Value);
            cmd.Parameters.AddWithValue("@expecteddeliverydate",
                expectedDeliveryDate.HasValue ? expectedDeliveryDate.Value.ToDateTime(TimeOnly.MinValue) : DBNull.Value);
            cmd.Parameters.AddWithValue("@totalamount", (object?)totalAmount ?? DBNull.Value);

            var newPoId = Convert.ToInt32(cmd.ExecuteScalar());
            SetPrivateField(po, "_poid", newPoId);

            return newPoId;
        }

        public Purchaseorder? FindById(int poId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            const string sql = @"
                SELECT poid, supplierid, podate, expecteddeliverydate, totalamount
                FROM purchaseorder
                WHERE poid = @poid;
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@poid", poId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapPurchaseOrder(reader);
        }

        public Purchaseorder? FindByRequestId(int reqId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            const string sql = @"
                SELECT po.poid, po.supplierid, po.podate, po.expecteddeliverydate, po.totalamount
                FROM purchaseorder po
                INNER JOIN polineitem pli ON po.poid = pli.poid
                INNER JOIN lineitem li ON pli.lineitemid = li.lineitemid
                WHERE li.requestid = @reqid
                ORDER BY po.poid DESC
                LIMIT 1;
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@reqid", reqId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapPurchaseOrder(reader);
        }

        public void UpdateExpectedDeliveryDate(int poId, DateOnly expectedDeliveryDate)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            const string sql = @"
                UPDATE purchaseorder
                SET expecteddeliverydate = @expecteddeliverydate
                WHERE poid = @poid;
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@expecteddeliverydate", expectedDeliveryDate.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@poid", poId);

            cmd.ExecuteNonQuery();
        }

        private static Purchaseorder MapPurchaseOrder(NpgsqlDataReader reader)
        {
            var po = new Purchaseorder();

            SetPrivateField(po, "_poid", reader.GetInt32(reader.GetOrdinal("poid")));
            SetPrivateField(po, "_supplierid", reader["supplierid"] == DBNull.Value ? null : Convert.ToInt32(reader["supplierid"]));
            SetPrivateField(po, "_podate", reader["podate"] == DBNull.Value ? null : DateOnly.FromDateTime(Convert.ToDateTime(reader["podate"])));
            SetPrivateField(po, "_expecteddeliverydate", reader["expecteddeliverydate"] == DBNull.Value ? null : DateOnly.FromDateTime(Convert.ToDateTime(reader["expecteddeliverydate"])));
            SetPrivateField(po, "_totalamount", reader["totalamount"] == DBNull.Value ? null : Convert.ToDecimal(reader["totalamount"]));

            return po;
        }

        private static void SetPrivateField(object target, string fieldName, object? value)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
                throw new InvalidOperationException(
                    $"Field '{fieldName}' was not found on type '{target.GetType().Name}'.");

            field.SetValue(target, value);
        }

        private static T? GetPrivateFieldValue<T>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
                throw new InvalidOperationException(
                    $"Field '{fieldName}' was not found on type '{target.GetType().Name}'.");

            return (T?)field.GetValue(target);
        }
        
    }
}