-- ============================================================
-- PRO RENTALS: MODULE 2 ULTIMATE DEMO SEED
-- Fixed for Strict ENUMs, initial_schema.sql alignment, and FK order
-- ============================================================

-- 1. Clear tables and completely reset all Identity sequences to 1
TRUNCATE 
    Category, "User", TransactionLog, Supplier, PurchaseOrder, 
    "transaction", "Order", Checkout, Cart, CartItem,
    Session, Payment, Deposit, replenishmentrequest, LoanList, ReturnRequest, ClearanceBatch,
    Analytics, AnalyticsList
RESTART IDENTITY CASCADE;

-- ============================================================
-- 2. USERS, ROLES & SUPPLIERS
-- ============================================================
INSERT INTO "User" (name, userRole, email, passwordHash, phoneCountry, phoneNumber) VALUES
  ('Alice Tan',        'CUSTOMER', 'alice.tan@example.com',        '$2b$12$hashAlice', 65, '90000001'),
  ('Benjamin Lee',     'CUSTOMER', 'ben.lee@example.com',          '$2b$12$hashBen',   65, '90000002'),
  ('Charlotte Ng',     'CUSTOMER', 'charlotte.ng@example.com',     '$2b$12$hashChar',  65, '90000003'),
  ('Daniel Wong',      'CUSTOMER', 'daniel.wong@example.com',      '$2b$12$hashDan',   65, '90000004'),
  ('Lydia Chua',       'STAFF',    'lydia.chua@example.com',       '$2b$12$hashLyd',   65, '90000012'),
  ('Operations Admin', 'ADMIN',    'ops.admin@company.com',        '$2b$12$hashOps',   65, '90000015');

INSERT INTO Customer (userId, address, customerType) VALUES
  (1, '123 Orchard Rd, Singapore 238123', 1), (2, '456 Marina Bay, Singapore 018972', 1),
  (3, '10 Tampines Ave, Singapore 529000', 2), (4, '5 Jurong East St, Singapore 609000', 2);

INSERT INTO Staff (userId, department) VALUES
  (5, 'Customer Support'), (6, 'IT');

-- Fixed: Explicitly provided SupplierID = 1 because the schema does not auto-increment it
INSERT INTO Supplier (SupplierID, Name, Details, CreditPeriod, AvgTurnaroundTime, SupplierCategory, IsVerified, VettingResult) VALUES 
  (1, 'Camera Supply Co', 'Main camera supplier', 30, 4.5, 'QUICKTURNAROUNDTIME', true, 'APPROVED');

-- ============================================================
-- 3. PRODUCT CATALOG
-- ============================================================
INSERT INTO Category (Name, Description) VALUES
('Camera', 'Digital and mirrorless cameras'), 
('Tripod', 'Camera tripods and stands'), 
('Gimbal', 'Camera stabilizers'),
('Microphone', 'Audio recording equipment');

INSERT INTO Product (CategoryId, Sku, Threshold) VALUES
(1, 'CAM-CANON-R5', 0.10),        -- 1 (Used for Historical Logs & ProdTrend)
(2, 'TRI-MANFROTTO-BEF', 0.10),   -- 2 (Clearance Demo Target)
(3, 'GIM-DJI-RS3', 0.10),         -- 3 (Used for Order 3)
(4, 'MIC-RODE-VIDEOMIC', 0.10),   -- 4 (Used for Historical Clearance Log)
(1, 'CAM-SONY-FX3-DEMO', 0.40);   -- 5 (Alert Demo Target - 40% threshold)

INSERT INTO ProductDetails (ProductId, TotalQuantity, Name, Description, Weight, Image, Price, DepositRate) VALUES
(1, 6, 'Canon EOS R5', '45MP full-frame mirrorless camera', 0.74, 'canon_r5.jpg', 150.00, 0.30),
(2, 5, 'Manfrotto Befree Tripod', 'Older travel tripod', 1.50, 'manfrotto_befree.jpg', 25.00, 0.10),
(3, 3, 'DJI RS 3 Gimbal Stabilizer', '3-axis camera stabilizer', 1.30, 'dji_rs3.jpg', 60.00, 0.20),
(4, 5, 'Rode VideoMic Pro+', 'Shotgun microphone for cameras', 0.12, 'rode_videomic.jpg', 20.00, 0.10),
(5, 5, 'Sony FX3 Cinema Line', 'High-end cinema camera for threshold alert demo', 0.71, 'sony_fx3.jpg', 200.00, 0.30);

-- ============================================================
-- 4. INVENTORY ITEMS
-- ============================================================
INSERT INTO InventoryItem (ProductId, SerialNumber, Status, CreatedAt, UpdatedAt) VALUES
-- Product 1 (Standard healthy stock for logs) IDs 1 to 6
(1, 'R5-0001', 'AVAILABLE', NOW(), NOW()), (1, 'R5-0002', 'AVAILABLE', NOW(), NOW()), 
(1, 'R5-0003', 'AVAILABLE', NOW(), NOW()), (1, 'R5-0004', 'AVAILABLE', NOW(), NOW()), 
(1, 'R5-0005', 'AVAILABLE', NOW(), NOW()), (1, 'R5-0006', 'AVAILABLE', NOW(), NOW()),

-- Product 2 (Clearance Targets) IDs 7 to 11
(2, 'TRI-OLD-001', 'AVAILABLE', NOW() - INTERVAL '3 years', NOW() - INTERVAL '2.5 years'),
(2, 'TRI-OLD-002', 'AVAILABLE', NOW() - INTERVAL '3 years', NOW() - INTERVAL '2.5 years'),
(2, 'TRI-OLD-003', 'AVAILABLE', NOW() - INTERVAL '3 years', NOW() - INTERVAL '2.5 years'),
(2, 'TRI-OLD-004', 'AVAILABLE', NOW() - INTERVAL '3 years', NOW() - INTERVAL '2.5 years'),
(2, 'TRI-OLD-005', 'AVAILABLE', NOW() - INTERVAL '3 years', NOW() - INTERVAL '2.5 years'),

-- Product 3 (For Order 3) IDs 12 to 14
(3, 'RS3-0001', 'AVAILABLE', NOW(), NOW()), (3, 'RS3-0002', 'AVAILABLE', NOW(), NOW()), 
(3, 'RS3-0003', 'AVAILABLE', NOW(), NOW()),

-- Product 4 (For Historical Clearance Log) ID 15
(4, 'RVP-OLD-001', 'SOLD', NOW() - INTERVAL '4 years', NOW() - INTERVAL '1 month'),

-- Product 5 (Alert Demo Targets) IDs 16 to 20
(5, 'FX3-0001', 'AVAILABLE', NOW(), NOW()), (5, 'FX3-0002', 'AVAILABLE', NOW(), NOW()),
(5, 'FX3-0003', 'AVAILABLE', NOW(), NOW()), (5, 'FX3-0004', 'AVAILABLE', NOW(), NOW()),
(5, 'FX3-0005', 'AVAILABLE', NOW(), NOW());

-- ============================================================
-- 5. "PRE" TABLES & HISTORICAL ORDERS
-- ============================================================
INSERT INTO Session (userId, role, createdAt, expiresAt) VALUES
(1, 'CUSTOMER', NOW(), NOW() + INTERVAL '1 day'),
(2, 'CUSTOMER', NOW(), NOW() + INTERVAL '1 day'),
(3, 'CUSTOMER', NOW(), NOW() + INTERVAL '1 day'),
(4, 'CUSTOMER', NOW(), NOW() + INTERVAL '1 day');

INSERT INTO Cart (customerId, sessionId, rentalStart, rentalEnd, status) VALUES 
(1, 1, '2026-03-05', '2026-03-12', 'CHECKED_OUT'),
(2, 2, '2026-03-06', '2026-03-13', 'CHECKED_OUT'),
(3, 3, '2026-03-07', '2026-03-14', 'CHECKED_OUT'),
(4, 4, NOW() + INTERVAL '1 day', NOW() + INTERVAL '5 days', 'CHECKED_OUT');

INSERT INTO Checkout (customerId, cartId, paymentMethodType, status, notifyOptIn, createdAt) VALUES 
(1, 1, 'CREDIT_CARD', 'CONFIRMED', TRUE, '2026-03-01'),
(2, 2, 'CREDIT_CARD', 'CONFIRMED', FALSE, '2026-03-02'),
(3, 3, 'CREDIT_CARD', 'CONFIRMED', TRUE, '2026-03-03'),
(4, 4, 'CREDIT_CARD', 'CONFIRMED', TRUE, NOW() - INTERVAL '1 hour');

INSERT INTO Transaction (amount, type, purpose, status, providerTransactionId, createdAt) VALUES 
(450.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'txn_1', '2026-03-01'),
(450.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'txn_2', '2026-03-02'),
(180.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'txn_3', '2026-03-03'),
(600.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'txn_4', NOW() - INTERVAL '1 hour');

INSERT INTO "Order" (customerId, checkoutId, transactionId, orderDate, status, deliveryType, totalAmount) VALUES
(1, 1, 1, '2026-03-01 10:00:00+00', 'CONFIRMED', 'NextDay', 450.00), 
(2, 2, 2, '2026-03-02 10:00:00+00', 'CONFIRMED', 'ThreeDays', 450.00), 
(3, 3, 3, '2026-03-03 10:00:00+00', 'CONFIRMED', 'ThreeDays', 180.00),
(4, 4, 4, NOW() - INTERVAL '1 hour',  'CONFIRMED', 'NextDay', 600.00); 

-- Fixed: Payment now properly comes AFTER Order, allowing FK to succeed
INSERT INTO Payment (paymentId, orderId, transactionId, amount, purpose, status, createdAt) VALUES 
('PAY-1', 1, 1, 450.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', '2026-03-01'),
('PAY-2', 2, 2, 450.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', '2026-03-02'),
('PAY-3', 3, 3, 180.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', '2026-03-03'),
('PAY-4', 4, 4, 600.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '1 hour');

INSERT INTO OrderItem (orderId, productId, quantity, unitPrice, rentalStartDate, rentalEndDate) VALUES
(1, 1, 3, 150.00, '2026-03-05', '2026-03-12'),
(2, 1, 3, 150.00, '2026-03-06', '2026-03-13'),
(3, 3, 3,  60.00, '2026-03-07', '2026-03-14'),
(4, 5, 4, 150.00, NOW() + INTERVAL '1 day', NOW() + INTERVAL '5 days');

-- ============================================================
-- 6. HISTORICAL DOMAIN DATA
-- ============================================================
INSERT INTO StockItem (productID, sku, name, uom) VALUES
(1, 'CAM-CANON-R5', 'Canon EOS R5', 'Unit'),
(2, 'TRI-MANFROTTO-BEF', 'Manfrotto Befree Tripod', 'Unit'),
(3, 'GIM-DJI-RS3', 'DJI RS 3 Gimbal Stabilizer', 'Unit'),
(4, 'MIC-RODE-VIDEOMIC', 'Rode VideoMic Pro+', 'Unit'),
(5, 'CAM-SONY-FX3-DEMO', 'Sony FX3 Cinema Line', 'Unit');

INSERT INTO LoanList (OrderId, CustomerId, LoanDate, DueDate, ReturnDate, Status, Remarks) VALUES 
(1, 1, '2026-03-05 10:00:00+00', '2026-03-12 10:00:00+00', '2026-03-11 14:00:00+00', 'RETURNED', 'Customer self-collected'),
(2, 2, '2026-03-06 10:00:00+00', '2026-03-13 10:00:00+00', '2026-03-13 09:00:00+00', 'RETURNED', 'Standard dispatch'),
(3, 3, '2026-03-07 10:00:00+00', '2026-03-14 10:00:00+00', NULL, 'ON_LOAN', 'Extended loan requested');

INSERT INTO LoanItem (LoanListId, InventoryItemId, Remarks) VALUES
(1, 1, NULL), (1, 2, NULL), (1, 3, NULL),
(2, 4, NULL), (2, 5, NULL), (2, 6, NULL),
(3, 12, NULL), (3, 13, NULL), (3, 14, NULL);

INSERT INTO ReturnRequest (OrderId, CustomerId, Status, RequestDate, CompletionDate) VALUES 
(1, 1, 'COMPLETED', '2026-03-11 14:00:00+00', '2026-03-11 15:00:00+00'),
(2, 2, 'PROCESSING', '2026-03-13 09:00:00+00', NULL);

INSERT INTO ReturnItem (ReturnRequestId, InventoryItemId, Status, CompletionDate, Image) VALUES 
(1, 1, 'RETURN_TO_INVENTORY', '2026-03-11 15:00:00+00', NULL),
(1, 2, 'RETURN_TO_INVENTORY', '2026-03-11 15:00:00+00', NULL),
(1, 3, 'RETURN_TO_INVENTORY', '2026-03-11 15:00:00+00', NULL),
(2, 4, 'CLEANING', NULL, NULL),
(2, 5, 'CLEANING', NULL, NULL),
(2, 6, 'CLEANING', NULL, NULL);

INSERT INTO PurchaseOrder (SupplierId, ExpectedDeliveryDate, PoDate, Status, TotalAmount) VALUES 
(1, '2026-02-22 10:00:00+00', '2026-02-15 10:00:00+00', 'COMPLETED', 5500.00);

INSERT INTO ClearanceBatch (BatchName, CreatedDate, ClearanceDate, Status) VALUES 
('Winter 2026 Blowout', '2026-02-15 10:00:00+00', '2026-02-20 10:00:00+00', 'CLOSED');

INSERT INTO ClearanceItem (ClearanceBatchId, InventoryItemId, FinalPrice, RecommendedPrice, SaleDate, Status) VALUES 
(1, 15, 5.00, 5.00, '2026-02-20 12:00:00+00', 'SOLD');

-- ============================================================
-- 7. TRANSACTION LOGS & UI JSON DATA
-- ============================================================
INSERT INTO TransactionLog (LogType, CreatedAt) VALUES
('RENTAL_ORDER', '2026-03-01 10:00:00+00'), -- 1
('RENTAL_ORDER', '2026-03-02 10:00:00+00'), -- 2
('RENTAL_ORDER', '2026-03-03 10:00:00+00'), -- 3
('RENTAL_ORDER', NOW()),                    -- 4 
('LOAN',         '2026-03-05 10:00:00+00'), -- 5
('LOAN',         '2026-03-06 10:00:00+00'), -- 6
('LOAN',         '2026-03-07 10:00:00+00'), -- 7
('RETURN',       '2026-03-11 14:00:00+00'), -- 8
('RETURN',       '2026-03-13 09:00:00+00'), -- 9
('PURCHASE_ORDER', '2026-02-15 10:00:00+00'), -- 10
('CLEARANCE',    '2026-02-20 10:00:00+00');   -- 11

INSERT INTO RentalOrderLog (RentalOrderLogId, OrderId, CustomerId, TotalAmount, OrderDate, DetailsJson) VALUES
(1, 1, 1, 450.00, '2026-03-01 10:00:00+00', '{"deliveryType":"EXPRESS","status":"COMPLETED","items":[{"productName":"Canon EOS R5","quantity":3,"unitPrice":150.00,"rentalStart":"2026-03-05","rentalEnd":"2026-03-12"}]}'),
(2, 2, 2, 450.00, '2026-03-02 10:00:00+00', '{"deliveryType":"STANDARD","status":"COMPLETED","items":[{"productName":"Canon EOS R5","quantity":3,"unitPrice":150.00,"rentalStart":"2026-03-06","rentalEnd":"2026-03-13"}]}'),
(3, 3, 3, 180.00, '2026-03-03 10:00:00+00', '{"deliveryType":"STANDARD","status":"CONFIRMED","items":[{"productName":"DJI RS 3 Gimbal Stabilizer","quantity":3,"unitPrice":60.00,"rentalStart":"2026-03-07","rentalEnd":"2026-03-14"}]}'),
(4, 4, 4, 600.00, NOW(), '{"deliveryType":"EXPRESS","status":"CONFIRMED","items":[{"productName":"Sony FX3 Cinema Line","quantity":4,"unitPrice":150.00,"rentalStart":"2026-03-20","rentalEnd":"2026-03-25"}]}');

INSERT INTO LoanLog (LoanLogId, RentalOrderLogId, LoanListId, LoanDate, DueDate, ReturnDate, DetailsJson) VALUES
(5, 1, 1, '2026-03-05 10:00:00+00', '2026-03-12 10:00:00+00', '2026-03-11 14:00:00+00', '{"remarks":"Customer self-collected at Orchard outlet","items":[{"loanItemId":1,"serialNumber":"R5-0001","productName":"Canon EOS R5","remarks":null},{"loanItemId":2,"serialNumber":"R5-0002","productName":"Canon EOS R5","remarks":null},{"loanItemId":3,"serialNumber":"R5-0003","productName":"Canon EOS R5","remarks":null}]}'),
(6, 2, 2, '2026-03-06 10:00:00+00', '2026-03-13 10:00:00+00', '2026-03-13 09:00:00+00', '{"remarks":"Standard dispatch","items":[{"loanItemId":4,"serialNumber":"R5-0004","productName":"Canon EOS R5","remarks":null},{"loanItemId":5,"serialNumber":"R5-0005","productName":"Canon EOS R5","remarks":null},{"loanItemId":6,"serialNumber":"R5-0006","productName":"Canon EOS R5","remarks":null}]}'),
(7, 3, 3, '2026-03-07 10:00:00+00', '2026-03-14 10:00:00+00', NULL, '{"remarks":"Extended loan requested","items":[{"loanItemId":7,"serialNumber":"RS3-0001","productName":"DJI RS 3 Gimbal Stabilizer","remarks":null},{"loanItemId":8,"serialNumber":"RS3-0002","productName":"DJI RS 3 Gimbal Stabilizer","remarks":null},{"loanItemId":9,"serialNumber":"RS3-0003","productName":"DJI RS 3 Gimbal Stabilizer","remarks":null}]}');

INSERT INTO ReturnLog (ReturnLogId, RentalOrderLogId, ReturnRequestId, CustomerId, RequestDate, CompletionDate, ImageUrl, DetailsJson) VALUES
(8, 1, 1, '1', '2026-03-11 14:00:00+00', '2026-03-11 15:00:00+00', NULL, '{"returnItemId":1,"serialNumber":"R5-0001","productName":"Canon EOS R5","status":"RETURN_TO_INVENTORY","completionDate":"2026-03-11","image":null}'),
(9, 2, 2, '2', '2026-03-13 09:00:00+00', NULL, NULL, '{"returnItemId":4,"serialNumber":"R5-0004","productName":"Canon EOS R5","status":"CLEANING","completionDate":null,"image":null}');

INSERT INTO PurchaseOrderLog (PoID, SupplierId, TotalAmount, PoDate, ExpectedDeliveryDate, DetailsJson) VALUES
(1, 1, 5500.00, '2026-02-15 10:00:00+00', '2026-02-22 10:00:00+00', '{"status":"COMPLETED","supplierName":"Camera Supply Co","lineItems":[{"productName":"Canon EOS R5","qty":5,"unitPrice":1100.00,"lineTotal":5500.00}]}');

INSERT INTO ClearanceLog (ClearanceLogId, ClearanceBatchId, BatchName, ClearanceDate, DetailsJson) VALUES
(11, 1, 'Winter 2026 Blowout', '2026-02-20 10:00:00+00', '{"batchStatus":"CLOSED","totalItemsCleared":1,"items":[{"clearanceItemId":1,"serialNumber":"RVP-OLD-001","productName":"Rode VideoMic Pro+","reason":"End of life","originalPrice":20.00,"clearancePrice":5.00}]}');

-- ============================================================
-- 8. ANALYTICS & ANALYTICSLIST
-- ============================================================
INSERT INTO Analytics (AnalyticsType, StartDate, EndDate, RefPrimaryName, RefPrimaryID, RefValue) VALUES
('DAILY',     '2026-03-01 00:00:00+00', '2026-03-01 23:59:59+00', '2026-03-01', NULL, NULL),
('SUPTREND',  '2026-01-01 00:00:00+00', '2026-12-31 23:59:59+00', 'Camera Supply Co', 1, 92.50),
('PRODTREND', '2026-01-01 00:00:00+00', '2026-12-31 23:59:59+00', 'Canon EOS R5', 1, 3.85);

INSERT INTO AnalyticsList (AnalyticsID, TransactionLogID) VALUES (1, 1);
INSERT INTO AnalyticsList (AnalyticsID, TransactionLogID) VALUES (2, 10);
INSERT INTO AnalyticsList (AnalyticsID, TransactionLogID) VALUES 
(3, 1), (3, 2), (3, 5), (3, 6), (3, 8), (3, 9);

-- Test account for authentication/authorization testing
BEGIN;

WITH inserted_users AS (
    INSERT INTO "User" (userrole, name, email, passwordhash, phonecountry, phonenumber)
    VALUES
        ('CUSTOMER', 'Alice Tan', 'alice@test.com', 'Test1234', 65, '91234567'),
        ('CUSTOMER', 'Bob Lim',   'bob@test.com',   'Test1234', 65, '92345678'),
        ('STAFF',    'Carol Ng',  'carol@test.com',  'Test1234', 65, '93456789'),
        ('ADMIN',    'Dave Ong',  'dave@test.com',   'Test1234', 65, '94567890')
    RETURNING userid, email, userrole
),

inserted_customers AS (
    INSERT INTO Customer (userid, address, customertype)
    SELECT userid,
           CASE email
               WHEN 'alice@test.com' THEN '10 Bedok North Ave 1, Singapore 460010'
               WHEN 'bob@test.com'   THEN '22 Tampines St 45, Singapore 520022'
           END,
           1
    FROM inserted_users
    WHERE email IN ('alice@test.com', 'bob@test.com')
    RETURNING customerid, userid
),

inserted_staff AS (
    INSERT INTO Staff (userid, department)
    SELECT userid, 'Operations'
    FROM inserted_users
    WHERE email = 'carol@test.com'
    RETURNING staffid, userid
)

SELECT
    u.email,
    u.userid,
    u.userrole,
    c.customerid,
    s.staffid
FROM inserted_users u
LEFT JOIN inserted_customers c ON c.userid = u.userid
LEFT JOIN inserted_staff     s ON s.userid = u.userid;

COMMIT;

-- Customer query test
SELECT c.customerid, u.email, u.name
FROM customer c
JOIN "User" u ON u.userid = c.userid
WHERE u.email = 'alice@test.com';

SELECT u.userid, u.email, u.userrole, s.staffid, s.department
FROM "User" u
JOIN staff s ON s.userid = u.userid
WHERE u.email = 'carol@test.com';