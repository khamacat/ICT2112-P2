using Microsoft.EntityFrameworkCore;
using ProRental.Data.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Controllers;
using System.Data;
using System.Text.Json;

namespace ProRental.Data.Gateways
{
    public class PurchaseOrderMapper : IPurchaseOrderMapper
    {
        private readonly AppDbContext _context;

        public PurchaseOrderMapper(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int Insert(Purchaseorder po)
        {
            if (po == null)
            {
                throw new ArgumentNullException(nameof(po));
            }

            _context.Database.OpenConnection();
            try
            {
                var connection = _context.Database.GetDbConnection();
                using var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO purchaseorder (supplierid, podate, status, expecteddeliverydate, totalamount)
                VALUES (@supplierid, CURRENT_DATE, 'CONFIRMED'::po_status_enum, @expecteddeliverydate, @totalamount)
                RETURNING poid;";

                AddParameter(command, "@supplierid", _context.Entry(po).Property<int?>("Supplierid").CurrentValue);
                AddParameter(command, "@expecteddeliverydate", _context.Entry(po).Property<DateOnly?>("Expecteddeliverydate").CurrentValue);
                AddParameter(command, "@totalamount", _context.Entry(po).Property<decimal?>("Totalamount").CurrentValue ?? 0m);

                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("Unable to create purchase order.");
                }

                return Convert.ToInt32(result);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public Purchaseorder? FindById(int poId)
        {
            return _context.Purchaseorders
                .Include(po => po.Polineitems)
                .FirstOrDefault(po => EF.Property<int>(po, "Poid") == poId);
        }

        public Purchaseorder? FindByRequestId(int reqId)
        {
            var poId = _context.Purchaseorderlogs
                .OrderByDescending(log => EF.Property<DateTime?>(log, "Podate"))
                .Select(log => new
                {
                    Poid = EF.Property<int?>(log, "Poid"),
                    DetailsJson = EF.Property<string?>(log, "Detailsjson")
                })
                .AsEnumerable()
                .FirstOrDefault(log => log.Poid.HasValue && TryExtractReqId(log.DetailsJson, out var id) && id == reqId)
                ?.Poid;

            return poId.HasValue ? FindById(poId.Value) : null;
        }

        public void UpdateExpectedDeliveryDate(int poId, DateOnly expectedDeliveryDate)
        {
            var po = FindById(poId) ?? throw new InvalidOperationException($"Purchase order #{poId} was not found.");
            _context.Entry(po).Property("Expecteddeliverydate").CurrentValue = expectedDeliveryDate;
            _context.SaveChanges();
        }

        public List<PurchaseOrderRequestListItemViewModel> GetAllRequests()
        {
            return _context.Replenishmentrequests
                .OrderByDescending(r => EF.Property<DateTime?>(r, "Createdat"))
                .ThenByDescending(r => EF.Property<int>(r, "Requestid"))
                .Select(r => new PurchaseOrderRequestListItemViewModel
                {
                    RequestId = EF.Property<int>(r, "Requestid"),
                    RequestedBy = EF.Property<string?>(r, "Requestedby") ?? "",
                    CreatedAt = EF.Property<DateTime?>(r, "Createdat"),
                    Status = r.Status.ToString(),
                    Remarks = EF.Property<string?>(r, "Remarks") ?? ""
                })
                .ToList();
        }

        public List<PurchaseOrderListItemViewModel> GetAllPurchaseOrders()
        {
            return _context.Purchaseorders
                .OrderByDescending(po => EF.Property<int>(po, "Poid"))
                .Select(po => new PurchaseOrderListItemViewModel
                {
                    PoId = EF.Property<int>(po, "Poid"),
                    SupplierId = EF.Property<int?>(po, "Supplierid") ?? 0,
                    PoDate = EF.Property<DateOnly?>(po, "Podate").HasValue
                        ? EF.Property<DateOnly?>(po, "Podate")!.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    ExpectedDeliveryDate = EF.Property<DateOnly?>(po, "Expecteddeliverydate").HasValue
                        ? EF.Property<DateOnly?>(po, "Expecteddeliverydate")!.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    Status = (EF.Property<POStatus>(po, "status")).ToString(),
                    TotalAmount = EF.Property<decimal?>(po, "Totalamount")
                })
                .ToList();
        }

        public void UpdatePurchaseOrderTotalAmount(int poId, decimal totalAmount)
        {
            var po = FindById(poId) ?? throw new InvalidOperationException($"Purchase order #{poId} was not found.");
            _context.Entry(po).Property("Totalamount").CurrentValue = totalAmount;
            _context.SaveChanges();
        }

        public void UpdateReplenishmentRequestStatusToSubmitted(int reqId)
        {
            var request = _context.Replenishmentrequests
                .FirstOrDefault(r => EF.Property<int>(r, "Requestid") == reqId)
                ?? throw new InvalidOperationException($"Replenishment request #{reqId} was not found.");

            request.Status = ReplenishmentStatus.SUBMITTED;
            _context.SaveChanges();
        }

        public int InsertTransactionLogForPurchaseOrder()
        {
            _context.Database.OpenConnection();
            try
            {
                var connection = _context.Database.GetDbConnection();
                using var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO transactionlog (logtype)
                VALUES ('PURCHASE_ORDER'::log_type_enum)
                RETURNING transactionlogid;";

                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("Unable to create transaction log for purchase order.");
                }

                return Convert.ToInt32(result);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public void InsertPurchaseOrderLog(
            int logId,
            int poId,
            int supplierId,
            DateOnly? expectedDeliveryDate,
            decimal totalAmount,
            string detailsJson)
        {
            if (string.IsNullOrWhiteSpace(detailsJson))
            {
                throw new ArgumentException("detailsJson is required.", nameof(detailsJson));
            }

            _context.Database.OpenConnection();
            try
            {
                var connection = _context.Database.GetDbConnection();
                using var command = connection.CreateCommand();
                command.CommandText = @"
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
                );";

                AddParameter(command, "@logId", logId);
                AddParameter(command, "@poId", poId);
                AddParameter(command, "@supplierId", supplierId);
                AddParameter(command, "@totalAmount", totalAmount);
                AddParameter(
                    command,
                    "@expectedDeliveryDate",
                    expectedDeliveryDate.HasValue
                        ? expectedDeliveryDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null);
                AddParameter(command, "@detailsJson", detailsJson);

                var affected = command.ExecuteNonQuery();
                if (affected == 0)
                {
                    throw new InvalidOperationException($"Unable to insert purchase order log for PO #{poId}.");
                }
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public void ApprovePurchaseOrder(int poId)
        {
            _context.Database.ExecuteSqlInterpolated($@"
                UPDATE purchaseorder
                SET status = 'APPROVED'::po_status_enum
                WHERE poid = {poId}
                AND status IN ('CONFIRMED'::po_status_enum, 'SUBMITTED'::po_status_enum);");
        }

        public void CompletePurchaseOrder(int poId)
        {
            _context.Database.ExecuteSqlInterpolated($@"
                UPDATE purchaseorder
                SET status = 'COMPLETED'::po_status_enum
                WHERE poid = {poId}
                AND status = 'APPROVED'::po_status_enum;");
        }

        public void CancelPurchaseOrder(int poId)
        {
            _context.Database.ExecuteSqlInterpolated($@"
                UPDATE purchaseorder
                SET status = 'CANCELLED'::po_status_enum
                WHERE poid = {poId}
                  AND status IN ('CONFIRMED'::po_status_enum, 'APPROVED'::po_status_enum);");
        }

        public int? FindLinkedRequestIdByPoId(int poId)
        {
            var detailsJson = _context.Purchaseorderlogs
                .Where(log => EF.Property<int?>(log, "Poid") == poId)
                .OrderByDescending(log => EF.Property<DateTime?>(log, "Podate"))
                .Select(log => EF.Property<string?>(log, "Detailsjson"))
                .FirstOrDefault();

            return TryExtractReqId(detailsJson, out var reqId) ? reqId : null;
        }

        public void CompleteReplenishmentRequest(int reqId, string completedBy)
        {
            if (string.IsNullOrWhiteSpace(completedBy))
            {
                throw new ArgumentException("completedBy is required.", nameof(completedBy));
            }

            var request = _context.Replenishmentrequests
                .FirstOrDefault(r => EF.Property<int>(r, "Requestid") == reqId);

            if (request == null || request.Status != ReplenishmentStatus.SUBMITTED)
            {
                throw new InvalidOperationException($"No SUBMITTED replenishment request found for request #{reqId}.");
            }

            request.Status = ReplenishmentStatus.COMPLETED;
            _context.Entry(request).Property("Completedat").CurrentValue = DateTime.UtcNow;
            _context.Entry(request).Property("Completedby").CurrentValue = completedBy;
            _context.SaveChanges();
        }

        public void CancelDraftReplenishmentRequest(int reqId)
        {
            var request = _context.Replenishmentrequests
                .FirstOrDefault(r => EF.Property<int>(r, "Requestid") == reqId);

            if (request == null || request.Status != ReplenishmentStatus.DRAFT)
            {
                return;
            }

            request.Status = ReplenishmentStatus.CANCELLED;
            _context.SaveChanges();
        }

        private static bool TryExtractReqId(string? detailsJson, out int reqId)
        {
            reqId = default;
            if (string.IsNullOrWhiteSpace(detailsJson))
            {
                return false;
            }

            try
            {
                using var document = JsonDocument.Parse(detailsJson);
                if (document.RootElement.TryGetProperty("reqId", out var reqIdElement) &&
                    reqIdElement.ValueKind == JsonValueKind.Number &&
                    reqIdElement.TryGetInt32(out reqId))
                {
                    return true;
                }
            }
            catch (JsonException)
            {
                return false;
            }

            return false;
        }

        private static void AddParameter(IDbCommand command, string name, object? value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

    }
}
