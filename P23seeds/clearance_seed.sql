-- ============================================================
-- CLEARANCE SEED DATA (clearance_seed.sql)
-- ============================================================

-- 1. Create aged InventoryItems (Created 2.5 years ago, last updated 2.1 years ago)
INSERT INTO InventoryItem (InventoryId, ProductId, SerialNumber, Status, CreatedAt, UpdatedAt)
OVERRIDING SYSTEM VALUE
VALUES 
(101, 5, 'TRI-OLD-001', 'CLEARANCE', '2023-09-01 10:00:00+00', '2024-02-01 10:00:00+00'),
(102, 5, 'TRI-OLD-002', 'CLEARANCE', '2023-09-01 10:00:00+00', '2024-02-01 10:00:00+00'),
(103, 7, 'RVP-OLD-001', 'SOLD',      '2023-09-01 10:00:00+00', '2024-02-01 10:00:00+00');

-- 2. Create an Active Clearance Batch
INSERT INTO ClearanceBatch (ClearanceBatchId, BatchName, CreatedDate, ClearanceDate, Status)
OVERRIDING SYSTEM VALUE
VALUES 
(2, 'Spring 2026 Aged Inventory Blowout', '2026-03-15 09:00:00+00', NULL, 'ACTIVE');

-- 3. Link the aged inventory to the clearance batch
INSERT INTO ClearanceItem (ClearanceItemId, ClearanceBatchId, InventoryItemId, FinalPrice, RecommendedPrice, SaleDate, Status)
OVERRIDING SYSTEM VALUE
VALUES 
-- Items still awaiting sale in the clearance batch
(1, 2, 101, NULL,  15.00, NULL, 'CLEARANCE'),
(2, 2, 102, NULL,  15.00, NULL, 'CLEARANCE'),

-- Item that successfully sold via the clearance workflow
(3, 2, 103, 12.00, 15.00, '2026-03-18 14:45:00+00', 'SOLD');