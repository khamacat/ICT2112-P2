-- ============================================================
-- LOAN SEED DATA (loan_seed.sql)
-- ============================================================

-- Insert LoanList records (IDs 2-5, as ID 1 was used in base seed)
INSERT INTO LoanList (LoanListId, OrderId, CustomerId, LoanDate, DueDate, ReturnDate, Status, Remarks)
OVERRIDING SYSTEM VALUE
VALUES 
-- Past loans that have been returned
(2, 2, 2, '2026-02-26 10:00:00+00', '2026-03-01 10:00:00+00', '2026-03-01 14:30:00+00', 'RETURNED', 'Returned on time.'),
(3, 3, 3, '2026-03-03 09:00:00+00', '2026-03-08 09:00:00+00', '2026-03-09 11:15:00+00', 'RETURNED', 'Returned 1 day late.'),

-- Active loan currently out with customer
(4, 4, 4, '2026-03-05 14:00:00+00', '2026-03-08 14:00:00+00', NULL, 'ON_LOAN', 'Customer requested extension.'),

-- Open loan waiting for dispatch pickup
(5, 6, 6, '2026-03-11 10:00:00+00', '2026-03-18 10:00:00+00', NULL, 'OPEN', 'Awaiting final QC before dispatch.');

-- Insert corresponding LoanItem records mapping to specific Inventory IDs
INSERT INTO LoanItem (LoanItemId, LoanListId, InventoryItemId, Remarks)
OVERRIDING SYSTEM VALUE
VALUES 
-- Items for LoanList 2 (Order 2: A7IV & 24-70mm)
(1, 2, 6,  'Clean dispatch'), -- Inventory 6: A7IV-0001
(2, 2, 10, 'Clean dispatch'), -- Inventory 10: 2470GM-0001

-- Items for LoanList 3 (Order 3: 24-70mm)
(3, 3, 11, 'Minor scratch on lens cap prior to loan'), -- Inventory 11: 2470GM-0002

-- Items for LoanList 4 (Order 4: 70-200mm & DJI RS3)
(4, 4, 16, 'Clean dispatch'), -- Inventory 16: 70200RF-0001
(5, 4, 27, 'Clean dispatch'), -- Inventory 27: RS3-0001

-- Items for LoanList 5 (Order 6: R5, 24-70mm, Tripod)
(6, 5, 2,  'Clean dispatch'), -- Inventory 2: R5-0002
(7, 5, 12, 'Clean dispatch'), -- Inventory 12: 2470GM-0003
(8, 5, 19, 'Clean dispatch'); -- Inventory 19: TRI-0001