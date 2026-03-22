-- Order Rental Processing System Schema
-- Version 1.1
-- Created: 28 Feb 2026
-- Revised: 03 Mar 2026 — Converted to pure PostgreSQL
--   * Removed T-SQL (GO, USE DATABASE) and MySQL syntax
--   * Converted all inline ENUM to named CREATE TYPE
--   * AUTO_INCREMENT → GENERATED ALWAYS AS IDENTITY
--   * DATETIME → TIMESTAMPTZ
--   * Fixed broken FK targets, duplicate tables, forward references

-- ============================================================
-- ENUM Types (global — all teams)
-- ============================================================

-- TEAM 1
CREATE TYPE transport_mode AS ENUM (
    'TRUCK', 'SHIP', 'PLANE', 'TRAIN'
);

CREATE TYPE preference_type AS ENUM (
    'FAST', 'CHEAP', 'GREEN'
);

CREATE TYPE batch_status AS ENUM (
    'PENDING', 'SHIPPEDOUT'
);

CREATE TYPE stagetype AS ENUM (
    'INSPECTION', 'REPAIRING', 'SERVICING', 'CLEANING', 'INV_RETURN'
);

CREATE TYPE carbon_stage_type AS ENUM (
    'DAMAGE_INSPECTION', 'REPAIRING', 'SERVICING', 'CLEANING', 'RETURN'
);

CREATE TYPE hub_type AS ENUM (
    'WAREHOUSE', 'SHIPPING_PORT', 'AIRPORT'
);

-- TEAM 2
-- Supplier & Vetting
CREATE TYPE supplier_category_enum AS ENUM ('LONGCREDITPERIOD', 'QUICKTURNAROUNDTIME', 'NEWUNTESTED');
CREATE TYPE vetting_result_enum AS ENUM ('APPROVED', 'REJECTED', 'PENDING');
CREATE TYPE vetting_decision_enum AS ENUM ('APPROVED', 'REJECTED', 'PENDING');
CREATE TYPE rating_band_enum AS ENUM ('HIGH', 'MEDIUM', 'LOW', 'UNRATED');

-- Purchase Replenisihment Request
CREATE TYPE po_status_enum AS ENUM ('COMPLETED', 'CONFIRMED', 'SUBMITTED', 'APPROVED', 'REJECTED', 'CANCELLED');
CREATE TYPE replenishment_status_enum AS ENUM ('DRAFT', 'SUBMITTED', 'CANCELLED', 'COMPLETED');
CREATE TYPE reason_code_enum AS ENUM ('LOWSTOCK', 'DEMANDSPIKE', 'REPLACEMENT', 'NEWITEM', 'OTHERS');

-- Logging
CREATE TYPE log_type_enum AS ENUM ('RENTAL_ORDER', 'LOAN', 'RETURN', 'PURCHASE_ORDER', 'CLEARANCE');
CREATE TYPE delivery_type_enum AS ENUM ('STANDARD', 'EXPRESS', 'SELF_PICKUP');
CREATE TYPE rental_status_enum AS ENUM ('PENDING', 'CONFIRMED', 'CANCELLED', 'COMPLETED');
CREATE TYPE loan_status_enum AS ENUM ('ONGOING', 'RETURNED', 'OVERDUE', 'CANCELLED');
CREATE TYPE return_status_enum AS ENUM ('PENDING', 'APPROVED', 'REJECTED', 'COMPLETED');
CREATE TYPE purchase_order_status_enum AS ENUM ('PENDING', 'APPROVED', 'REJECTED', 'DELIVERED', 'CANCELLED');
CREATE TYPE clearance_status_enum AS ENUM ('ONGOING', 'COMPLETED', 'CANCELLED');

-- Analytics
CREATE TYPE analytics_type_enum AS ENUM ('DAILY', 'SUPTREND', 'PRODTREND');
CREATE TYPE visual_type_enum AS ENUM ('TABLE', 'BAR', 'COLUMN', 'LINE', 'PIE', 'AREA');
CREATE TYPE file_format_enum AS ENUM ('CSV', 'XLSX', 'PDF', 'PNG');

-- TEAM 4
CREATE TYPE notification_type_enum AS ENUM ('ORDER_UPDATE', 'PROMOTION', 'SYSTEM', 'PRODUCT');

CREATE TYPE notification_frequency_enum AS ENUM ('INSTANT', 'DAILY', 'WEEKLY');

CREATE TYPE notification_granularity_enum AS ENUM ('ALL', 'IMPORTANT_ONLY', 'NONE');

-- Aligns with order_status_enum (Team 6); SHIPPED → DISPATCHED for consistency
CREATE TYPE order_history_status_enum AS ENUM (
    'PENDING', 'CONFIRMED', 'PROCESSING',
    'READY_FOR_DISPATCH', 'DISPATCHED', 'DELIVERED', 'CANCELLED'
);

CREATE TYPE shipment_status_enum AS ENUM ('PENDING', 'IN_TRANSIT', 'DELIVERED', 'CANCELLED');

CREATE TYPE user_role_enum AS ENUM ('CUSTOMER', 'STAFF', 'ADMIN');

--TEAM 1 PRIMARY KEY TABLES
-- ============================================================
-- TransportationHub (Parent — Table-Per-Subtype Inheritance)
-- ============================================================
CREATE TABLE transportation_hub (
    hub_id             INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    hub_type           hub_type         NOT NULL,
    longitude          DOUBLE PRECISION NOT NULL,
    latitude           DOUBLE PRECISION NOT NULL,
    country_code       VARCHAR(10)      NOT NULL,
    address            VARCHAR(255)     NOT NULL,
    operational_status VARCHAR(50),
    operation_time     VARCHAR(50)
);

CREATE TABLE warehouse (
    hub_id                        INT PRIMARY KEY,
    warehouse_code                VARCHAR(100) NOT NULL,
    max_product_capacity          INT,
    total_warehouse_volume        DOUBLE PRECISION,
    climate_control_emission_rate DOUBLE PRECISION,
    lighting_emission_rate        DOUBLE PRECISION,
    security_system_emission_rate DOUBLE PRECISION,
    CONSTRAINT fk_warehouse_hub FOREIGN KEY (hub_id)
        REFERENCES transportation_hub(hub_id) ON DELETE CASCADE
);

CREATE TABLE shipping_port (
    hub_id      INT PRIMARY KEY,
    port_code   VARCHAR(20)  NOT NULL,
    port_name   VARCHAR(255) NOT NULL,
    port_type   VARCHAR(50),
    vessel_size INT,
    CONSTRAINT fk_shipping_port_hub FOREIGN KEY (hub_id)
        REFERENCES transportation_hub(hub_id) ON DELETE CASCADE
);

CREATE TABLE airport (
    hub_id       INT PRIMARY KEY,
    airport_code VARCHAR(10)  NOT NULL,
    airport_name VARCHAR(255) NOT NULL,
    terminal     INT,
    aircraft_size INT,
    CONSTRAINT fk_airport_hub FOREIGN KEY (hub_id)
        REFERENCES transportation_hub(hub_id) ON DELETE CASCADE
);


-- ============================================================
-- Transport (Parent — Table-Per-Subtype Inheritance)
-- ============================================================
CREATE TABLE transport (
    transport_id    INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    transport_mode  transport_mode   NOT NULL,
    max_load_kg     DOUBLE PRECISION,
    vehicle_size_m2 DOUBLE PRECISION,
    is_available    BOOLEAN          DEFAULT TRUE
);

CREATE TABLE truck (
    transport_id  INT PRIMARY KEY,
    truck_id      INT         NOT NULL,
    truck_type    VARCHAR(50),
    license_plate VARCHAR(50),
    CONSTRAINT fk_truck_transport FOREIGN KEY (transport_id)
        REFERENCES transport(transport_id) ON DELETE CASCADE
);

CREATE TABLE ship (
    transport_id    INT PRIMARY KEY,
    ship_id         INT         NOT NULL,
    vessel_type     VARCHAR(50),
    vessel_number   VARCHAR(50),
    max_vessel_size VARCHAR(50),
    CONSTRAINT fk_ship_transport FOREIGN KEY (transport_id)
        REFERENCES transport(transport_id) ON DELETE CASCADE
);

CREATE TABLE plane (
    transport_id   INT PRIMARY KEY,
    plane_id       INT         NOT NULL,
    plane_type     VARCHAR(50),
    plane_callsign VARCHAR(50),
    CONSTRAINT fk_plane_transport FOREIGN KEY (transport_id)
        REFERENCES transport(transport_id) ON DELETE CASCADE
);

CREATE TABLE train (
    transport_id  INT PRIMARY KEY,
    train_id      INT         NOT NULL,
    train_type    VARCHAR(50),
    train_number  VARCHAR(50),
    CONSTRAINT fk_train_transport FOREIGN KEY (transport_id)
        REFERENCES transport(transport_id) ON DELETE CASCADE
);


-- ============================================================
-- Route & Route Legs
-- ============================================================
CREATE TABLE delivery_route (
    route_id            INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    origin_address      VARCHAR(255)               NOT NULL,
    destination_address VARCHAR(255)               NOT NULL,
    total_distance_km   DOUBLE PRECISION,
    is_valid            BOOLEAN                    DEFAULT TRUE,
    origin_hub_id       INT,
    destination_hub_id  INT,
    CONSTRAINT fk_route_origin_hub      FOREIGN KEY (origin_hub_id)
        REFERENCES transportation_hub(hub_id),
    CONSTRAINT fk_route_destination_hub FOREIGN KEY (destination_hub_id)
        REFERENCES transportation_hub(hub_id)
);

CREATE TABLE route_leg (
    leg_id         INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    route_id       INT            NOT NULL,
    sequence       INT,
    transport_mode transport_mode,
    start_point    VARCHAR(255),
    end_point      VARCHAR(255),
    distance_km    DOUBLE PRECISION,
    is_first_mile  BOOLEAN        DEFAULT FALSE,
    is_last_mile   BOOLEAN        DEFAULT FALSE,
    transport_id   INT,
    CONSTRAINT fk_route_leg_route     FOREIGN KEY (route_id)
        REFERENCES delivery_route(route_id),
    CONSTRAINT fk_route_leg_transport FOREIGN KEY (transport_id)
        REFERENCES transport(transport_id)
);


-- ============================================================
-- Carbon Results & Leg Carbon
-- ============================================================
CREATE TABLE carbon_result (
    carbon_result_id  INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    total_carbon_kg   DOUBLE PRECISION,
    created_at        TIMESTAMPTZ DEFAULT NOW(),
    validation_passed BOOLEAN   DEFAULT FALSE
);

CREATE TABLE leg_carbon (
    leg_id           INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    transport_mode   transport_mode,
    distance_km      DOUBLE PRECISION,
    weight_kg        DOUBLE PRECISION,
    carbon_kg        DOUBLE PRECISION,
    carbon_rate      DOUBLE PRECISION,
    carbon_result_id INT,
    route_leg_id     INT,
    CONSTRAINT fk_leg_carbon_result FOREIGN KEY (carbon_result_id)
        REFERENCES carbon_result(carbon_result_id),
    CONSTRAINT fk_leg_carbon_leg    FOREIGN KEY (route_leg_id)
        REFERENCES route_leg(leg_id)
);


-- ============================================================
-- Shipping Options & Pricing Rules
-- ============================================================
CREATE TABLE shipping_option (
    option_id        INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    display_name     VARCHAR(255),
    cost             NUMERIC(10, 2),
    carbonFootprintKg DOUBLE PRECISION,
    delivery_days    INT,
    preference_type  preference_type,
    order_id         INT,
    transport_mode   transport_mode,
    route_id         INT,
    CONSTRAINT fk_shipping_option_route FOREIGN KEY (route_id)
        REFERENCES delivery_route(route_id)
);

CREATE TABLE pricing_rule (
    rule_id          INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    transport_mode   transport_mode,
    base_rate_per_km NUMERIC(10, 4),
    is_active        BOOLEAN        DEFAULT TRUE,
    -- NOTE: ERD typed carbon_surcharge as DateTime — corrected to NUMERIC(10,4)
    carbon_surcharge NUMERIC(10, 4)
);


-- ============================================================
-- Customer Choice (Composite PK)
-- NOTE: customer_id and order_id reference external Customer/Order
--       tables not defined in this ERD — FK constraints omitted.
-- ============================================================
CREATE TABLE customer_choice (
    customer_id     INT             NOT NULL,
    order_id        INT             NOT NULL,
    preference_type preference_type,
    created_at      TIMESTAMPTZ       DEFAULT NOW(),
    PRIMARY KEY (customer_id, order_id)
);


-- ============================================================
-- Delivery Batch & Batch Orders
-- NOTE: batch_order.order_id references an external Order table
--       not defined in this ERD — FK constraint omitted.
-- ============================================================
CREATE TABLE delivery_batch (
    delivery_batch_id     INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    batch_weight_kg       DOUBLE PRECISION,
    destination_address   VARCHAR(255),
    delivery_batch_status batch_status     DEFAULT 'PENDING',
    total_orders          INT              DEFAULT 0,
    carbon_savings        DOUBLE PRECISION,
    hub_id         INT,
    CONSTRAINT fk_delivery_batch_hub FOREIGN KEY (hub_id)
        REFERENCES transportation_hub(hub_id)
);


-- ============================================================
-- Product Return & Return Stages
-- ============================================================
CREATE TABLE product_return (
    return_id     INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    return_status VARCHAR(50),
    total_carbon  DOUBLE PRECISION,
    date_in       DATE,
    date_on       DATE
);

CREATE TABLE return_stage (
    stage_id              INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    return_id             INT              NOT NULL,
    stage_type            carbon_stage_type,          -- use defined ENUM (was VARCHAR(50))
    stageType             stagetype,
    energy_kwh            DOUBLE PRECISION,
    labour_hours          DOUBLE PRECISION,
    materials_kg          DOUBLE PRECISION,
    cleaning_supplies_qty DOUBLE PRECISION,
    water_litres          DOUBLE PRECISION,
    packaging_kg          DOUBLE PRECISION,
    surcharge_rate        NUMERIC(10, 4)
);

CREATE TABLE carbon_emission (
    emission_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    stage_id    INT               NOT NULL,
    carbon_kg   DOUBLE PRECISION,
    stage_type  carbon_stage_type,
    CONSTRAINT fk_carbon_emission_stage FOREIGN KEY (stage_id)
        REFERENCES return_stage(stage_id)
);


-- TEAM 1 CROSS TEAM FK TABLES
-- this was made because batch_order has many orders and order can only belong to one batch, so we need a separate table to link them together. 
-- either this or order table has to have batch_id as FK, and we chose to have a separate table to avoid having a nullable batch_id in order table.
CREATE TABLE batch_order ( 
    batch_id        INT       NOT NULL,
    order_id        INT       NOT NULL UNIQUE,
    added_timestamp TIMESTAMPTZ DEFAULT NOW(),
    PRIMARY KEY (batch_id, order_id),
    CONSTRAINT fk_batch_order_batch FOREIGN KEY (batch_id)
        REFERENCES delivery_batch(delivery_batch_id)
    -- FK to "Order"(orderId) is deferred to the end of the file
);
-- NOTE: customer_choice FKs to Customer and "Order" are deferred to the
--       end of the file (after those tables are created by Team 4/6).


-- TEAM 1 END

--TEAM 2 PRIMARY KEY TABLES
-- PURCHASE ORDER & STOCK --
CREATE TABLE IF NOT EXISTS PurchaseOrder (
    poID                 INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    supplierID           INT,
    poDate               DATE,
    status               po_status_enum,
    expectedDeliveryDate DATE,
    totalAmount          DECIMAL(10,2)
);

CREATE TABLE IF NOT EXISTS StockItem (
    productID INT PRIMARY KEY,
    sku       VARCHAR(100),
    name      VARCHAR(255),
    uom       VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS POLineItem (
    poLineID  INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    poID      INT,
    productID INT,
    qty       INT,
    unitPrice DECIMAL(10,2),
    lineTotal DECIMAL(10,2),
    CONSTRAINT fk_polineitem_po FOREIGN KEY (poID) REFERENCES PurchaseOrder(poID) ON DELETE CASCADE,
    CONSTRAINT fk_product_stock FOREIGN KEY (productID) REFERENCES StockItem(productID) ON DELETE CASCADE
);

-- SUPPLIER & VETTING --
CREATE TABLE IF NOT EXISTS Supplier (
    SupplierID        INT PRIMARY KEY,
    Name              VARCHAR(255),
    Details           VARCHAR(500),
    CreditPeriod      INT,
    AvgTurnaroundTime DOUBLE PRECISION,
    SupplierCategory  supplier_category_enum,
    IsVerified        BOOLEAN,
    VettingResult     vetting_result_enum
);

CREATE TABLE IF NOT EXISTS SupplierCategoryChangeLog (
    LogID            INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    SupplierID       INT,
    PreviousCategory supplier_category_enum,
    NewCategory      supplier_category_enum,
    ChangeReason     VARCHAR(255),
    ChangedAt        TIMESTAMPTZ,
    CONSTRAINT fk_suppliercatelog_supplier FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID) ON DELETE CASCADE
);

-- ReliabilityRating must precede VettingRecord because VettingRecord.RatingID references it
CREATE TABLE IF NOT EXISTS ReliabilityRating (
    RatingID           INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    SupplierID         INT,
    Score              DECIMAL(5,2),
    Rationale          TEXT,
    RatingBand         rating_band_enum,
    CalculatedByUserID INT,
    CalculatedAt       TIMESTAMPTZ,
    CONSTRAINT fk_reliabilityrating_supplier FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS VettingRecord (
    VettingID      INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    RatingID       INT,
    SupplierID     INT,
    VettedByUserID INT,
    VettedAt       TIMESTAMPTZ,
    Decision       vetting_decision_enum,
    Notes          TEXT,
    CONSTRAINT fk_vettingrecord_rating   FOREIGN KEY (RatingID)   REFERENCES ReliabilityRating(RatingID) ON DELETE SET NULL,
    CONSTRAINT fk_vettingrecord_supplier FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)        ON DELETE SET NULL
);

-- REPLENISHMENT --
-- NOTE: LineItem has been deferred to CROSS FK TABLE section
CREATE TABLE IF NOT EXISTS ReplenishmentRequest (
    RequestId   INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    RequestedBy VARCHAR(255),
    Status      replenishment_status_enum,
    CreatedAt   TIMESTAMPTZ,
    Remarks     TEXT,
    CompletedAt TIMESTAMPTZ,
    CompletedBy VARCHAR(255)
);

-- TRANSACTION LOG TABLES
-- NOTE: All other log subtypes are deferred to CROSS FK TABLE section
CREATE TABLE IF NOT EXISTS TransactionLog (
    TransactionLogID INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    LogType log_type_enum NOT NULL,
    CreatedAt TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS PurchaseOrderLog (
    PurchaseOrderLogId      INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    PoID                    INT NOT NULL,
    PoDate                  TIMESTAMPTZ,
    SupplierId              INT,
    Status                  rental_status_enum,
    ExpectedDeliveryDate    TIMESTAMPTZ,
    TotalAmount             DECIMAL(10,2),
    DetailsJSON             TEXT,

    CONSTRAINT fk_po_transaction FOREIGN KEY (PurchaseOrderLogId) REFERENCES TransactionLog(TransactionLogID) ON DELETE CASCADE,
    CONSTRAINT fk_po_log_po      FOREIGN KEY (PoID)               REFERENCES PurchaseOrder(poID)              ON DELETE CASCADE
);

-- ANALYTICS TABLES --
CREATE TABLE IF NOT EXISTS Analytics (
    AnalyticsID         INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    AnalyticsType       analytics_type_enum,
    StartDate           TIMESTAMPTZ,
    EndDate             TIMESTAMPTZ,
    LoanAmt             INT,
    ReturnAmt           INT,
    RefPrimaryID        INT,
    RefPrimaryName      VARCHAR(255),
    RefValue            DECIMAL(10,2)
);

CREATE TABLE IF NOT EXISTS ReportExport (
    ReportID       INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    RefAnalyticsID INT,
    Title          VARCHAR(255),
    VisualType     visual_type_enum,
    FileFormat     file_format_enum,
    URL            VARCHAR(500),
    CONSTRAINT fk_reportexport_analytics FOREIGN KEY (RefAnalyticsID) REFERENCES Analytics(AnalyticsID) ON DELETE SET NULL
);

-- AnalyticsList links an analytics snapshot to a transaction log entry.
-- TransactionLog is the transaction source for analytics. 
CREATE TABLE IF NOT EXISTS AnalyticsList (
    AnalyticsID      INT NOT NULL,
    TransactionLogID INT NOT NULL,
    PRIMARY KEY (AnalyticsID, TransactionLogID),
    CONSTRAINT fk_AnalyticsList_analytics FOREIGN KEY (AnalyticsID)      REFERENCES Analytics(AnalyticsID)            ON DELETE CASCADE,
    CONSTRAINT fk_AnalyticsList_log       FOREIGN KEY (TransactionLogID) REFERENCES TransactionLog(TransactionLogID)  ON DELETE CASCADE
);


--TEAM 3 PRIMARY KEY TABLES
CREATE TYPE product_status AS ENUM ('AVAILABLE', 'UNAVAILABLE', 'RETIRED');

CREATE TYPE inventory_status AS ENUM 
('AVAILABLE', 'RETIRED', 'CLEARANCE', 'SOLD', 
 'MAINTENANCE', 'RESERVED', 'ON_LOAN', 'BROKEN');

CREATE TYPE alert_status AS ENUM ('OPEN', 'ACKNOWLEDGED', 'RESOLVED');

CREATE TYPE clearance_status AS ENUM ('CLEARANCE', 'SOLD');

CREATE TYPE clearance_batch_status AS ENUM ('SCHEDULED', 'ACTIVE', 'CLOSED');

CREATE TABLE Category (
    CategoryId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    CreatedDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Product (
    ProductId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    CategoryId INT NOT NULL,
    Sku VARCHAR(255) NOT NULL,
    Status product_status NOT NULL DEFAULT 'AVAILABLE',
    Threshold DECIMAL(5,4) NOT NULL DEFAULT 0.0,
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_product_category
        FOREIGN KEY (CategoryId)
        REFERENCES Category(CategoryId)
        ON DELETE RESTRICT
);

CREATE TABLE ProductDetails (
    DetailsId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ProductId INT NOT NULL UNIQUE,
    TotalQuantity INT NOT NULL DEFAULT 0,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    Weight DECIMAL(10,2),
    Image VARCHAR(255),
    Price DECIMAL(10,2) NOT NULL,
    DepositRate DECIMAL(10,2) DEFAULT 0,

    CONSTRAINT fk_productdetails_product
        FOREIGN KEY (ProductId)
        REFERENCES Product(ProductId)
        ON DELETE CASCADE
);

CREATE TABLE InventoryItem (
    InventoryId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ProductId INT NOT NULL,
    SerialNumber VARCHAR(255) NOT NULL UNIQUE,
    Status inventory_status NOT NULL DEFAULT 'AVAILABLE',
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ExpiryDate TIMESTAMPTZ,

    CONSTRAINT fk_inventory_product
        FOREIGN KEY (ProductId)
        REFERENCES Product(ProductId)
        ON DELETE CASCADE
);

CREATE TABLE Alert (
    AlertId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ProductId INT NOT NULL,
    Status alert_status NOT NULL DEFAULT 'OPEN',
    MinThreshold INT NOT NULL,
    CurrentStock INT NOT NULL,
    Message VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_alert_product
        FOREIGN KEY (ProductId)
        REFERENCES Product(ProductId)
        ON DELETE CASCADE
);

CREATE TABLE ClearanceBatch (
    ClearanceBatchId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    BatchName VARCHAR(255) NOT NULL,
    CreatedDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ClearanceDate TIMESTAMPTZ,
    Status clearance_batch_status NOT NULL DEFAULT 'SCHEDULED'
);

CREATE TABLE ClearanceItem (
    ClearanceItemId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ClearanceBatchId INT NOT NULL,
    InventoryItemId INT NOT NULL UNIQUE,
    FinalPrice DECIMAL(10,2),
    RecommendedPrice DECIMAL(10,2),
    SaleDate TIMESTAMPTZ,
    Status clearance_status NOT NULL DEFAULT 'CLEARANCE',

    CONSTRAINT fk_clearance_batch
        FOREIGN KEY (ClearanceBatchId)
        REFERENCES ClearanceBatch(ClearanceBatchId)
        ON DELETE CASCADE,

    CONSTRAINT fk_clearance_inventory
        FOREIGN KEY (InventoryItemId)
        REFERENCES InventoryItem(InventoryId)
        ON DELETE CASCADE
);

--TEAM 4 PRIMARY KEY TABLES
CREATE TABLE IF NOT EXISTS "User" (
    userId       INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    userRole   user_role_enum NOT NULL,
    name         VARCHAR(100) NOT NULL,
    email        VARCHAR(100) NOT NULL UNIQUE,
    passwordHash VARCHAR(255) NOT NULL,
    phoneCountry INT,
    phoneNumber  VARCHAR(20)
  );

CREATE TABLE IF NOT EXISTS Customer (
    customerId   INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    userId       INT          NOT NULL UNIQUE, --customer is also a user, so userId is unique and not null
    address      VARCHAR(255) NOT NULL,
    customerType INT          NOT NULL,
    CONSTRAINT fk_customer_user FOREIGN KEY (userId) REFERENCES "User"(userId) ON UPDATE CASCADE ON DELETE CASCADE
  );

CREATE TABLE IF NOT EXISTS Staff (
    staffId    INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    userId     INT         NOT NULL UNIQUE, --staff is also a user, so userId is unique and not null
    department VARCHAR(50) NOT NULL,
    CONSTRAINT fk_staff_user FOREIGN KEY (userId) REFERENCES "User"(userId) ON UPDATE CASCADE ON DELETE CASCADE
  );

CREATE TABLE IF NOT EXISTS Notification (
    notificationId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    userId         INT          NOT NULL,
    message        VARCHAR(255) NOT NULL,
    dateSent       TIMESTAMPTZ    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    isRead         BOOLEAN      NOT NULL DEFAULT FALSE,
    type           notification_type_enum NOT NULL,
    CONSTRAINT fk_notification_user FOREIGN KEY (userId) REFERENCES "User"(userId) ON UPDATE CASCADE ON DELETE CASCADE
  );

CREATE TABLE IF NOT EXISTS NotificationPreference (
    preferenceId  INT     GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    userId        INT     NOT NULL,
    emailEnabled  BOOLEAN NOT NULL DEFAULT TRUE,
    smsEnabled    BOOLEAN NOT NULL DEFAULT FALSE,
    frequency     notification_frequency_enum    NOT NULL,
    granularity   notification_granularity_enum  NOT NULL,
    CONSTRAINT fk_notificationpref_user FOREIGN KEY (userId) REFERENCES "User"(userId) ON UPDATE CASCADE ON DELETE CASCADE
  );

-- TEAM 5 TABLES

-- 001_building_footprint
CREATE TABLE IF NOT EXISTS BuildingFootprint (
    buildingCarbonFootprintID INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    timeHourly TIMESTAMPTZ NOT NULL,
    zone VARCHAR(50),
    block VARCHAR(50),
    floor VARCHAR(50),
    room VARCHAR(50),
    totalRoomCo2 DOUBLE PRECISION NOT NULL
);

-- 002_EcoBadge
CREATE TABLE IF NOT EXISTS EcoBadge (
    badgeId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    maxCarbonG DOUBLE PRECISION NOT NULL,
    criteriaDescription VARCHAR(255),
    badgeName VARCHAR(100) NOT NULL
);

-- 008_PackagingConfiguration
-- NOTE: FK to PackagingProfile is deferred via ALTER TABLE below
--       because PackagingProfile depends on "Order" (Team 6) and is defined later.
CREATE TABLE IF NOT EXISTS PackagingConfiguration (
    configurationId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    profileId       INT NOT NULL
);

-- 009_PackagingMaterial
CREATE TABLE IF NOT EXISTS PackagingMaterial (
    materialId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    type VARCHAR(50),
    recyclable BOOLEAN NOT NULL DEFAULT FALSE,
    reusable BOOLEAN NOT NULL DEFAULT FALSE
);

-- 010_PackagingConfigMaterials
CREATE TABLE IF NOT EXISTS PackagingConfigMaterials (
    configurationId INT NOT NULL,
    materialId INT NOT NULL,
    category VARCHAR(50),
    quantity INT NOT NULL,

    PRIMARY KEY (configurationId, materialId),

    CONSTRAINT fk_pcm_configuration
        FOREIGN KEY (configurationId)
        REFERENCES PackagingConfiguration(configurationId)
        ON DELETE CASCADE,

    CONSTRAINT fk_pcm_material
        FOREIGN KEY (materialId)
        REFERENCES PackagingMaterial(materialId)
        ON DELETE CASCADE
);

--TEAM 6 PRIMARY KEY TABLES

-- ENUM TYPES
CREATE TYPE cart_status_enum AS ENUM ('ACTIVE','CHECKED_OUT','EXPIRED');

CREATE TYPE checkout_status_enum AS ENUM ('IN_PROGRESS','CONFIRMED','CANCELLED');

CREATE TYPE order_status_enum AS ENUM (
    'PENDING',
    'CONFIRMED',
    'PROCESSING',
    'READY_FOR_DISPATCH',
    'DISPATCHED',
    'DELIVERED',
    'CANCELLED'
);

CREATE TYPE delivery_duration_enum AS ENUM ('NextDay','ThreeDays','OneWeek');

CREATE TYPE transaction_type_enum AS ENUM ('PAYMENT','REFUND');

CREATE TYPE transaction_purpose_enum AS ENUM ('ORDER','PENALTY','REFUND_DEPOSIT');

CREATE TYPE transaction_status_enum AS ENUM ('PENDING','COMPLETED','FAILED','CANCELLED');

CREATE TYPE payment_method_enum AS ENUM ('CREDIT_CARD');

CREATE TYPE payment_purpose_enum AS ENUM ('RENTAL_FEE_DEPOSIT','PENALTY_FEE');
-- End ENUM TYPES

-- SESSION
CREATE TABLE IF NOT EXISTS Session (
    sessionId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    userId    INT         NOT NULL,
    role      VARCHAR(50) NOT NULL,
    createdAt TIMESTAMPTZ   NOT NULL,
    expiresAt TIMESTAMPTZ   NOT NULL,

    CONSTRAINT fk_session_user FOREIGN KEY (userId) REFERENCES "User"(userId) ON DELETE CASCADE
);

-- CART
CREATE TABLE IF NOT EXISTS Cart (
    cartId      INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    customerId  INT NULL,
    sessionId   INT NULL,
    rentalStart TIMESTAMPTZ,
    rentalEnd   TIMESTAMPTZ,
    status      cart_status_enum DEFAULT 'ACTIVE',

    CONSTRAINT fk_cart_customer
        FOREIGN KEY (customerId)
        REFERENCES Customer(customerId)
        ON DELETE SET NULL,

    CONSTRAINT fk_cart_session
        FOREIGN KEY (sessionId)
        REFERENCES Session(sessionId)
        ON DELETE SET NULL
);

-- CART ITEM
CREATE TABLE IF NOT EXISTS CartItem (
    cartItemId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    cartId     INT     NOT NULL,
    productId  INT     NOT NULL,
    quantity   INT     NOT NULL,
    isSelected BOOLEAN DEFAULT TRUE,

    CONSTRAINT fk_cartitem_cart
        FOREIGN KEY (cartId)
        REFERENCES Cart(cartId)
        ON DELETE CASCADE,

    CONSTRAINT fk_cartitem_product
        FOREIGN KEY (productId)
        REFERENCES Product(ProductId)
        ON DELETE RESTRICT
);

-- CHECKOUT
CREATE TABLE IF NOT EXISTS Checkout (
    checkoutId        INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    customerId        INT          NOT NULL,
    cartId            INT          NOT NULL,
    option_id         INT,
    paymentMethodType payment_method_enum,
    status            checkout_status_enum DEFAULT 'IN_PROGRESS',
    notifyOptIn       BOOLEAN DEFAULT FALSE,
    createdAt         TIMESTAMPTZ NOT NULL,

    CONSTRAINT fk_checkout_customer
        FOREIGN KEY (customerId)
        REFERENCES Customer(customerId)
        ON DELETE RESTRICT,

    CONSTRAINT fk_checkout_cart
        FOREIGN KEY (cartId)
        REFERENCES Cart(cartId)
        ON DELETE CASCADE
);

-- TRANSACTION
CREATE TABLE IF NOT EXISTS Transaction (
    transactionId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    amount DECIMAL(10,2) NOT NULL,
    type transaction_type_enum NOT NULL,
    purpose transaction_purpose_enum NOT NULL,
    status transaction_status_enum DEFAULT 'PENDING',
    providerTransactionId VARCHAR(100),
    createdAt TIMESTAMPTZ NOT NULL
);

-- ORDER
CREATE TABLE IF NOT EXISTS "Order" (
    orderId       INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    customerId    INT             NOT NULL,
    checkoutId    INT             NOT NULL,
    transactionId INT,                                         
    orderDate     TIMESTAMPTZ       NOT NULL,
    status        order_status_enum DEFAULT 'PENDING',
    deliveryType  delivery_duration_enum,
    totalAmount   DECIMAL(10,2)   NOT NULL,

    CONSTRAINT fk_order_customer
        FOREIGN KEY (customerId)
        REFERENCES Customer(customerId)
        ON DELETE RESTRICT,

    CONSTRAINT fk_order_checkout
        FOREIGN KEY (checkoutId)
        REFERENCES Checkout(checkoutId)
        ON DELETE CASCADE,

    CONSTRAINT fk_order_transaction
        FOREIGN KEY (transactionId)
        REFERENCES Transaction(transactionId)
        ON DELETE SET NULL
);

-- ORDER ITEM
CREATE TABLE IF NOT EXISTS OrderItem (
    orderItemId     INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    orderId         INT           NOT NULL,
    productId       INT           NOT NULL,
    quantity        INT           NOT NULL,
    unitPrice       DECIMAL(10,2) NOT NULL,
    rentalStartDate TIMESTAMPTZ,
    rentalEndDate   TIMESTAMPTZ,

    CONSTRAINT fk_orderitem_order
        FOREIGN KEY (orderId)
        REFERENCES "Order"(orderId)
        ON DELETE CASCADE,

    CONSTRAINT fk_orderitem_product
        FOREIGN KEY (productId)
        REFERENCES Product(ProductId)
        ON DELETE RESTRICT
);

-- PAYMENT
CREATE TABLE IF NOT EXISTS Payment (
    paymentId VARCHAR(50) PRIMARY KEY,
    orderId INT NOT NULL,
    transactionId INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    purpose payment_purpose_enum,
    status transaction_status_enum DEFAULT 'PENDING',
    createdAt TIMESTAMPTZ NOT NULL,

    CONSTRAINT fk_payment_order
        FOREIGN KEY (orderId)
        REFERENCES "Order"(orderId)
        ON DELETE CASCADE,

    CONSTRAINT fk_payment_transaction
        FOREIGN KEY (transactionId)
        REFERENCES Transaction(transactionId)
        ON DELETE CASCADE
);

-- DEPOSIT
CREATE TABLE IF NOT EXISTS Deposit (
    depositId VARCHAR(50) PRIMARY KEY,
    orderId INT NOT NULL,
    transactionId INT NOT NULL,
    originalAmount DECIMAL(10,2) NOT NULL,
    heldAmount DECIMAL(10,2) NOT NULL,
    refundedAmount DECIMAL(10,2) DEFAULT 0,
    forfeitedAmount DECIMAL(10,2) DEFAULT 0,
    createdAt TIMESTAMPTZ NOT NULL,

    CONSTRAINT fk_deposit_order
        FOREIGN KEY (orderId)
        REFERENCES "Order"(orderId)
        ON DELETE CASCADE,

    CONSTRAINT fk_deposit_transaction
        FOREIGN KEY (transactionId)
        REFERENCES Transaction(transactionId)
        ON DELETE CASCADE
);

--TEAM 1 CROSS TEAM FK TABLES

--TEAM 2 CROSS TEAM FK TABLES
CREATE TABLE IF NOT EXISTS LineItem (
    LineItemId      INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    RequestId       INT,
    ProductId       INT,
    QuantityRequest INT,
    ReasonCode      reason_code_enum,
    Remarks         TEXT,
    CONSTRAINT fk_lineitem_request FOREIGN KEY (RequestId) REFERENCES ReplenishmentRequest(RequestId) ON DELETE CASCADE,
    CONSTRAINT fk_lineitem_product FOREIGN KEY (ProductId) REFERENCES Product(ProductID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS RentalOrderLog (
    RentalOrderLogId INT PRIMARY KEY,
    OrderId       INT,
    CustomerId    INT,
    OrderDate     TIMESTAMPTZ,
    TotalAmount   DECIMAL(10,2),
    DeliveryType  delivery_type_enum,
    Status        rental_status_enum,
    DetailsJSON   TEXT,
    CONSTRAINT fk_rental_transaction FOREIGN KEY (RentalOrderLogId) REFERENCES TransactionLog(TransactionLogID) ON DELETE CASCADE,
    CONSTRAINT fk_rental_order       FOREIGN KEY (OrderId)          REFERENCES "Order"(OrderId)                   ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS LoanLog (
    LoanLogId           INT PRIMARY KEY,
    LoanListId          INT NOT NULL,
    RentalOrderLogId    INT NOT NULL,
    Status              loan_status_enum,
    LoanDate            TIMESTAMPTZ,
    ReturnDate          TIMESTAMPTZ,
    DueDate             TIMESTAMPTZ,
    DetailsJSON         TEXT,
    CONSTRAINT fk_loan_transaction FOREIGN KEY (LoanLogId)        REFERENCES TransactionLog(TransactionLogID) ON DELETE CASCADE,
    CONSTRAINT fk_loan_rental      FOREIGN KEY (RentalOrderLogId) REFERENCES RentalOrderLog(RentalOrderLogId) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ReturnLog (
    ReturnLogId         INT PRIMARY KEY,
    ReturnRequestId     INT NOT NULL,
    RentalOrderLogId    INT NOT NULL,
    CustomerId          VARCHAR(50),
    Status              return_status_enum,
    RequestDate         TIMESTAMPTZ,
    CompletionDate      TIMESTAMPTZ,
    ImageURL            VARCHAR(500),
    DetailsJSON         TEXT,
    CONSTRAINT fk_return_transaction FOREIGN KEY (ReturnLogId)      REFERENCES TransactionLog(TransactionLogID) ON DELETE CASCADE,
    CONSTRAINT fk_return_rental      FOREIGN KEY (RentalOrderLogId) REFERENCES RentalOrderLog(RentalOrderLogId) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ClearanceLog (
    ClearanceLogId INT PRIMARY KEY,
    ClearanceBatchId INT NOT NULL,
    BatchName VARCHAR(255),
    ClearanceDate TIMESTAMPTZ,
    Status clearance_status_enum,
    DetailsJSON TEXT,
    CONSTRAINT fk_clearance_transaction FOREIGN KEY (ClearanceLogId)   REFERENCES TransactionLog(TransactionLogID) ON DELETE CASCADE
);

--TEAM 3 CROSS TEAM FK TABLES
CREATE TYPE loan_status AS ENUM ('OPEN', 'ON_LOAN', 'RETURNED');

CREATE TYPE return_request_status AS ENUM ('PROCESSING', 'COMPLETED');

CREATE TYPE return_item_status AS ENUM (
    'DAMAGE_INSPECTION',
    'REPAIRING',
    'SERVICING',
    'CLEANING',
    'RETURN_TO_INVENTORY'
);

CREATE TABLE LoanList (
    LoanListId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    OrderId INT NOT NULL,
    CustomerId INT NOT NULL,
    LoanDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DueDate TIMESTAMPTZ NOT NULL,
    ReturnDate TIMESTAMPTZ,
    Status loan_status NOT NULL DEFAULT 'OPEN',
    Remarks TEXT,

    CONSTRAINT fk_loan_order
        FOREIGN KEY (OrderId)
        REFERENCES "Order"(OrderId)
        ON DELETE RESTRICT,

    CONSTRAINT fk_loan_customer
        FOREIGN KEY (CustomerId)
        REFERENCES Customer(CustomerId)
        ON DELETE RESTRICT
);

CREATE TABLE LoanItem (
    LoanItemId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    LoanListId INT NOT NULL,
    InventoryItemId INT NOT NULL,
    Remarks TEXT,

    CONSTRAINT fk_loanitem_loan
        FOREIGN KEY (LoanListId)
        REFERENCES LoanList(LoanListId)
        ON DELETE CASCADE,

    CONSTRAINT fk_loanitem_inventory
        FOREIGN KEY (InventoryItemId)
        REFERENCES InventoryItem(InventoryId)
        ON DELETE RESTRICT
);

CREATE TABLE ReturnRequest (
    ReturnRequestId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    OrderId INT NOT NULL,
    CustomerId INT NOT NULL,
    Status return_request_status NOT NULL DEFAULT 'PROCESSING',
    RequestDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CompletionDate TIMESTAMPTZ,

    CONSTRAINT fk_returnrequest_order
        FOREIGN KEY (OrderId)
        REFERENCES "Order"(OrderId)
        ON DELETE RESTRICT,

    CONSTRAINT fk_returnrequest_customer
        FOREIGN KEY (CustomerId)
        REFERENCES Customer(CustomerId)
        ON DELETE RESTRICT
);

CREATE TABLE ReturnItem (
    ReturnItemId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ReturnRequestId INT NOT NULL,
    InventoryItemId INT NOT NULL,
    Status return_item_status NOT NULL DEFAULT 'DAMAGE_INSPECTION',
    CompletionDate TIMESTAMPTZ,
    Image VARCHAR(255),

    CONSTRAINT fk_returnitem_request
        FOREIGN KEY (ReturnRequestId)
        REFERENCES ReturnRequest(ReturnRequestId)
        ON DELETE CASCADE,

    CONSTRAINT fk_returnitem_inventory
        FOREIGN KEY (InventoryItemId)
        REFERENCES InventoryItem(InventoryId)
        ON DELETE RESTRICT
);

CREATE TABLE DamageReport (
    DamageReportId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ReturnItemId INT NOT NULL,
    Description TEXT,
    Severity VARCHAR(255),
    RepairCost DECIMAL(10,2),
    Images VARCHAR(255),
    ReportDate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_damagereport_returnitem
        FOREIGN KEY (ReturnItemId)
        REFERENCES ReturnItem(ReturnItemId)
        ON DELETE CASCADE
);

--TEAM 4 CROSS TEAM FK TABLES
CREATE TABLE IF NOT EXISTS OrderStatusHistory (
    historyId  INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    orderId    INT          NOT NULL,
    status     order_history_status_enum NOT NULL,  -- aligned with order_status_enum values
    timestamp  TIMESTAMPTZ    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updatedBy  VARCHAR(50)  NOT NULL,
    remark     VARCHAR(255),
    CONSTRAINT fk_order_status_history_order
        FOREIGN KEY (orderId) REFERENCES "Order"(orderId) ON UPDATE CASCADE ON DELETE CASCADE
  );

CREATE TABLE IF NOT EXISTS Refund (
    refundId            INT           GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    orderId             INT           NOT NULL,
    customerId          INT           NOT NULL,
    transactionId       INT, 
    ReturnRequestId     INT           NOT NULL,
    depositRefundAmount DECIMAL(10,2) NOT NULL,
    returnDate          TIMESTAMPTZ     NOT NULL,
    penaltyAmount       DECIMAL(10,2) DEFAULT 0.00,
    returnMethod        VARCHAR(50)   NOT NULL,
    CONSTRAINT fk_refund_transaction FOREIGN KEY (transactionId)   REFERENCES Transaction(transactionId)             ON DELETE SET NULL,
    CONSTRAINT fk_refund_order    FOREIGN KEY (orderId)    REFERENCES "Order"(orderId)       ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_refund_customer FOREIGN KEY (customerId) REFERENCES Customer(customerId)   ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_refund_return   FOREIGN KEY (ReturnRequestId) REFERENCES ReturnRequest(ReturnRequestId) ON UPDATE CASCADE ON DELETE RESTRICT
  );

-- NOTE: batchId references delivery_batch (Team 1). Column renamed to delivery_batch_id
--       to match the PK name. Confirm with Team 1 if a separate Batches table is intended.
CREATE TABLE IF NOT EXISTS Shipment (
    trackingId   INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    orderId      INT              NOT NULL,
    batchId      INT              NOT NULL,
    status       shipment_status_enum NOT NULL,
    weight       DOUBLE PRECISION NOT NULL,
    destination  VARCHAR(255)     NOT NULL,
    CONSTRAINT fk_shipment_order FOREIGN KEY (orderId) REFERENCES "Order"(orderId)                     ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_shipment_batch FOREIGN KEY (batchId) REFERENCES delivery_batch(delivery_batch_id)    ON UPDATE CASCADE ON DELETE RESTRICT
  );

--TEAM 5 CROSS TEAM FK TABLES

-- 003_ProductFootprint
CREATE TABLE IF NOT EXISTS ProductFootprint (
    productCarbonFootprintID INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    productID INT NOT NULL,
    badgeId INT NOT NULL,
    productToxicPercentage DOUBLE PRECISION,
    totalCo2 DOUBLE PRECISION NOT NULL,
    calculatedAt TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT fk_productfootprint_badge
        FOREIGN KEY (badgeId)
        REFERENCES EcoBadge(badgeId)
        ON DELETE CASCADE,
    
    CONSTRAINT fk_productfootprint_product
        FOREIGN KEY (productID)
        REFERENCES Product(productId)
        ON DELETE CASCADE
);

-- Enum type for StaffAccessLog.eventType
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'access_event_type') THEN
    CREATE TYPE access_event_type AS ENUM ('IN','OUT');
    END IF;
END$$;

-- 004_StaffAccessLog
CREATE TABLE IF NOT EXISTS StaffAccessLog (
    accessId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    staffId INT NOT NULL,
    eventTime TIMESTAMPTZ NOT NULL DEFAULT now(),
    eventType access_event_type NOT NULL,

    CONSTRAINT fk_staffaccesslog_staff
        FOREIGN KEY (staffId)
        REFERENCES Staff(staffId)
        ON DELETE CASCADE
);

-- 005_StaffFootprint
CREATE TABLE IF NOT EXISTS StaffFootprint (
    staffCarbonFootprintID INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    staffId                INT              NOT NULL,
    time                   TIMESTAMPTZ      NOT NULL DEFAULT now(),
    hoursWorked            DOUBLE PRECISION NOT NULL,
    totalStaffCo2          DOUBLE PRECISION NOT NULL,

    CONSTRAINT fk_stafffootprint_staff  -- was incorrectly 'fk_staffaccesslog_staff' (copy-paste error)
        FOREIGN KEY (staffId)
        REFERENCES Staff(staffId)
        ON DELETE CASCADE
);

-- 006_OrderCarbondata
CREATE TABLE IF NOT EXISTS ordercarbondata (
    ordercarbondataid INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    orderid INT NOT NULL,
    productcarbon DOUBLE PRECISION NOT NULL,
    packagingcarbon DOUBLE PRECISION NOT NULL,
    staffcarbon DOUBLE PRECISION NOT NULL,
    buildingcarbon DOUBLE PRECISION NOT NULL,
    totalcarbon DOUBLE PRECISION NOT NULL,
    impactlevel VARCHAR(20),
    calculatedat TIMESTAMPTZ NOT NULL,

    CONSTRAINT fk_ordercarbondata_order
        FOREIGN KEY (orderid)
        REFERENCES "Order"(orderid)
        ON DELETE CASCADE
);

-- 007_CustomerRewards
CREATE TABLE IF NOT EXISTS customerrewards (
    rewardid INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    customerid INT NOT NULL,
    ordercarbondataid INT NOT NULL,
    rewardtype VARCHAR(50) NOT NULL,
    rewardvalue DOUBLE PRECISION NOT NULL,
    createdat TIMESTAMPTZ NOT NULL,

    CONSTRAINT fk_customerrewards_customer
        FOREIGN KEY (customerid)
        REFERENCES customer(customerid)
        ON DELETE CASCADE,

    CONSTRAINT fk_customerrewards_ordercarbondata
        FOREIGN KEY (ordercarbondataid)
        REFERENCES ordercarbondata(ordercarbondataid)
        ON DELETE CASCADE
);

-- 008_PackagingProfile
CREATE TABLE IF NOT EXISTS PackagingProfile (
    profileId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    orderId INT NOT NULL,
    volume DOUBLE PRECISION NOT NULL,
    fragilityLevel VARCHAR(50),

    CONSTRAINT fk_packagingprofile_order
        FOREIGN KEY (orderId)
        REFERENCES "Order"(orderId)
        ON DELETE CASCADE
);

-- Deferred FK: PackagingConfiguration was created before PackagingProfile.
ALTER TABLE PackagingConfiguration
    ADD CONSTRAINT fk_packagingconfiguration_profile
        FOREIGN KEY (profileId)
        REFERENCES PackagingProfile(profileId)
        ON DELETE CASCADE;


--TEAM 6 CROSS TEAM FK TABLES
-- The tables below were previously re-declared here, which is invalid in PostgreSQL.
-- "User", Customer, Session, Cart, "Order", and DeliveryMethod are already created above
-- with all required columns and constraints. No re-creation is necessary.
-- Any FKs that were missing from the primary definitions have been added inline above.
--
-- All cross-team FKs that were the actual intent of this section:
--   Session.userId       → "User"          (added in Team 6 SESSION definition above)
--   Cart.customerId      → Customer         (added in Team 6 CART definition above)
--   Checkout.customerId  → Customer         (added in Team 6 CHECKOUT definition above)
--   "Order".customerId   → Customer         (added in Team 6 ORDER definition above)
-- No further statements needed here.

-- ============================================================
-- DEFERRED CROSS-TEAM FKs
-- Applied after all tables across all teams are defined.
-- ============================================================

-- Team 2 → Team 3: LoanList references
ALTER TABLE LoanLog
    ADD CONSTRAINT fk_loan_list
		FOREIGN KEY (LoanListId) REFERENCES LoanList(LoanListId) ON DELETE CASCADE;
ALTER TABLE ReturnLog
	ADD CONSTRAINT fk_return_request
		FOREIGN KEY (ReturnRequestId) REFERENCES ReturnRequest(ReturnRequestId) ON DELETE CASCADE;
ALTER TABLE ClearanceLog
	ADD CONSTRAINT fk_clearance_batch
		FOREIGN KEY (ClearanceBatchId) REFERENCES ClearanceBatch(ClearanceBatchId) ON DELETE CASCADE;

-- Team 1 → Team 4/6: Customer and "Order" references
ALTER TABLE customer_choice
    ADD CONSTRAINT fk_customerchoice_customer
        FOREIGN KEY (customer_id) REFERENCES Customer(customerId) ON DELETE CASCADE;
ALTER TABLE customer_choice
    ADD CONSTRAINT fk_customerchoice_order
        FOREIGN KEY (order_id) REFERENCES "Order"(orderId) ON DELETE CASCADE;

-- Team 1 → Team 6: batch_order.order_id
ALTER TABLE batch_order
    ADD CONSTRAINT fk_batch_order_order
        FOREIGN KEY (order_id) REFERENCES "Order"(orderId) ON DELETE CASCADE;

-- Team 1 → Team 6 & Team 3: shipping_option and return_stage references
ALTER TABLE shipping_option
    ADD CONSTRAINT fk_shipping_option_order
        FOREIGN KEY (order_id) REFERENCES "Order"(orderId);
ALTER TABLE return_stage
    ADD CONSTRAINT fk_return_stage_return_request
        FOREIGN KEY (return_id) REFERENCES ReturnRequest(ReturnRequestId);

-- Team 6 → Team 4: Checkout.deliveryId
ALTER TABLE Checkout
ADD CONSTRAINT fk_checkout_delivery
    FOREIGN KEY (option_id) REFERENCES shipping_option(option_id) ON DELETE RESTRICT; 