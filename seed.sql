-- Team 2-3 Seed Data

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

INSERT INTO "User" (name, email, passwordHash, phoneCountry, phoneNumber)
VALUES
  ('Alice Tan',        'alice.tan@example.com',        '$2b$12$hashAlice', 65, '90000001'),
  ('Benjamin Lee',     'ben.lee@example.com',          '$2b$12$hashBen',   65, '90000002'),
  ('Charlotte Ng',     'charlotte.ng@example.com',     '$2b$12$hashChar',  65, '90000003'),
  ('Daniel Wong',      'daniel.wong@example.com',      '$2b$12$hashDan',   65, '90000004'),
  ('Elaine Goh',       'elaine.goh@example.com',       '$2b$12$hashEla',   65, '90000005'),
  ('Farid Ahmad',      'farid.ahmad@example.com',      '$2b$12$hashFarid', 65, '90000006'),
  ('Grace Lim',        'grace.lim@example.com',        '$2b$12$hashGrace', 65, '90000007'),
  ('Hannah Koh',       'hannah.koh@example.com',       '$2b$12$hashHan',   65, '90000008'),
  ('Ivan Tan',         'ivan.tan@example.com',         '$2b$12$hashIvan',  65, '90000009'),
  ('Jasmine Ong',      'jasmine.ong@example.com',      '$2b$12$hashJas',   65, '90000010'),
  ('Kevin Chan',       'kevin.chan@example.com',       '$2b$12$hashKev',   65, '90000011'),
  ('Lydia Chua',       'lydia.chua@example.com',       '$2b$12$hashLyd',   65, '90000012'),
  ('Marcus Ho',        'marcus.ho@example.com',        '$2b$12$hashMar',   65, '90000013'),
  ('Natalie Yeo',      'natalie.yeo@example.com',      '$2b$12$hashNat',   65, '90000014'),
  ('Operations Admin', 'ops.admin@company.com',        '$2b$12$hashOps',   65, '90000015')
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
  (11, '224 Serangoon Ave 4, Singapore 334224', 2),
ON CONFLICT (userId) DO NOTHING;

INSERT INTO Staff (userId, department)
VALUES
  (12, 'Customer Support'),
  (13, 'Operations'),
  (14, 'Finance'),
  (15, 'IT')
ON CONFLICT (userId) DO NOTHING;