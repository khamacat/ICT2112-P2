-- Clear tables and resetting the Identity counters 
TRUNCATE 
    Category, "User", TransactionLog, Supplier, PurchaseOrder, 
    transportation_hub, transport, pricing_rule, carbon_result, 
    product_return, Analytics, PackagingMaterial,
    "transaction", "Order", Checkout, Cart, CartItem,
    Session, Payment, Deposit, replenishmentrequest, LoanList, ReturnRequest, ClearanceBatch 
RESTART IDENTITY CASCADE;
-- Team 2-3 Seed Data (non business such as loan, returns, clearance, damage reports and alerts not seeded)

INSERT INTO Category (Name, Description) VALUES
('Camera', 'Digital and mirrorless cameras'),
('Lens', 'Camera lenses'),
('Tripod', 'Camera tripods and stands'),
('Gimbal', 'Camera stabilizers'),
('Lighting', 'Studio and portable lighting'),
('Microphone', 'Audio recording equipment'),
('Memory Card', 'SD and CFExpress storage cards'),
('Camera Bag', 'Carrying bags and backpacks');

INSERT INTO Product (CategoryId, Sku, Threshold) VALUES
(1, 'CAM-CANON-R5', 0.10),
(1, 'CAM-SONY-A7IV', 0.10),
(2, 'LEN-SONY-2470GM', 0.10),
(2, 'LEN-CANON-70200RF', 0.10),
(3, 'TRI-MANFROTTO-BEFREE', 0.10),
(4, 'GIM-DJI-RS3', 0.10),
(6, 'MIC-RODE-VIDEOMICPRO', 0.10);

INSERT INTO ProductDetails 
(ProductId, TotalQuantity, Name, Description, Weight, Image, Price, DepositRate)
VALUES
(1, 5, 'Canon EOS R5', '45MP full-frame mirrorless camera', 0.74, 'canon_r5.jpg', 150.00, 0.30),
(2, 4, 'Sony A7 IV', '33MP hybrid mirrorless camera', 0.66, 'sony_a7iv.jpg', 130.00, 0.30),
(3, 6, 'Sony FE 24-70mm f2.8 GM', 'Professional standard zoom lens', 0.88, 'sony_2470gm.jpg', 90.00, 0.25),
(4, 3, 'Canon RF 70-200mm f2.8 L', 'Telephoto zoom lens', 1.07, 'canon_70200.jpg', 110.00, 0.25),
(5, 8, 'Manfrotto Befree Advanced Tripod', 'Portable travel tripod', 1.50, 'manfrotto_befree.jpg', 25.00, 0.10),
(6, 4, 'DJI RS 3 Gimbal Stabilizer', '3-axis camera stabilizer', 1.30, 'dji_rs3.jpg', 60.00, 0.20),
(7, 10, 'Rode VideoMic Pro+', 'Shotgun microphone for cameras', 0.12, 'rode_videomic.jpg', 20.00, 0.10);

INSERT INTO InventoryItem (ProductId, SerialNumber) VALUES
(1, 'R5-0001'),
(1, 'R5-0002'),
(1, 'R5-0003'),
(1, 'R5-0004'),
(1, 'R5-0005'),
(2, 'A7IV-0001'),
(2, 'A7IV-0002'),
(2, 'A7IV-0003'),
(2, 'A7IV-0004'),
(3,'2470GM-0001'),
(3,'2470GM-0002'),
(3,'2470GM-0003'),
(3,'2470GM-0004'),
(3,'2470GM-0005'),
(3,'2470GM-0006'),
(4,'70200RF-0001'),
(4,'70200RF-0002'),
(4,'70200RF-0003'),
(5,'TRI-0001'),
(5,'TRI-0002'),
(5,'TRI-0003'),
(5,'TRI-0004'),
(5,'TRI-0005'),
(5,'TRI-0006'),
(5,'TRI-0007'),
(5,'TRI-0008'),
(6,'RS3-0001'),
(6,'RS3-0002'),
(6,'RS3-0003'),
(6,'RS3-0004'),
(7,'RVP-0001'),
(7,'RVP-0002'),
(7,'RVP-0003'),
(7,'RVP-0004'),
(7,'RVP-0005'),
(7,'RVP-0006'),
(7,'RVP-0007'),
(7,'RVP-0008'),
(7,'RVP-0009'),
(7,'RVP-0010');

-- Team 2-4 Seed Data

INSERT INTO "User" (name, userRole, email, passwordHash, phoneCountry, phoneNumber)
VALUES
  ('Alice Tan',        'CUSTOMER', 'alice.tan@example.com',        '$2b$12$hashAlice', 65, '90000001'),
  ('Benjamin Lee',     'CUSTOMER', 'ben.lee@example.com',          '$2b$12$hashBen',   65, '90000002'),
  ('Charlotte Ng',     'CUSTOMER', 'charlotte.ng@example.com',     '$2b$12$hashChar',  65, '90000003'),
  ('Daniel Wong',      'CUSTOMER', 'daniel.wong@example.com',      '$2b$12$hashDan',   65, '90000004'),
  ('Elaine Goh',       'CUSTOMER', 'elaine.goh@example.com',       '$2b$12$hashEla',   65, '90000005'),
  ('Farid Ahmad',      'CUSTOMER', 'farid.ahmad@example.com',      '$2b$12$hashFarid', 65, '90000006'),
  ('Grace Lim',        'CUSTOMER', 'grace.lim@example.com',        '$2b$12$hashGrace', 65, '90000007'),
  ('Hannah Koh',       'CUSTOMER', 'hannah.koh@example.com',       '$2b$12$hashHan',   65, '90000008'),
  ('Ivan Tan',         'CUSTOMER', 'ivan.tan@example.com',         '$2b$12$hashIvan',  65, '90000009'),
  ('Jasmine Ong',      'CUSTOMER', 'jasmine.ong@example.com',      '$2b$12$hashJas',   65, '90000010'),
  ('Kevin Chan',       'CUSTOMER', 'kevin.chan@example.com',       '$2b$12$hashKev',   65, '90000011'),
  ('Lydia Chua',       'STAFF', 'lydia.chua@example.com',       '$2b$12$hashLyd',   65, '90000012'),
  ('Marcus Ho',        'STAFF', 'marcus.ho@example.com',        '$2b$12$hashMar',   65, '90000013'),
  ('Natalie Yeo',      'STAFF', 'natalie.yeo@example.com',      '$2b$12$hashNat',   65, '90000014'),
  ('Operations Admin', 'ADMIN', 'ops.admin@company.com',        '$2b$12$hashOps',   65, '90000015')
ON CONFLICT (email) DO NOTHING;

INSERT INTO Customer (userId, address, customerType)
VALUES
  (1, '123 Orchard Rd, Singapore 238123', 1),
  (2, '456 Marina Bay, Singapore 018972', 1),
  (3, '10 Tampines Ave, Singapore 529000', 2),
  (4, '5 Jurong East St, Singapore 609000', 2),
  (5, '88 Punggol Walk, Singapore 828000', 1),
  (6, '2 Clementi Rd, Singapore 129000', 2),
  (7, '12 Clementi Ave 1, Singapore 129012', 2),
  (8, '128 Plantation Crescent, Singapore 691128', 1),
  (9, '225 Bukit Batok Central, Singapore 650225', 2),
  (10, '503 Woodlands Ave 14, Singapore 730503', 2),
  (11, '224 Serangoon Ave 4, Singapore 334224', 2)
ON CONFLICT (userId) DO NOTHING;

INSERT INTO Staff (userId, department)
VALUES
  (12, 'Customer Support'),
  (13, 'Operations'),
  (14, 'Finance'),
  (15, 'IT')
ON CONFLICT (userId) DO NOTHING;

-- ============================================================
-- TEAM 1 SEED DATA - INSERT STATEMENTS (In Dependency Order)
-- ============================================================

-- LEVEL 1: Independent Tables (No FK dependencies)
-- ============================================================

-- Insert parent hubs
INSERT INTO transportation_hub (hub_type, longitude, latitude, country_code, address, operational_status, operation_time)
VALUES ('WAREHOUSE', 103.8198, 1.3521, 'SG', '1 Marina Boulevard, Singapore', 'OPERATIONAL', '24/7'),
       ('SHIPPING_PORT', 104.2167, 1.3667, 'SG', 'Port of Singapore, Pasir Ris, Singapore', 'OPERATIONAL', '6AM-11PM'),
       ('AIRPORT', 103.9914, 1.3644, 'SG', 'Changi Airport Terminal 3, Singapore', 'OPERATIONAL', '24/7'),
       ('WAREHOUSE', 114.1694, 22.3193, 'HK', '123 Industrial Road, Hong Kong', 'OPERATIONAL', '8AM-8PM');

-- Insert transport modes
INSERT INTO transport (transport_mode, max_load_kg, vehicle_size_m2, is_available)
VALUES ('TRUCK', 5000.0, 25.0, TRUE),
       ('TRUCK', 8000.0, 35.0, TRUE),
       ('SHIP', 50000.0, 500.0, TRUE),
       ('PLANE', 100000.0, 800.0, TRUE),
       ('TRAIN', 150000.0, 1200.0, TRUE);

-- Insert pricing rules
INSERT INTO pricing_rule (transport_mode, base_rate_per_km, is_active, carbon_surcharge)
VALUES ('TRUCK', 1.5, TRUE, 0.50),
       ('SHIP', 0.5, TRUE, 0.10),
       ('PLANE', 3.0, TRUE, 1.50),
       ('TRAIN', 1.0, TRUE, 0.05);

-- Insert carbon result records
INSERT INTO carbon_result (total_carbon_kg, created_at, validation_passed)
VALUES (250.50, NOW(), TRUE),
       (180.75, NOW(), TRUE),
       (420.30, NOW(), FALSE);

-- Insert product returns
INSERT INTO product_return (return_status, total_carbon, date_in, date_on)
VALUES ('PENDING', 45.25, CURRENT_DATE, CURRENT_DATE),
       ('COMPLETED', 78.90, CURRENT_DATE - INTERVAL '5 days', CURRENT_DATE - INTERVAL '2 days');

-- ============================================================
-- LEVEL 2: Tables with Single Parent Dependencies
-- ============================================================

-- Insert warehouse subtypes
INSERT INTO warehouse (hub_id, warehouse_code, max_product_capacity, total_warehouse_volume, climate_control_emission_rate, lighting_emission_rate, security_system_emission_rate)
VALUES (1, 'WH-SG-001', 10000, 5000.0, 2.5, 1.8, 0.5),
       (4, 'WH-HK-001', 8000, 4000.0, 2.2, 1.6, 0.4);

-- Insert shipping port subtype
INSERT INTO shipping_port (hub_id, port_code, port_name, port_type, vessel_size)
VALUES (2, 'SG-PORT', 'Port of Singapore', 'CONTAINER_PORT', 5000);

-- Insert airport subtype
INSERT INTO airport (hub_id, airport_code, airport_name, terminal, aircraft_size)
VALUES (3, 'SIN', 'Singapore Changi Airport', 3, 400);

-- Insert transport subtypes
INSERT INTO truck (transport_id, truck_id, truck_type, license_plate)
VALUES (1, 1001, 'FLATBED', 'SG-TRUCK-001'),
       (2, 1002, 'BOX_TRUCK', 'SG-TRUCK-002');

INSERT INTO ship (transport_id, ship_id, vessel_type, vessel_number, max_vessel_size)
VALUES (3, 2001, 'CONTAINER_SHIP', 'VESSEL-001', 'Panamax');

INSERT INTO plane (transport_id, plane_id, plane_type, plane_callsign)
VALUES (4, 3001, 'AIRBUS_A330', 'SQ-001');

INSERT INTO train (transport_id, train_id, train_type, train_number)
VALUES (5, 4001, 'FREIGHT', 'TRAIN-001');

-- Insert delivery batches
INSERT INTO delivery_batch (batch_weight_kg, destination_address, delivery_batch_status, total_orders, carbon_savings, hub_id)
VALUES (1500.0, 'Port of Singapore', 'PENDING', 5, 25.50, 1),
       (2000.0, 'Changi Airport', 'SHIPPEDOUT', 8, 35.75, 2),
       (800.0, 'Hong Kong Industrial', 'PENDING', 3, 12.30, 4);
-- ============================================================
-- TEAM 1 SEED DATA - END
-- ============================================================

-- Team 2-6 Seed Data

-- ================================================================
-- 1. SESSION (15 rows)
-- Roles: CUSTOMER (users 1–11), STAFF (users 12–15)
-- Mix of: active, expired
-- ================================================================
INSERT INTO Session (userId, role, createdAt, expiresAt) VALUES
(1,  'CUSTOMER', NOW() - INTERVAL '30 days',  NOW() - INTERVAL '29 days'),   -- 1  expired (old order)
(2,  'CUSTOMER', NOW() - INTERVAL '25 days',  NOW() - INTERVAL '24 days'),   -- 2  expired
(3,  'CUSTOMER', NOW() - INTERVAL '20 days',  NOW() - INTERVAL '19 days'),   -- 3  expired
(4,  'CUSTOMER', NOW() - INTERVAL '18 days',  NOW() - INTERVAL '17 days'),   -- 4  expired
(5,  'CUSTOMER', NOW() - INTERVAL '15 days',  NOW() - INTERVAL '14 days'),   -- 5  expired
(6,  'CUSTOMER', NOW() - INTERVAL '12 days',  NOW() - INTERVAL '11 days'),   -- 6  expired
(7,  'CUSTOMER', NOW() - INTERVAL '10 days',  NOW() - INTERVAL '9 days'),    -- 7  expired
(8,  'CUSTOMER', NOW() - INTERVAL '7 days',   NOW() - INTERVAL '6 days'),    -- 8  expired
(9,  'CUSTOMER', NOW() - INTERVAL '5 days',   NOW() - INTERVAL '4 days'),    -- 9  expired
(10, 'CUSTOMER', NOW() - INTERVAL '4 days',   NOW() - INTERVAL '3 days'),    -- 10 expired
(11, 'CUSTOMER', NOW() - INTERVAL '6 days',   NOW() - INTERVAL '5 days'),    -- 11 expired
(1,  'CUSTOMER', NOW() - INTERVAL '1 hour',   NOW() + INTERVAL '23 hours'),  -- 12 ACTIVE
(2,  'CUSTOMER', NOW() - INTERVAL '30 mins',  NOW() + INTERVAL '23 hours'),  -- 13 ACTIVE
(12, 'STAFF',    NOW() - INTERVAL '2 hours',  NOW() + INTERVAL '6 hours'),   -- 14 ACTIVE
(15, 'STAFF',    NOW() - INTERVAL '3 hours',  NOW() - INTERVAL '30 mins');   -- 15 expired

-- ================================================================
-- 2. CART (15 rows)
-- cart_status_enum: ACTIVE | CHECKED_OUT | EXPIRED
-- Each cart linked to a customer + the session from above
-- Carts 1–11: CHECKED_OUT (completed flow)
-- Cart 12–13: CHECKED_OUT (in-progress checkout)
-- Cart 14: ACTIVE (browsing, not checked out)
-- Cart 15: EXPIRED (abandoned)
-- ================================================================
INSERT INTO Cart (customerId, sessionId, rentalStart, rentalEnd, status) VALUES
-- Historical carts (all checked out / led to orders)
(1,  1,  NOW() - INTERVAL '30 days', NOW() - INTERVAL '27 days', 'CHECKED_OUT'),  -- 1
(2,  2,  NOW() - INTERVAL '25 days', NOW() - INTERVAL '22 days', 'CHECKED_OUT'),  -- 2
(3,  3,  NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', 'CHECKED_OUT'),  -- 3
(4,  4,  NOW() - INTERVAL '18 days', NOW() - INTERVAL '15 days', 'CHECKED_OUT'),  -- 4
(5,  5,  NOW() - INTERVAL '15 days', NOW() - INTERVAL '12 days', 'CHECKED_OUT'),  -- 5
(6,  6,  NOW() - INTERVAL '12 days', NOW() - INTERVAL '5 days',  'CHECKED_OUT'),  -- 6
(7,  7,  NOW() - INTERVAL '10 days', NOW() - INTERVAL '7 days',  'CHECKED_OUT'),  -- 7
(8,  8,  NOW() - INTERVAL '7 days',  NOW() - INTERVAL '4 days',  'CHECKED_OUT'),  -- 8
(9,  9,  NOW() + INTERVAL '2 days',  NOW() + INTERVAL '7 days',  'CHECKED_OUT'),  -- 9  future rental
(10, 10, NOW() - INTERVAL '4 days',  NOW() - INTERVAL '1 day',   'CHECKED_OUT'),  -- 10 cancelled checkout
(11, 11, NOW() + INTERVAL '5 days',  NOW() + INTERVAL '12 days', 'CHECKED_OUT'),  -- 11 future rental (customer 1 repeat)
-- Recent / in-progress
(1,  11, NOW() - INTERVAL '3 days',  NOW() - INTERVAL '1 day',   'CHECKED_OUT'),  -- 12 customer 11 order
(2,  13, NOW() + INTERVAL '3 days',  NOW() + INTERVAL '7 days',  'CHECKED_OUT'),  -- 13 pending checkout
(3,  12, NOW() + INTERVAL '1 day',   NOW() + INTERVAL '4 days',  'CHECKED_OUT'),  -- 14 pending checkout
-- Abandoned / active
(5,  5,  NOW() - INTERVAL '8 days',  NOW() - INTERVAL '5 days',  'EXPIRED');      -- 15 abandoned (led to cancelled order)

-- ================================================================
-- 3. CART ITEM
-- productId: 1=Canon R5 ($150), 2=Sony A7IV ($130), 3=Sony 24-70mm ($90),
--            4=Canon 70-200mm ($110), 5=Manfrotto Tripod ($25),
--            6=DJI RS3 ($60), 7=Rode VideoMic Pro+ ($20)
-- ================================================================
INSERT INTO CartItem (cartId, productId, quantity, isSelected) VALUES
-- Cart 1: Canon R5 + tripod
(1, 1, 1, TRUE),
(1, 5, 1, TRUE),
-- Cart 2: Sony A7IV + Sony 24-70mm
(2, 2, 1, TRUE),
(2, 3, 1, TRUE),
-- Cart 3: Sony 24-70mm + mic
(3, 3, 1, TRUE),
(3, 7, 1, TRUE),
-- Cart 4: Canon 70-200 + DJI RS3
(4, 4, 1, TRUE),
(4, 6, 1, TRUE),
-- Cart 5: Canon 70-200 only
(5, 4, 1, TRUE),
-- Cart 6: Full kit (R5 + Sony lens + tripod)
(6, 1, 1, TRUE),
(6, 3, 1, TRUE),
(6, 5, 1, TRUE),
-- Cart 7: Canon R5 + mic
(7, 1, 1, TRUE),
(7, 7, 1, TRUE),
-- Cart 8: Tripod + mic
(8, 5, 1, TRUE),
(8, 7, 1, TRUE),
-- Cart 9: Sony 24-70mm (future rental)
(9, 3, 1, TRUE),
-- Cart 10: Sony A7IV + Sony 24-70mm (cancelled)
(10, 2, 1, TRUE),
(10, 3, 1, TRUE),
-- Cart 11: Canon R5 + Sony 24-70mm (future rental)
(11, 1, 1, TRUE),
(11, 3, 1, TRUE),
-- Cart 12: DJI RS3 gimbal (customer 11 order)
(12, 6, 1, TRUE),
-- Cart 13: Sony A7IV (pending)
(13, 2, 1, TRUE),
-- Cart 14: DJI RS3 + tripod (pending)
(14, 6, 1, TRUE),
(14, 5, 1, TRUE),
-- Cart 15: R5 + Canon 70-200 + DJI RS3 (abandoned — cancelled order)
(15, 1, 1, TRUE),
(15, 4, 1, FALSE),  -- deselected before abandoning
(15, 6, 1, TRUE);

-- ================================================================
-- 4. CHECKOUT (15 rows)
-- checkout_status_enum: IN_PROGRESS | CONFIRMED | CANCELLED
-- All columns now provided including cartId
-- ================================================================
INSERT INTO Checkout (customerId, cartId, paymentMethodType, status, notifyOptIn, createdAt) VALUES
(1,  1,  'CREDIT_CARD', 'CONFIRMED',   TRUE,  NOW() - INTERVAL '30 days'),  -- 1
(2,  2,  'CREDIT_CARD', 'CONFIRMED',   FALSE, NOW() - INTERVAL '25 days'),  -- 2
(3,  3,  'CREDIT_CARD', 'CONFIRMED',   TRUE,  NOW() - INTERVAL '20 days'),  -- 3
(4,  4,  'CREDIT_CARD', 'CONFIRMED',   FALSE, NOW() - INTERVAL '18 days'),  -- 4
(5,  5,  'CREDIT_CARD', 'CONFIRMED',   TRUE,  NOW() - INTERVAL '15 days'),  -- 5
(6,  6,  'CREDIT_CARD', 'CONFIRMED',   TRUE,  NOW() - INTERVAL '12 days'),  -- 6
(7,  7,  'CREDIT_CARD', 'CONFIRMED',   FALSE, NOW() - INTERVAL '10 days'),  -- 7
(8,  8,  'CREDIT_CARD', 'CONFIRMED',   FALSE, NOW() - INTERVAL '7 days'),   -- 8
(9,  9,  'CREDIT_CARD', 'CONFIRMED',   TRUE,  NOW() - INTERVAL '5 days'),   -- 9
(10, 10, 'CREDIT_CARD', 'CANCELLED',   FALSE, NOW() - INTERVAL '4 days'),   -- 10
(1,  11, 'CREDIT_CARD', 'CONFIRMED',   TRUE,  NOW() - INTERVAL '3 days'),   -- 11
(11, 12, 'CREDIT_CARD', 'CONFIRMED',   FALSE, NOW() - INTERVAL '6 days'),   -- 12
(2,  13, 'CREDIT_CARD', 'IN_PROGRESS', FALSE, NOW() - INTERVAL '1 day'),    -- 13
(3,  14, 'CREDIT_CARD', 'IN_PROGRESS', TRUE,  NOW() - INTERVAL '2 hours'),  -- 14
(5,  15, 'CREDIT_CARD', 'CANCELLED',   FALSE, NOW() - INTERVAL '8 days');   -- 15


-- ================================================================
-- 5. TRANSACTION (one per order — covers all statuses)
-- transaction_type_enum:    PAYMENT | REFUND
-- transaction_purpose_enum: ORDER | PENALTY | REFUND_DEPOSIT
-- transaction_status_enum:  PENDING | COMPLETED | FAILED | CANCELLED
-- ================================================================
INSERT INTO Transaction (amount, type, purpose, status, providerTransactionId, createdAt) VALUES
-- Orders 1–3: DELIVERED → COMPLETED payments
(195.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_001', NOW() - INTERVAL '30 days'),  -- 1
(220.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_002', NOW() - INTERVAL '25 days'),  -- 2
(130.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_003', NOW() - INTERVAL '20 days'),  -- 3
-- Orders 4–5: DISPATCHED → COMPLETED
(175.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_004', NOW() - INTERVAL '18 days'),  -- 4
(110.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_005', NOW() - INTERVAL '15 days'),  -- 5
-- Orders 6–7: READY_FOR_DISPATCH → COMPLETED
(265.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_006', NOW() - INTERVAL '12 days'),  -- 6
(60.00,  'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_007', NOW() - INTERVAL '6 days'),   -- 7
-- Orders 8–9: PROCESSING → COMPLETED
(150.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_008', NOW() - INTERVAL '10 days'),  -- 8
(45.00,  'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_009', NOW() - INTERVAL '7 days'),   -- 9
-- Orders 10–11: CONFIRMED → COMPLETED
(90.00,  'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_010', NOW() - INTERVAL '5 days'),   -- 10
(210.00, 'PAYMENT', 'ORDER', 'COMPLETED', 'stripe_txn_011', NOW() - INTERVAL '3 days'),   -- 11
-- Orders 12–13: PENDING → PENDING (payment initiated, awaiting confirmation)
(155.00, 'PAYMENT', 'ORDER', 'PENDING',   'stripe_txn_012', NOW() - INTERVAL '1 day'),    -- 12
(80.00,  'PAYMENT', 'ORDER', 'PENDING',   'stripe_txn_013', NOW() - INTERVAL '2 hours'),  -- 13
-- Order 14: CANCELLED checkout → CANCELLED transaction
(130.00, 'PAYMENT', 'ORDER', 'CANCELLED', 'stripe_txn_014', NOW() - INTERVAL '4 days'),   -- 14
-- Order 15: CANCELLED large order → REFUND issued
(270.00, 'REFUND',  'ORDER', 'COMPLETED', 'stripe_txn_015', NOW() - INTERVAL '8 days'),   -- 15
-- Penalty transaction (late return on Order 1)
(30.00,  'PAYMENT', 'PENALTY',        'COMPLETED', 'stripe_txn_016', NOW() - INTERVAL '26 days'),  -- 16
-- Deposit refund transaction (Order 2 deposit returned)
(66.00,  'PAYMENT', 'REFUND_DEPOSIT', 'COMPLETED', 'stripe_txn_017', NOW() - INTERVAL '22 days');  -- 17


-- ================================================================
-- 6. ORDER (15 rows — all 7 statuses covered)
-- transactionId references Transaction rows above (1–15)
-- ================================================================
INSERT INTO "Order" (customerId, checkoutId, transactionId, orderDate, status, deliveryType, totalAmount) VALUES
-- DELIVERED
(1,  1,  1,    NOW() - INTERVAL '30 days', 'DELIVERED',          'NextDay',    195.00),  -- 1
(2,  2,  2,    NOW() - INTERVAL '25 days', 'DELIVERED',          'ThreeDays',  220.00),  -- 2
(3,  3,  3,    NOW() - INTERVAL '20 days', 'DELIVERED',          'NextDay',    130.00),  -- 3
-- DISPATCHED
(4,  4,  4,    NOW() - INTERVAL '18 days', 'DISPATCHED',         'ThreeDays',  175.00),  -- 4
(5,  5,  5,    NOW() - INTERVAL '15 days', 'DISPATCHED',         'NextDay',    110.00),  -- 5
-- READY_FOR_DISPATCH
(6,  6,  6,    NOW() - INTERVAL '12 days', 'READY_FOR_DISPATCH', 'OneWeek',    265.00),  -- 6
(11, 12, 7,    NOW() - INTERVAL '6 days',  'READY_FOR_DISPATCH', 'NextDay',     60.00),  -- 7
-- PROCESSING
(7,  7,  8,    NOW() - INTERVAL '10 days', 'PROCESSING',         'ThreeDays',  150.00),  -- 8
(8,  8,  9,    NOW() - INTERVAL '7 days',  'PROCESSING',         'NextDay',     45.00),  -- 9
-- CONFIRMED
(9,  9,  10,   NOW() - INTERVAL '5 days',  'CONFIRMED',          'ThreeDays',   90.00),  -- 10
(1,  11, 11,   NOW() - INTERVAL '3 days',  'CONFIRMED',          'OneWeek',    210.00),  -- 11
-- PENDING
(2,  13, 12,   NOW() - INTERVAL '1 day',   'PENDING',            'NextDay',    155.00),  -- 12
(3,  14, 13,   NOW() - INTERVAL '2 hours', 'PENDING',            'ThreeDays',   80.00),  -- 13
-- CANCELLED
(10, 10, 14,   NOW() - INTERVAL '4 days',  'CANCELLED',          'NextDay',    130.00),  -- 14
(5,  15, 15,   NOW() - INTERVAL '8 days',  'CANCELLED',          'OneWeek',    270.00);  -- 15

-- ================================================================
-- 7. ORDER ITEM
-- rentalStartDate/rentalEndDate = NULL → non-rental (purchase)
-- ================================================================
INSERT INTO OrderItem (orderId, productId, quantity, unitPrice, rentalStartDate, rentalEndDate) VALUES
-- Order 1: Canon R5 (rental) + tripod (purchase)
(1, 1, 1, 150.00, NOW() - INTERVAL '30 days', NOW() - INTERVAL '27 days'),
(1, 5, 1,  25.00, NULL, NULL),
-- Order 2: Sony A7IV (rental) + Sony 24-70mm (rental)
(2, 2, 1, 130.00, NOW() - INTERVAL '25 days', NOW() - INTERVAL '22 days'),
(2, 3, 1,  90.00, NOW() - INTERVAL '25 days', NOW() - INTERVAL '22 days'),
-- Order 3: Sony 24-70mm (rental) + mic (purchase)
(3, 3, 1,  90.00, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days'),
(3, 7, 1,  20.00, NULL, NULL),
-- Order 4: Canon 70-200 (rental) + DJI RS3 (rental)
(4, 4, 1, 110.00, NOW() - INTERVAL '18 days', NOW() - INTERVAL '15 days'),
(4, 6, 1,  60.00, NOW() - INTERVAL '18 days', NOW() - INTERVAL '15 days'),
-- Order 5: Canon 70-200 (rental)
(5, 4, 1, 110.00, NOW() - INTERVAL '15 days', NOW() - INTERVAL '12 days'),
-- Order 6: Canon R5 + Sony lens + tripod (all rental)
(6, 1, 1, 150.00, NOW() - INTERVAL '12 days', NOW() - INTERVAL '5 days'),
(6, 3, 1,  90.00, NOW() - INTERVAL '12 days', NOW() - INTERVAL '5 days'),
(6, 5, 1,  25.00, NOW() - INTERVAL '12 days', NOW() - INTERVAL '5 days'),
-- Order 7: DJI RS3 (rental)
(7, 6, 1,  60.00, NOW() - INTERVAL '6 days', NOW() - INTERVAL '3 days'),
-- Order 8: Canon R5 (rental) + mic (purchase)
(8, 1, 1, 150.00, NOW() - INTERVAL '10 days', NOW() - INTERVAL '7 days'),
(8, 7, 1,  20.00, NULL, NULL),
-- Order 9: Tripod (purchase) + mic (purchase) — non-rental order
(9, 5, 1, 25.00, NULL, NULL),
(9, 7, 1, 20.00, NULL, NULL),
-- Order 10: Sony 24-70mm (future rental, confirmed)
(10, 3, 1, 90.00, NOW() + INTERVAL '2 days', NOW() + INTERVAL '7 days'),
-- Order 11: Canon R5 + Sony 24-70mm (future rental, confirmed)
(11, 1, 1, 150.00, NOW() + INTERVAL '5 days', NOW() + INTERVAL '12 days'),
(11, 3, 1,  90.00, NOW() + INTERVAL '5 days', NOW() + INTERVAL '12 days'),
-- Order 12: Sony A7IV (future rental, pending)
(12, 2, 1, 130.00, NOW() + INTERVAL '3 days', NOW() + INTERVAL '7 days'),
-- Order 13: DJI RS3 + tripod (future rental, pending)
(13, 6, 1, 60.00, NOW() + INTERVAL '1 day', NOW() + INTERVAL '4 days'),
(13, 5, 1, 25.00, NOW() + INTERVAL '1 day', NOW() + INTERVAL '4 days'),
-- Order 14: Sony A7IV + Sony 24-70mm (cancelled — rental never started)
(14, 2, 1, 130.00, NOW() + INTERVAL '1 day', NOW() + INTERVAL '5 days'),
(14, 3, 1,  90.00, NOW() + INTERVAL '1 day', NOW() + INTERVAL '5 days'),
-- Order 15: Canon R5 + Canon 70-200 + DJI RS3 (cancelled)
(15, 1, 1, 150.00, NOW() + INTERVAL '2 days', NOW() + INTERVAL '9 days'),
(15, 4, 1, 110.00, NOW() + INTERVAL '2 days', NOW() + INTERVAL '9 days'),
(15, 6, 1,  60.00, NOW() + INTERVAL '2 days', NOW() + INTERVAL '9 days');

-- ================================================================
-- 8. PAYMENT (one per order, using Transaction ids 1–15)
-- payment_purpose_enum: RENTAL_FEE_DEPOSIT | PENALTY_FEE
-- paymentId = VARCHAR(50), using human-readable prefixed IDs
-- ================================================================
INSERT INTO Payment (paymentId, orderId, transactionId, amount, purpose, status, createdAt) VALUES
('PAY-ORD-001',  1,  1,  195.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '30 days'),
('PAY-ORD-002',  2,  2,  220.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '25 days'),
('PAY-ORD-003',  3,  3,  130.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '20 days'),
('PAY-ORD-004',  4,  4,  175.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '18 days'),
('PAY-ORD-005',  5,  5,  110.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '15 days'),
('PAY-ORD-006',  6,  6,  265.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '12 days'),
('PAY-ORD-007',  7,  7,   60.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '6 days'),
('PAY-ORD-008',  8,  8,  150.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '10 days'),
('PAY-ORD-009',  9,  9,   45.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '7 days'),
('PAY-ORD-010', 10, 10,   90.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '5 days'),
('PAY-ORD-011', 11, 11,  210.00, 'RENTAL_FEE_DEPOSIT', 'COMPLETED', NOW() - INTERVAL '3 days'),
('PAY-ORD-012', 12, 12,  155.00, 'RENTAL_FEE_DEPOSIT', 'PENDING',   NOW() - INTERVAL '1 day'),
('PAY-ORD-013', 13, 13,   80.00, 'RENTAL_FEE_DEPOSIT', 'PENDING',   NOW() - INTERVAL '2 hours'),
('PAY-ORD-014', 14, 14,  130.00, 'RENTAL_FEE_DEPOSIT', 'CANCELLED', NOW() - INTERVAL '4 days'),
('PAY-ORD-015', 15, 15,  270.00, 'RENTAL_FEE_DEPOSIT', 'CANCELLED', NOW() - INTERVAL '8 days'),
-- Penalty payment for Order 1 late return (uses Transaction 16)
('PAY-PEN-001',  1, 16,   30.00, 'PENALTY_FEE',        'COMPLETED', NOW() - INTERVAL '26 days');

-- ================================================================
-- 9. DEPOSIT (rental orders only — orders with rentalStartDate set)
-- DepositRate from ProductDetails: R5=30%, A7IV=30%, lenses=25%,
--   tripod=10%, gimbal=20%, mic=10%
-- Deposit = unitPrice * depositRate * quantity
-- Orders 1–11 have rentals; orders 9 (purchase-only) excluded.
-- Orders 14–15 cancelled → forfeitedAmount = 0, refundedAmount = originalAmount
-- ================================================================
INSERT INTO Deposit (depositId, orderId, transactionId, originalAmount, heldAmount, refundedAmount, forfeitedAmount, createdAt) VALUES

-- Order 1: Canon R5 deposit (150 * 30% = 45). Late return → partial forfeit
('DEP-ORD-001', 1,  1,  45.00, 45.00, 15.00, 30.00, NOW() - INTERVAL '30 days'),

-- Order 2: Sony A7IV (130*30%=39) + Sony 24-70mm (90*25%=22.50) = 61.50. Clean return → full refund
('DEP-ORD-002', 2,  2,  61.50, 61.50, 61.50,  0.00, NOW() - INTERVAL '25 days'),

-- Order 3: Sony 24-70mm (90*25%=22.50). Clean return → full refund
('DEP-ORD-003', 3,  3,  22.50, 22.50, 22.50,  0.00, NOW() - INTERVAL '20 days'),

-- Order 4: Canon 70-200 (110*25%=27.50) + DJI RS3 (60*20%=12) = 39.50. Clean → full refund
('DEP-ORD-004', 4,  4,  39.50, 39.50, 39.50,  0.00, NOW() - INTERVAL '18 days'),

-- Order 5: Canon 70-200 (110*25%=27.50). Clean → full refund
('DEP-ORD-005', 5,  5,  27.50, 27.50, 27.50,  0.00, NOW() - INTERVAL '15 days'),

-- Order 6: R5 (45) + Sony 24-70mm (22.50) + Tripod (25*10%=2.50) = 70. Still held (in dispatch)
('DEP-ORD-006', 6,  6,  70.00, 70.00,  0.00,  0.00, NOW() - INTERVAL '12 days'),

-- Order 7: DJI RS3 (60*20%=12). Still held
('DEP-ORD-007', 7,  7,  12.00, 12.00,  0.00,  0.00, NOW() - INTERVAL '6 days'),

-- Order 8: Canon R5 (45). Still held (processing)
('DEP-ORD-008', 8,  8,  45.00, 45.00,  0.00,  0.00, NOW() - INTERVAL '10 days'),

-- Order 10: Sony 24-70mm (22.50). Held (confirmed, rental not started)
('DEP-ORD-010', 10, 10, 22.50, 22.50,  0.00,  0.00, NOW() - INTERVAL '5 days'),

-- Order 11: R5 (45) + Sony 24-70mm (22.50) = 67.50. Held (confirmed)
('DEP-ORD-011', 11, 11, 67.50, 67.50,  0.00,  0.00, NOW() - INTERVAL '3 days'),

-- Order 12: Sony A7IV (39). Pending — not yet captured
('DEP-ORD-012', 12, 12, 39.00, 39.00,  0.00,  0.00, NOW() - INTERVAL '1 day'),

-- Order 13: DJI RS3 (12) + Tripod (2.50) = 14.50. Pending
('DEP-ORD-013', 13, 13, 14.50, 14.50,  0.00,  0.00, NOW() - INTERVAL '2 hours'),

-- Order 14: CANCELLED → full refund, nothing forfeited
('DEP-ORD-014', 14, 14, 32.50, 32.50, 32.50,  0.00, NOW() - INTERVAL '4 days'),
-- (A7IV 39 + Sony lens 22.50 = 61.50; but order was cancelled before payment completed — refund matches what was captured: 0 in practice. Using 32.50 as partial capture example)

-- Order 15: CANCELLED full kit → full refund
('DEP-ORD-015', 15, 15, 84.50, 84.50, 84.50,  0.00, NOW() - INTERVAL '8 days');
-- (R5 45 + Canon 70-200 27.50 + DJI RS3 12 = 84.50)

-- Team 5 seed--

--Packaging Material--
INSERT INTO PackagingMaterial (name, type, recyclable, reusable)
VALUES
('Protective Camera Foam Insert', 'Foam', FALSE, TRUE),
('Shockproof Equipment Hard Case', 'Plastic', TRUE, TRUE),
('Padded Equipment Transport Bag', 'Fabric', TRUE, TRUE),
('Reusable Plastic Transport Crate', 'Plastic', TRUE, TRUE),
('Corrugated Protective Shipping Box', 'Paper', TRUE, FALSE),
('Anti-Static Bubble Wrap', 'Plastic', FALSE, FALSE),
('Lens Protection Sleeve', 'Fabric', TRUE, TRUE),
('Tripod Protection Tube', 'Plastic', TRUE, TRUE),
('Camera Body Protective Wrap', 'Foam', FALSE, TRUE),
('Reusable Velcro Cable Organizer', 'Fabric', TRUE, TRUE),
('Silica Gel Moisture Pack', 'Chemical', FALSE, FALSE),
('Reusable Foam Divider Set', 'Foam', FALSE, TRUE),
('Protective Corner Guards', 'Plastic', TRUE, FALSE),
('Heavy Duty Equipment Case', 'Plastic', TRUE, TRUE),
('Reusable Equipment Strap Wrap', 'Fabric', TRUE, TRUE),
('Protective Microphone Case', 'Plastic', TRUE, TRUE),
('Lens Cushion Padding', 'Foam', FALSE, TRUE),
('Equipment Transport Carton', 'Paper', TRUE, FALSE),
('Reusable Camera Transport Box', 'Plastic', TRUE, TRUE),
('Shock Absorbing Packing Foam', 'Foam', FALSE, FALSE);

-- Team 2-2 Seed Data
-- PURCHASE ORDERS & STOCK
INSERT INTO PurchaseOrder (supplierID, poDate, status, expectedDeliveryDate, totalAmount) VALUES
(1, '2026-03-01', 'COMPLETED', '2026-03-05', 5500.00),
(2, '2026-03-10', 'CONFIRMED', '2026-03-25', 1200.00),
(3, '2026-03-15', 'SUBMITTED', '2026-03-30', 450.00);

INSERT INTO StockItem (productID, sku, name, uom) VALUES
(1, 'CAM-CANON-R5', 'Canon EOS R5 Body', 'Unit'),
(2, 'LEN-SONY-2470GM', 'Sony 24-70mm f/2.8 GM II', 'Unit'),
(3, 'ACC-DJI-RS3', 'DJI RS 3 Pro Gimbal', 'Unit'),
(4, 'LGT-APUTURE-300D', 'Aputure LS 300d II Light', 'Unit');

-- poID 1, 2, 3
INSERT INTO POLineItem (poID, productID, qty, unitPrice, lineTotal) VALUES
(1, 1, 2, 2500.00, 5000.00),
(1, 3, 1, 500.00, 500.00),
(2, 2, 1, 1200.00, 1200.00),
(3, 4, 1, 450.00, 450.00);

-- SUPPLIERS & VETTING
INSERT INTO Supplier (SupplierID, Name, Details, CreditPeriod, AvgTurnaroundTime, SupplierCategory, IsVerified, VettingResult) VALUES
(1, 'ShutterSpeed Wholesale', 'Main body and lens distributor', 30, 3.5, 'QUICKTURNAROUNDTIME', TRUE, 'APPROVED'),
(2, 'Vintage Glass Co.', 'Specialist in rare and analog lenses', 60, 14.2, 'LONGCREDITPERIOD', TRUE, 'APPROVED'),
(3, 'Nano-Tech Sensors', 'Startup for high-speed sensor repair parts', 15, 20.0, 'NEWUNTESTED', FALSE, 'PENDING');

INSERT INTO ReliabilityRating (SupplierID, Score, Rationale, RatingBand, CalculatedByUserID, CalculatedAt) VALUES
(1, 95.00, 'Excellent delivery speed.', 'HIGH', 1, '2026-01-15 10:00:00'),
(2, 75.00, 'Reliable but long lead times.', 'MEDIUM', 1, '2026-01-16 11:30:00');

INSERT INTO VettingRecord (RatingID, SupplierID, VettedByUserID, VettedAt, Decision, Notes) VALUES
(1, 1, 10, '2026-01-20 09:00:00', 'APPROVED', 'Highly recommended for bulk orders.'),
(2, 2, 10, '2026-01-21 14:00:00', 'APPROVED', 'Approved for niche vintage requests only.');

-- REPLENISHMENT
INSERT INTO ReplenishmentRequest (RequestedBy, Status, CreatedAt, Remarks, CompletedAt, CompletedBy) VALUES
('Warehouse_Steve', 'COMPLETED', '2026-02-01 08:00:00', 'Low stock on flagship bodies', '2026-02-05 17:00:00', 'Procurement_Jane'),
('Store_Manager_Ali', 'SUBMITTED', '2026-03-16 12:00:00', 'Restocking for summer wedding peak', NULL, NULL);

INSERT INTO LineItem (RequestId, ProductId, QuantityRequest, ReasonCode, Remarks) VALUES
(1, 1, 5, 'LOWSTOCK', 'Immediate need'),
(2, 2, 3, 'DEMANDSPIKE', 'Rental bookings increased');

-- 2-3 seed needed by 2-2
INSERT INTO LoanList (LoanListId, OrderId, CustomerId, LoanDate, DueDate, ReturnDate, Status, Remarks)
OVERRIDING SYSTEM VALUE
VALUES (1, 1, 1, '2026-03-05', '2026-03-12', NULL, 'ON_LOAN', 'Seed loan for log reference');

INSERT INTO ReturnRequest (ReturnRequestId, OrderId, CustomerId, Status, RequestDate, CompletionDate)
OVERRIDING SYSTEM VALUE
VALUES (1, 1, 1, 'COMPLETED', '2026-03-10', '2026-03-11');

INSERT INTO ClearanceBatch (ClearanceBatchId, BatchName, CreatedDate, ClearanceDate, Status)
OVERRIDING SYSTEM VALUE
VALUES (1, 'Quarterly Cleanup', '2026-03-01', '2026-03-14', 'CLOSED');

-- TransactionLog uses auto-generated IDs, so we don't need the override here
INSERT INTO TransactionLog (LogType, CreatedAt) VALUES
('PURCHASE_ORDER', '2026-03-01 09:00:00'), -- Will get ID 1
('RENTAL_ORDER',   '2026-03-05 10:00:00'), -- Will get ID 2
('LOAN',           '2026-03-05 11:00:00'), -- Will get ID 3
('RETURN',         '2026-03-10 16:00:00'), -- Will get ID 4
('CLEARANCE',      '2026-03-14 09:00:00'); -- Will get ID 5

-- Use OVERRIDING SYSTEM VALUE because these IDs must match TransactionLog
INSERT INTO PurchaseOrderLog (PurchaseOrderLogId, PoID, PoDate, SupplierId, Status, ExpectedDeliveryDate, TotalAmount, DetailsJSON) 
OVERRIDING SYSTEM VALUE
VALUES (1, 1, '2026-03-01', 1, 'COMPLETED', '2026-03-05', 5500.00, '{"note": "Initial spring stock"}');

INSERT INTO RentalOrderLog (RentalOrderLogId, OrderId, CustomerId, OrderDate, TotalAmount, DeliveryType, Status, DetailsJSON) 
OVERRIDING SYSTEM VALUE
VALUES (2, 1, 1, '2026-03-05', 300.00, 'EXPRESS', 'COMPLETED', '{"items": ["Canon R5"]}');

INSERT INTO LoanLog (LoanLogId, LoanListId, RentalOrderLogId, Status, LoanDate, ReturnDate, DueDate, DetailsJSON) 
OVERRIDING SYSTEM VALUE
VALUES (3, 1, 2, 'ONGOING', '2026-03-05', NULL, '2026-03-12', '{"assetTag": "PR-001"}');

INSERT INTO ReturnLog (ReturnLogId, ReturnRequestId, RentalOrderLogId, CustomerId, Status, RequestDate, CompletionDate, ImageURL, DetailsJSON) 
OVERRIDING SYSTEM VALUE
VALUES (4, 1, 2, '1', 'COMPLETED', '2026-03-10', '2026-03-11', 'img_ret_4.jpg', '{"condition": "Clean"}');

INSERT INTO ClearanceLog (ClearanceLogId, ClearanceBatchId, BatchName, ClearanceDate, Status, DetailsJSON) 
OVERRIDING SYSTEM VALUE
VALUES (5, 1, 'Quarterly Cleanup', '2026-03-14', 'COMPLETED', '{"discardedCount": 5}');

-- ANALYTICS
INSERT INTO Analytics (AnalyticsType, StartDate, EndDate, LoanAmt, ReturnAmt, RefPrimaryID, RefPrimaryName, RefValue) VALUES
('DAILY', '2026-03-01', '2026-03-01', 1, 1, 1, '01032026 Logs', 0.00);

INSERT INTO ReportExport (RefAnalyticsID, Title, VisualType, FileFormat, URL) VALUES
(1, 'March 1 Daily Log', 'BAR', 'PDF', 'https://cdn.prorental.com/reports/mar2026.pdf');

INSERT INTO AnalyticsList (AnalyticsID, TransactionLogID) VALUES
(1, 3),
(1, 4);