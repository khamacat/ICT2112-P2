-- ============================================================
-- RETURN SEED DATA (return_seed.sql)
-- ============================================================

-- Insert ReturnRequest records for the returned loans
INSERT INTO ReturnRequest (ReturnRequestId, OrderId, CustomerId, Status, RequestDate, CompletionDate)
OVERRIDING SYSTEM VALUE
VALUES 
-- Completed return for Order 2
(2, 2, 2, 'COMPLETED',  '2026-03-01 14:30:00+00', '2026-03-02 09:00:00+00'),
-- In-progress return for Order 3 (Waiting on repairs)
(3, 3, 3, 'PROCESSING', '2026-03-09 11:15:00+00', NULL);

-- Insert ReturnItems in various stages of the return pipeline
INSERT INTO ReturnItem (ReturnItemId, ReturnRequestId, InventoryItemId, Status, CompletionDate, Image)
OVERRIDING SYSTEM VALUE
VALUES 
-- Items from Order 2 successfully processed and returned to inventory
(1, 2, 6,  'RETURN_TO_INVENTORY', '2026-03-02 09:00:00+00', 'return_6_clean.jpg'),
(2, 2, 10, 'RETURN_TO_INVENTORY', '2026-03-02 09:00:00+00', 'return_10_clean.jpg'),

-- Item from Order 3 undergoing damage processing
(3, 3, 11, 'REPAIRING', NULL, 'return_11_damaged.jpg');

-- Insert a DamageReport for the item currently being repaired
INSERT INTO DamageReport (DamageReportId, ReturnItemId, Description, Severity, RepairCost, Images, ReportDate)
OVERRIDING SYSTEM VALUE
VALUES 
(1, 3, 'Deep scratch on the front glass element. Requires complete element replacement.', 'HIGH', 150.00, 'damage_11_glass.jpg', '2026-03-09 13:00:00+00');