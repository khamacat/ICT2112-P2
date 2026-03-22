using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using Route = ProRental.Domain.Entities.Route;

namespace ProRental.Data.UnitOfWork;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<Analytic> Analytics { get; set; }

    public virtual DbSet<BatchOrder> BatchOrders { get; set; }

    public virtual DbSet<Buildingfootprint> Buildingfootprints { get; set; }

    public virtual DbSet<CarbonEmission> CarbonEmissions { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Cartitem> Cartitems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Checkout> Checkouts { get; set; }

    public virtual DbSet<Clearancebatch> Clearancebatches { get; set; }

    public virtual DbSet<Clearanceitem> Clearanceitems { get; set; }

    public virtual DbSet<Clearancelog> Clearancelogs { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customerreward> Customerrewards { get; set; }

    public virtual DbSet<Damagereport> Damagereports { get; set; }

    public virtual DbSet<DeliveryBatch> DeliveryBatches { get; set; }

    public virtual DbSet<DeliveryRoute> DeliveryRoutes { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Ecobadge> Ecobadges { get; set; }

    public virtual DbSet<Inventoryitem> Inventoryitems { get; set; }

    public virtual DbSet<Lineitem> Lineitems { get; set; }

    public virtual DbSet<Loanitem> Loanitems { get; set; }

    public virtual DbSet<Loanlist> Loanlists { get; set; }

    public virtual DbSet<Loanlog> Loanlogs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Notificationpreference> Notificationpreferences { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Ordercarbondatum> Ordercarbondata { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Orderstatushistory> Orderstatushistories { get; set; }

    public virtual DbSet<Packagingconfigmaterial> Packagingconfigmaterials { get; set; }

    public virtual DbSet<Packagingconfiguration> Packagingconfigurations { get; set; }

    public virtual DbSet<Packagingmaterial> Packagingmaterials { get; set; }

    public virtual DbSet<Packagingprofile> Packagingprofiles { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Plane> Planes { get; set; }

    public virtual DbSet<Polineitem> Polineitems { get; set; }

    public virtual DbSet<PricingRule> PricingRules { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReturn> ProductReturns { get; set; }

    public virtual DbSet<Productdetail> Productdetails { get; set; }

    public virtual DbSet<Productfootprint> Productfootprints { get; set; }

    public virtual DbSet<Purchaseorder> Purchaseorders { get; set; }

    public virtual DbSet<Purchaseorderlog> Purchaseorderlogs { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Reliabilityrating> Reliabilityratings { get; set; }

    public virtual DbSet<Rentalorderlog> Rentalorderlogs { get; set; }

    public virtual DbSet<Replenishmentrequest> Replenishmentrequests { get; set; }

    public virtual DbSet<Reportexport> Reportexports { get; set; }

    public virtual DbSet<ReturnStage> ReturnStages { get; set; }

    public virtual DbSet<Returnitem> Returnitems { get; set; }

    public virtual DbSet<Returnlog> Returnlogs { get; set; }

    public virtual DbSet<Returnrequest> Returnrequests { get; set; }

    public virtual DbSet<RouteLeg> RouteLegs { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Ship> Ships { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<ShippingOption> ShippingOptions { get; set; }

    public virtual DbSet<ShippingPort> ShippingPorts { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Staffaccesslog> Staffaccesslogs { get; set; }

    public virtual DbSet<Stafffootprint> Stafffootprints { get; set; }

    public virtual DbSet<Stockitem> Stockitems { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Suppliercategorychangelog> Suppliercategorychangelogs { get; set; }

    public virtual DbSet<Train> Trains { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Transactionlog> Transactionlogs { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    public virtual DbSet<TransportationHub> TransportationHubs { get; set; }

    public virtual DbSet<Truck> Trucks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vettingrecord> Vettingrecords { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("access_event_type", new[] { "IN", "OUT" })
            .HasPostgresEnum("alert_status", new[] { "OPEN", "ACKNOWLEDGED", "RESOLVED" })
            .HasPostgresEnum("analytics_type_enum", new[] { "DAILY", "SUPTREND", "PRODTREND" })
            .HasPostgresEnum("batch_status", new[] { "PENDING", "SHIPPEDOUT" })
            .HasPostgresEnum("carbon_stage_type", new[] { "DAMAGE_INSPECTION", "REPAIRING", "SERVICING", "CLEANING", "RETURN" })
            .HasPostgresEnum("cart_status_enum", new[] { "ACTIVE", "CHECKED_OUT", "EXPIRED" })
            .HasPostgresEnum("checkout_status_enum", new[] { "IN_PROGRESS", "CONFIRMED", "CANCELLED" })
            .HasPostgresEnum("clearance_batch_status", new[] { "SCHEDULED", "ACTIVE", "CLOSED" })
            .HasPostgresEnum("clearance_status", new[] { "CLEARANCE", "SOLD" })
            .HasPostgresEnum("clearance_status_enum", new[] { "ONGOING", "COMPLETED", "CANCELLED" })
            .HasPostgresEnum("delivery_duration_enum", new[] { "NextDay", "ThreeDays", "OneWeek" })
            .HasPostgresEnum("delivery_type_enum", new[] { "STANDARD", "EXPRESS", "SELF_PICKUP" })
            .HasPostgresEnum("file_format_enum", new[] { "CSV", "XLSX", "PDF", "PNG" })
            .HasPostgresEnum("hub_type", new[] { "WAREHOUSE", "SHIPPING_PORT", "AIRPORT" })
            .HasPostgresEnum("inventory_status", new[] { "AVAILABLE", "RETIRED", "CLEARANCE", "SOLD", "MAINTENANCE", "RESERVED", "ON_LOAN", "BROKEN" })
            .HasPostgresEnum("loan_status", new[] { "OPEN", "ON_LOAN", "RETURNED" })
            .HasPostgresEnum("loan_status_enum", new[] { "ONGOING", "RETURNED", "OVERDUE", "CANCELLED" })
            .HasPostgresEnum("log_type_enum", new[] { "RENTAL_ORDER", "LOAN", "RETURN", "PURCHASE_ORDER", "CLEARANCE" })
            .HasPostgresEnum("notification_frequency_enum", new[] { "INSTANT", "DAILY", "WEEKLY" })
            .HasPostgresEnum("notification_granularity_enum", new[] { "ALL", "IMPORTANT_ONLY", "NONE" })
            .HasPostgresEnum("notification_type_enum", new[] { "ORDER_UPDATE", "PROMOTION", "SYSTEM", "PRODUCT" })
            .HasPostgresEnum("order_history_status_enum", new[] { "PENDING", "CONFIRMED", "PROCESSING", "READY_FOR_DISPATCH", "DISPATCHED", "DELIVERED", "CANCELLED" })
            .HasPostgresEnum("order_status_enum", new[] { "PENDING", "CONFIRMED", "PROCESSING", "READY_FOR_DISPATCH", "DISPATCHED", "DELIVERED", "CANCELLED" })
            .HasPostgresEnum("payment_method_enum", new[] { "CREDIT_CARD" })
            .HasPostgresEnum("payment_purpose_enum", new[] { "RENTAL_FEE_DEPOSIT", "PENALTY_FEE" })
            .HasPostgresEnum("po_status_enum", new[] { "COMPLETED", "CONFIRMED", "SUBMITTED", "APPROVED", "REJECTED", "CANCELLED" })
            .HasPostgresEnum("preference_type", new[] { "FAST", "CHEAP", "GREEN" })
            .HasPostgresEnum("product_status", new[] { "AVAILABLE", "UNAVAILABLE", "RETIRED" })
            .HasPostgresEnum("purchase_order_status_enum", new[] { "PENDING", "APPROVED", "REJECTED", "DELIVERED", "CANCELLED" })
            .HasPostgresEnum("rating_band_enum", new[] { "HIGH", "MEDIUM", "LOW", "UNRATED" })
            .HasPostgresEnum("reason_code_enum", new[] { "LOWSTOCK", "DEMANDSPIKE", "REPLACEMENT", "NEWITEM", "OTHERS" })
            .HasPostgresEnum("rental_status_enum", new[] { "PENDING", "CONFIRMED", "CANCELLED", "COMPLETED" })
            .HasPostgresEnum("replenishment_status_enum", new[] { "DRAFT", "SUBMITTED", "CANCELLED", "COMPLETED" })
            .HasPostgresEnum("return_item_status", new[] { "DAMAGE_INSPECTION", "REPAIRING", "SERVICING", "CLEANING", "RETURN_TO_INVENTORY" })
            .HasPostgresEnum("return_request_status", new[] { "PROCESSING", "COMPLETED" })
            .HasPostgresEnum("return_status_enum", new[] { "PENDING", "APPROVED", "REJECTED", "COMPLETED" })
            .HasPostgresEnum("shipment_status_enum", new[] { "PENDING", "IN_TRANSIT", "DELIVERED", "CANCELLED" })
            .HasPostgresEnum("stagetype", new[] { "INSPECTION", "REPAIRING", "SERVICING", "CLEANING", "INV_RETURN" })
            .HasPostgresEnum("supplier_category_enum", new[] { "LONGCREDITPERIOD", "QUICKTURNAROUNDTIME", "NEWUNTESTED" })
            .HasPostgresEnum("transaction_purpose_enum", new[] { "ORDER", "PENALTY", "REFUND_DEPOSIT" })
            .HasPostgresEnum("transaction_status_enum", new[] { "PENDING", "COMPLETED", "FAILED", "CANCELLED" })
            .HasPostgresEnum("transaction_type_enum", new[] { "PAYMENT", "REFUND" })
            .HasPostgresEnum("transport_mode", new[] { "TRUCK", "SHIP", "PLANE", "TRAIN" })
            .HasPostgresEnum("user_role_enum", new[] { "CUSTOMER", "STAFF", "ADMIN" })
            .HasPostgresEnum("vetting_decision_enum", new[] { "APPROVED", "REJECTED", "PENDING" })
            .HasPostgresEnum("vetting_result_enum", new[] { "APPROVED", "REJECTED", "PENDING" })
            .HasPostgresEnum("visual_type_enum", new[] { "TABLE", "BAR", "COLUMN", "LINE", "PIE", "AREA" });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey("HubId").HasName("airport_pkey");

            entity.ToTable("airport");

            entity.Property("HubId")
                .HasField("_hubId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("hub_id");
            entity.Property("AircraftSize")
                .HasField("_aircraftSize")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("aircraft_size");
            entity.Property("AirportCode")
                .HasField("_airportCode")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(10)
                .HasColumnName("airport_code");
            entity.Property("AirportName")
                .HasField("_airportName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("airport_name");
            entity.Property("Terminal")
                .HasField("_terminal")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("terminal");

            entity.HasOne(d => d.Hub).WithOne(p => p.Airport)
                .HasForeignKey<Airport>("HubId")
                .HasConstraintName("fk_airport_hub");
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey("Alertid").HasName("alert_pkey");

            entity.ToTable("alert");

            entity.Property("Alertid")
                .HasField("_alertid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("alertid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property("Currentstock")
                .HasField("_currentstock")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("currentstock");
            entity.Property("Message")
                .HasField("_message")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property("Minthreshold")
                .HasField("_minthreshold")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("minthreshold");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Updatedat")
                .HasField("_updatedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Product).WithMany(p => p.Alerts)
                .HasForeignKey("Productid")
                .HasConstraintName("fk_alert_product");
        });

        modelBuilder.Entity<Analytic>(entity =>
        {
            entity.HasKey("Analyticsid").HasName("analytics_pkey");

            entity.ToTable("analytics");

            entity.Property("Analyticsid")
                .HasField("_analyticsid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("analyticsid");
            entity.Property("Enddate")
                .HasField("_enddate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("enddate");
            entity.Property("Loanamt")
                .HasField("_loanamt")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("loanamt");
            entity.Property("Refprimaryid")
                .HasField("_refprimaryid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("refprimaryid");
            entity.Property("Refprimaryname")
                .HasField("_refprimaryname")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("refprimaryname");
            entity.Property("Refvalue")
                .HasField("_refvalue")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("refvalue");
            entity.Property("Returnamt")
                .HasField("_returnamt")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returnamt");
            entity.Property("Startdate")
                .HasField("_startdate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("startdate");

            entity.HasMany(d => d.Transactionlogs).WithMany(p => p.Analytics)
                .UsingEntity<Dictionary<string, object>>(
                    "Analyticslist",
                    r => r.HasOne<Transactionlog>().WithMany()
                        .HasForeignKey("Transactionlogid")
                        .HasConstraintName("fk_analyticslist_log"),
                    l => l.HasOne<Analytic>().WithMany()
                        .HasForeignKey("Analyticsid")
                        .HasConstraintName("fk_analyticslist_analytics"),
                    j =>
                    {
                        j.HasKey("Analyticsid", "Transactionlogid").HasName("analyticslist_pkey");
                        j.ToTable("analyticslist");
                        j.IndexerProperty<int>("Analyticsid").HasColumnName("analyticsid");
                        j.IndexerProperty<int>("Transactionlogid").HasColumnName("transactionlogid");
                    });
        });

        modelBuilder.Entity<BatchOrder>(entity =>
        {
            entity.HasKey("BatchId", "OrderId").HasName("batch_order_pkey");

            entity.ToTable("batch_order");

            // entity.HasIndex("OrderId", "batch_order_order_id_key").IsUnique();
            entity.HasIndex("OrderId").HasDatabaseName("batch_order_order_id_key").IsUnique();

            entity.Property("BatchId")
                .HasField("_batchId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("batch_id");
            entity.Property("OrderId")
                .HasField("_orderId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("order_id");
            entity.Property("AddedTimestamp")
                .HasField("_addedTimestamp")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("now()")
                .HasColumnName("added_timestamp");

            entity.HasOne(d => d.Batch).WithMany(p => p.BatchOrders)
                .HasForeignKey("BatchId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_batch_order_batch");

            entity.HasOne(d => d.Order).WithOne(p => p.BatchOrder)
                .HasForeignKey<BatchOrder>("OrderId")
                .HasConstraintName("fk_batch_order_order");
        });

        modelBuilder.Entity<Buildingfootprint>(entity =>
        {
            entity.HasKey("Buildingcarbonfootprintid").HasName("buildingfootprint_pkey");

            entity.ToTable("buildingfootprint");

            entity.Property("Buildingcarbonfootprintid")
                .HasField("_buildingcarbonfootprintid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("buildingcarbonfootprintid");
            entity.Property("Block")
                .HasField("_block")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("block");
            entity.Property("Floor")
                .HasField("_floor")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("floor");
            entity.Property("Room")
                .HasField("_room")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("room");
            entity.Property("Timehourly")
                .HasField("_timehourly")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("timehourly");
            entity.Property("Totalroomco2")
                .HasField("_totalroomco2")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("totalroomco2");
            entity.Property("Zone")
                .HasField("_zone")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("zone");
        });

        modelBuilder.Entity<CarbonEmission>(entity =>
        {
            entity.HasKey("EmissionId").HasName("carbon_emission_pkey");

            entity.ToTable("carbon_emission");

            entity.Property("EmissionId")
                .HasField("_emissionId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("emission_id");
            entity.Property("CarbonKg")
                .HasField("_carbonKg")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("carbon_kg");
            entity.Property("StageId")
                .HasField("_stageId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("stage_id");

            entity.HasOne(d => d.Stage).WithMany(p => p.CarbonEmissions)
                .HasForeignKey("StageId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_carbon_emission_stage");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey("Cartid").HasName("cart_pkey");

            entity.ToTable("cart");

            entity.Property("Cartid")
                .HasField("_cartid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("cartid");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Rentalend")
                .HasField("_rentalend")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rentalend");
            entity.Property("Rentalstart")
                .HasField("_rentalstart")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rentalstart");
            entity.Property("Sessionid")
                .HasField("_sessionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("sessionid");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey("Customerid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cart_customer");

            entity.HasOne(d => d.Session).WithMany(p => p.Carts)
                .HasForeignKey("Sessionid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cart_session");
        });

        modelBuilder.Entity<Cartitem>(entity =>
        {
            entity.HasKey("Cartitemid").HasName("cartitem_pkey");

            entity.ToTable("cartitem");

            entity.Property("Cartitemid")
                .HasField("_cartitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("cartitemid");
            entity.Property("Cartid")
                .HasField("_cartid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("cartid");
            entity.Property("Isselected")
                .HasField("_isselected")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(true)
                .HasColumnName("isselected");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Quantity")
                .HasField("_quantity")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.Cartitems)
                .HasForeignKey("Cartid")
                .HasConstraintName("fk_cartitem_cart");

            entity.HasOne(d => d.Product).WithMany(p => p.Cartitems)
                .HasForeignKey("Productid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_cartitem_product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey("Categoryid").HasName("category_pkey");

            entity.ToTable("category");

            entity.Property("Categoryid")
                .HasField("_categoryid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("categoryid");
            entity.Property("Createddate")
                .HasField("_createddate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createddate");
            entity.Property("Description")
                .HasField("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("description");
            entity.Property("Name")
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property("Updateddate")
                .HasField("_updateddate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updateddate");
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey("Checkoutid").HasName("checkout_pkey");

            entity.ToTable("checkout");

            entity.Property("Checkoutid")
                .HasField("_checkoutid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("checkoutid");
            entity.Property("Cartid")
                .HasField("_cartid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("cartid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("createdat");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Notifyoptin")
                .HasField("_notifyoptin")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(false)
                .HasColumnName("notifyoptin");
            entity.Property("OptionId")
                .HasField("_optionId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("option_id");

            entity.HasOne(d => d.Cart).WithMany(p => p.Checkouts)
                .HasForeignKey("Cartid")
                .HasConstraintName("fk_checkout_cart");

            entity.HasOne(d => d.Customer).WithMany(p => p.Checkouts)
                .HasForeignKey("Customerid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_checkout_customer");

            entity.HasOne(d => d.Option).WithMany(p => p.Checkouts)
                .HasForeignKey("OptionId")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_checkout_delivery");
        });

        modelBuilder.Entity<Clearancebatch>(entity =>
        {
            entity.HasKey("Clearancebatchid").HasName("clearancebatch_pkey");

            entity.ToTable("clearancebatch");

            entity.Property("Clearancebatchid")
                .HasField("_clearancebatchid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("clearancebatchid");
            entity.Property("Batchname")
                .HasField("_batchname")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("batchname");
            entity.Property("Clearancedate")
                .HasField("_clearancedate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("clearancedate");
            entity.Property("Createddate")
                .HasField("_createddate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createddate");
        });

        modelBuilder.Entity<Clearanceitem>(entity =>
        {
            entity.HasKey("Clearanceitemid").HasName("clearanceitem_pkey");

            entity.ToTable("clearanceitem");

            // entity.HasIndex("Inventoryitemid", "clearanceitem_inventoryitemid_key").IsUnique();
            entity.HasIndex("Inventoryitemid").HasDatabaseName("clearanceitem_inventoryitemid_key").IsUnique();

            entity.Property("Clearanceitemid")
                .HasField("_clearanceitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("clearanceitemid");
            entity.Property("Clearancebatchid")
                .HasField("_clearancebatchid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("clearancebatchid");
            entity.Property("Finalprice")
                .HasField("_finalprice")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("finalprice");
            entity.Property("Inventoryitemid")
                .HasField("_inventoryitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("inventoryitemid");
            entity.Property("Recommendedprice")
                .HasField("_recommendedprice")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("recommendedprice");
            entity.Property("Saledate")
                .HasField("_saledate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("saledate");

            entity.HasOne(d => d.Clearancebatch).WithMany(p => p.Clearanceitems)
                .HasForeignKey("Clearancebatchid")
                .HasConstraintName("fk_clearance_batch");

            entity.HasOne(d => d.Inventoryitem).WithOne(p => p.Clearanceitem)
                .HasForeignKey<Clearanceitem>("Inventoryitemid")
                .HasConstraintName("fk_clearance_inventory");
        });

        modelBuilder.Entity<Clearancelog>(entity =>
        {
            entity.HasKey("Clearancelogid").HasName("clearancelog_pkey");

            entity.ToTable("clearancelog");

            entity.Property("Clearancelogid")
                .HasField("_clearancelogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("clearancelogid");
            entity.Property("Batchname")
                .HasField("_batchname")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("batchname");
            entity.Property("Clearancebatchid")
                .HasField("_clearancebatchid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("clearancebatchid");
            entity.Property("Clearancedate")
                .HasField("_clearancedate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("clearancedate");
            entity.Property("Detailsjson")
                .HasField("_detailsjson")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("detailsjson");

            entity.HasOne(d => d.Clearancebatch).WithMany(p => p.Clearancelogs)
                .HasForeignKey("Clearancebatchid")
                .HasConstraintName("fk_clearance_batch");

            entity.HasOne(d => d.ClearancelogNavigation).WithOne(p => p.Clearancelog)
                .HasForeignKey<Clearancelog>("Clearancelogid")
                .HasConstraintName("fk_clearance_transaction");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey("Customerid").HasName("customer_pkey");

            entity.ToTable("customer");

            // entity.HasIndex("Userid", "customer_userid_key").IsUnique();
            entity.HasIndex("Userid").HasDatabaseName("customer_userid_key").IsUnique();

            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("customerid");
            entity.Property("Address")
                .HasField("_address")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property("Customertype")
                .HasField("_customertype")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customertype");
            entity.Property("Userid")
                .HasField("_userid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>("Userid")
                .HasConstraintName("fk_customer_user");
        });

        modelBuilder.Entity<Customerreward>(entity =>
        {
            entity.HasKey("Rewardid").HasName("customerrewards_pkey");

            entity.ToTable("customerrewards");

            entity.Property("Rewardid")
                .HasField("_rewardid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("rewardid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("createdat");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Ordercarbondataid")
                .HasField("_ordercarbondataid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("ordercarbondataid");
            entity.Property("Rewardtype")
                .HasField("_rewardtype")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("rewardtype");
            entity.Property("Rewardvalue")
                .HasField("_rewardvalue")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rewardvalue");

            entity.HasOne(d => d.Customer).WithMany(p => p.Customerrewards)
                .HasForeignKey("Customerid")
                .HasConstraintName("fk_customerrewards_customer");

            entity.HasOne(d => d.Ordercarbondata).WithMany(p => p.Customerrewards)
                .HasForeignKey("Ordercarbondataid")
                .HasConstraintName("fk_customerrewards_ordercarbondata");
        });

        modelBuilder.Entity<Damagereport>(entity =>
        {
            entity.HasKey("Damagereportid").HasName("damagereport_pkey");

            entity.ToTable("damagereport");

            entity.Property("Damagereportid")
                .HasField("_damagereportid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("damagereportid");
            entity.Property("Description")
                .HasField("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("description");
            entity.Property("Images")
                .HasField("_images")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("images");
            entity.Property("Repaircost")
                .HasField("_repaircost")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("repaircost");
            entity.Property("Reportdate")
                .HasField("_reportdate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("reportdate");
            entity.Property("Returnitemid")
                .HasField("_returnitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returnitemid");
            entity.Property("Severity")
                .HasField("_severity")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("severity");

            entity.HasOne(d => d.Returnitem).WithMany(p => p.Damagereports)
                .HasForeignKey("Returnitemid")
                .HasConstraintName("fk_damagereport_returnitem");
        });

        modelBuilder.Entity<DeliveryBatch>(entity =>
        {
            entity.HasKey("DeliveryBatchId").HasName("delivery_batch_pkey");

            entity.ToTable("delivery_batch");

            entity.Property("DeliveryBatchId")
                .HasField("_deliveryBatchId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("delivery_batch_id");
            entity.Property("BatchWeightKg")
                .HasField("_batchWeightKg")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("batch_weight_kg");
            entity.Property("CarbonSavings")
                .HasField("_carbonSavings")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("carbon_savings");
            entity.Property("DestinationAddress")
                .HasField("_destinationAddress")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("destination_address");
            entity.Property("HubId")
                .HasField("_hubId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("hub_id");
            entity.Property("TotalOrders")
                .HasField("_totalOrders")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(0)
                .HasColumnName("total_orders");

            entity.HasOne(d => d.Hub).WithMany(p => p.DeliveryBatches)
                .HasForeignKey("HubId")
                .HasConstraintName("fk_delivery_batch_hub");
        });

        modelBuilder.Entity<DeliveryRoute>(entity =>
        {
            entity.HasKey("RouteId").HasName("delivery_route_pkey");

            entity.ToTable("delivery_route");

            entity.Property("RouteId")
                .HasField("_routeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("route_id");
            entity.Property("DestinationAddress")
                .HasField("_destinationAddress")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("destination_address");
            entity.Property("DestinationHubId")
                .HasField("_destinationHubId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("destination_hub_id");
            entity.Property("IsValid")
                .HasField("_isValid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(true)
                .HasColumnName("is_valid");
            entity.Property("OriginAddress")
                .HasField("_originAddress")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("origin_address");
            entity.Property("OriginHubId")
                .HasField("_originHubId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("origin_hub_id");
            entity.Property("TotalDistanceKm")
                .HasField("_totalDistanceKm")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("total_distance_km");

            entity.HasOne(d => d.DestinationHub).WithMany(p => p.DeliveryRouteDestinationHubs)
                .HasForeignKey("DestinationHubId")
                .HasConstraintName("fk_route_destination_hub");

            entity.HasOne(d => d.OriginHub).WithMany(p => p.DeliveryRouteOriginHubs)
                .HasForeignKey("OriginHubId")
                .HasConstraintName("fk_route_origin_hub");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey("Depositid").HasName("deposit_pkey");

            entity.ToTable("deposit");

            entity.Property("Depositid")
                .HasField("_depositid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("depositid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("createdat");
            entity.Property("Forfeitedamount")
                .HasField("_forfeitedamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("forfeitedamount");
            entity.Property("Heldamount")
                .HasField("_heldamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("heldamount");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Originalamount")
                .HasField("_originalamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("originalamount");
            entity.Property("Refundedamount")
                .HasField("_refundedamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("refundedamount");
            entity.Property("Transactionid")
                .HasField("_transactionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("transactionid");

            entity.HasOne(d => d.Order).WithMany(p => p.Deposits)
                .HasForeignKey("Orderid")
                .HasConstraintName("fk_deposit_order");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Deposits)
                .HasForeignKey("Transactionid")
                .HasConstraintName("fk_deposit_transaction");
        });

        modelBuilder.Entity<Ecobadge>(entity =>
        {
            entity.HasKey("Badgeid").HasName("ecobadge_pkey");

            entity.ToTable("ecobadge");

            entity.Property("Badgeid")
                .HasField("_badgeid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("badgeid");
            entity.Property("Badgename")
                .HasField("_badgename")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(100)
                .HasColumnName("badgename");
            entity.Property("Criteriadescription")
                .HasField("_criteriadescription")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("criteriadescription");
            entity.Property("Maxcarbong")
                .HasField("_maxcarbong")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("maxcarbong");
        });

        modelBuilder.Entity<Inventoryitem>(entity =>
        {
            entity.HasKey("Inventoryid").HasName("inventoryitem_pkey");

            entity.ToTable("inventoryitem");

            // entity.HasIndex("Serialnumber", "inventoryitem_serialnumber_key").IsUnique();
            entity.HasIndex("Serialnumber").HasDatabaseName("inventoryitem_serialnumber_key").IsUnique();

            entity.Property("Inventoryid")
                .HasField("_inventoryid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("inventoryid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property("Expirydate")
                .HasField("_expirydate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("expirydate");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Serialnumber")
                .HasField("_serialnumber")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("serialnumber");
            entity.Property("Updatedat")
                .HasField("_updatedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventoryitems)
                .HasForeignKey("Productid")
                .HasConstraintName("fk_inventory_product");
        });

        modelBuilder.Entity<Lineitem>(entity =>
        {
            entity.HasKey("Lineitemid").HasName("lineitem_pkey");

            entity.ToTable("lineitem");

            entity.Property("Lineitemid")
                .HasField("_lineitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("lineitemid");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Quantityrequest")
                .HasField("_quantityrequest")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("quantityrequest");
            entity.Property("Remarks")
                .HasField("_remarks")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("remarks");
            entity.Property("Requestid")
                .HasField("_requestid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("requestid");

            entity.HasOne(d => d.Product).WithMany(p => p.Lineitems)
                .HasForeignKey("Productid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_lineitem_product");

            entity.HasOne(d => d.Request).WithMany(p => p.Lineitems)
                .HasForeignKey("Requestid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_lineitem_request");
        });

        modelBuilder.Entity<Loanitem>(entity =>
        {
            entity.HasKey("Loanitemid").HasName("loanitem_pkey");

            entity.ToTable("loanitem");

            entity.Property("Loanitemid")
                .HasField("_loanitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("loanitemid");
            entity.Property("Inventoryitemid")
                .HasField("_inventoryitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("inventoryitemid");
            entity.Property("Loanlistid")
                .HasField("_loanlistid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("loanlistid");
            entity.Property("Remarks")
                .HasField("_remarks")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("remarks");

            entity.HasOne(d => d.Inventoryitem).WithMany(p => p.Loanitems)
                .HasForeignKey("Inventoryitemid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_loanitem_inventory");

            entity.HasOne(d => d.Loanlist).WithMany(p => p.Loanitems)
                .HasForeignKey("Loanlistid")
                .HasConstraintName("fk_loanitem_loan");
        });

        modelBuilder.Entity<Loanlist>(entity =>
        {
            entity.HasKey("Loanlistid").HasName("loanlist_pkey");

            entity.ToTable("loanlist");

            entity.Property("Loanlistid")
                .HasField("_loanlistid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("loanlistid");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Duedate")
                .HasField("_duedate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("duedate");
            entity.Property("Loandate")
                .HasField("_loandate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("loandate");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Remarks")
                .HasField("_remarks")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("remarks");
            entity.Property("Returndate")
                .HasField("_returndate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returndate");

            entity.HasOne(d => d.Customer).WithMany(p => p.Loanlists)
                .HasForeignKey("Customerid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_loan_customer");

            entity.HasOne(d => d.Order).WithMany(p => p.Loanlists)
                .HasForeignKey("Orderid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_loan_order");
        });

        modelBuilder.Entity<Loanlog>(entity =>
        {
            entity.HasKey("Loanlogid").HasName("loanlog_pkey");

            entity.ToTable("loanlog");

            entity.Property("Loanlogid")
                .HasField("_loanlogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("loanlogid");
            entity.Property("Detailsjson")
                .HasField("_detailsjson")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("detailsjson");
            entity.Property("Duedate")
                .HasField("_duedate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("duedate");
            entity.Property("Loandate")
                .HasField("_loandate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("loandate");
            entity.Property("Loanlistid")
                .HasField("_loanlistid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("loanlistid");
            entity.Property("Rentalorderlogid")
                .HasField("_rentalorderlogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rentalorderlogid");
            entity.Property("Returndate")
                .HasField("_returndate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returndate");

            entity.HasOne(d => d.Loanlist).WithMany(p => p.Loanlogs)
                .HasForeignKey("Loanlistid")
                .HasConstraintName("fk_loan_list");

            entity.HasOne(d => d.LoanlogNavigation).WithOne(p => p.Loanlog)
                .HasForeignKey<Loanlog>("Loanlogid")
                .HasConstraintName("fk_loan_transaction");

            entity.HasOne(d => d.Rentalorderlog).WithMany(p => p.Loanlogs)
                .HasForeignKey("Rentalorderlogid")
                .HasConstraintName("fk_loan_rental");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey("Notificationid").HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property("Notificationid")
                .HasField("_notificationid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("notificationid");
            entity.Property("Datesent")
                .HasField("_datesent")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("datesent");
            entity.Property("Isread")
                .HasField("_isread")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(false)
                .HasColumnName("isread");
            entity.Property("Message")
                .HasField("_message")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property("Userid")
                .HasField("_userid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey("Userid")
                .HasConstraintName("fk_notification_user");
        });

        modelBuilder.Entity<Notificationpreference>(entity =>
        {
            entity.HasKey("Preferenceid").HasName("notificationpreference_pkey");

            entity.ToTable("notificationpreference");

            entity.Property("Preferenceid")
                .HasField("_preferenceid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("preferenceid");
            entity.Property("Emailenabled")
                .HasField("_emailenabled")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(true)
                .HasColumnName("emailenabled");
            entity.Property("Smsenabled")
                .HasField("_smsenabled")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(false)
                .HasColumnName("smsenabled");
            entity.Property("Userid")
                .HasField("_userid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Notificationpreferences)
                .HasForeignKey("Userid")
                .HasConstraintName("fk_notificationpref_user");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey("Orderid").HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orderid");
            entity.Property("Checkoutid")
                .HasField("_checkoutid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("checkoutid");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Orderdate")
                .HasField("_orderdate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderdate");
            entity.Property("Totalamount")
                .HasField("_totalamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");
            entity.Property("Transactionid")
                .HasField("_transactionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("transactionid");

            entity.HasOne(d => d.Checkout).WithMany(p => p.Orders)
                .HasForeignKey("Checkoutid")
                .HasConstraintName("fk_order_checkout");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey("Customerid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_order_customer");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Orders)
                .HasForeignKey("Transactionid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_order_transaction");
        });

        modelBuilder.Entity<Ordercarbondatum>(entity =>
        {
            entity.HasKey("Ordercarbondataid").HasName("ordercarbondata_pkey");

            entity.ToTable("ordercarbondata");

            entity.Property("Ordercarbondataid")
                .HasField("_ordercarbondataid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ordercarbondataid");
            entity.Property("Buildingcarbon")
                .HasField("_buildingcarbon")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("buildingcarbon");
            entity.Property("Calculatedat")
                .HasField("_calculatedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("calculatedat");
            entity.Property("Impactlevel")
                .HasField("_impactlevel")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(20)
                .HasColumnName("impactlevel");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Packagingcarbon")
                .HasField("_packagingcarbon")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("packagingcarbon");
            entity.Property("Productcarbon")
                .HasField("_productcarbon")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productcarbon");
            entity.Property("Staffcarbon")
                .HasField("_staffcarbon")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("staffcarbon");
            entity.Property("Totalcarbon")
                .HasField("_totalcarbon")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("totalcarbon");

            entity.HasOne(d => d.Order).WithMany(p => p.Ordercarbondata)
                .HasForeignKey("Orderid")
                .HasConstraintName("fk_ordercarbondata_order");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey("Orderitemid").HasName("orderitem_pkey");

            entity.ToTable("orderitem");

            entity.Property("Orderitemid")
                .HasField("_orderitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orderitemid");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Quantity")
                .HasField("_quantity")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("quantity");
            entity.Property("Rentalenddate")
                .HasField("_rentalenddate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rentalenddate");
            entity.Property("Rentalstartdate")
                .HasField("_rentalstartdate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rentalstartdate");
            entity.Property("Unitprice")
                .HasField("_unitprice")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey("Orderid")
                .HasConstraintName("fk_orderitem_order");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey("Productid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orderitem_product");
        });

        modelBuilder.Entity<Orderstatushistory>(entity =>
        {
            entity.HasKey("Historyid").HasName("orderstatushistory_pkey");

            entity.ToTable("orderstatushistory");

            entity.Property("Historyid")
                .HasField("_historyid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("historyid");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Remark")
                .HasField("_remark")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("remark");
            entity.Property("Timestamp")
                .HasField("_timestamp")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("timestamp");
            entity.Property("Updatedby")
                .HasField("_updatedby")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("updatedby");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderstatushistories)
                .HasForeignKey("Orderid")
                .HasConstraintName("fk_order_status_history_order");
        });

        modelBuilder.Entity<Packagingconfigmaterial>(entity =>
        {
            entity.HasKey("Configurationid", "Materialid").HasName("packagingconfigmaterials_pkey");

            entity.ToTable("packagingconfigmaterials");

            entity.Property("Configurationid")
                .HasField("_configurationid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("configurationid");
            entity.Property("Materialid")
                .HasField("_materialid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("materialid");
            entity.Property("Category")
                .HasField("_category")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property("Quantity")
                .HasField("_quantity")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("quantity");

            entity.HasOne(d => d.Configuration).WithMany(p => p.Packagingconfigmaterials)
                .HasForeignKey("Configurationid")
                .HasConstraintName("fk_pcm_configuration");

            entity.HasOne(d => d.Material).WithMany(p => p.Packagingconfigmaterials)
                .HasForeignKey("Materialid")
                .HasConstraintName("fk_pcm_material");
        });

        modelBuilder.Entity<Packagingconfiguration>(entity =>
        {
            entity.HasKey("Configurationid").HasName("packagingconfiguration_pkey");

            entity.ToTable("packagingconfiguration");

            entity.Property("Configurationid")
                .HasField("_configurationid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("configurationid");
            entity.Property("Profileid")
                .HasField("_profileid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("profileid");

            entity.HasOne(d => d.Profile).WithMany(p => p.Packagingconfigurations)
                .HasForeignKey("Profileid")
                .HasConstraintName("fk_packagingconfiguration_profile");
        });

        modelBuilder.Entity<Packagingmaterial>(entity =>
        {
            entity.HasKey("Materialid").HasName("packagingmaterial_pkey");

            entity.ToTable("packagingmaterial");

            entity.Property("Materialid")
                .HasField("_materialid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("materialid");
            entity.Property("Name")
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property("Recyclable")
                .HasField("_recyclable")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(false)
                .HasColumnName("recyclable");
            entity.Property("Reusable")
                .HasField("_reusable")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(false)
                .HasColumnName("reusable");
            entity.Property("Type")
                .HasField("_type")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Packagingprofile>(entity =>
        {
            entity.HasKey("Profileid").HasName("packagingprofile_pkey");

            entity.ToTable("packagingprofile");

            entity.Property("Profileid")
                .HasField("_profileid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("profileid");
            entity.Property("Fragilitylevel")
                .HasField("_fragilitylevel")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("fragilitylevel");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Volume")
                .HasField("_volume")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("volume");

            entity.HasOne(d => d.Order).WithMany(p => p.Packagingprofiles)
                .HasForeignKey("Orderid")
                .HasConstraintName("fk_packagingprofile_order");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey("Paymentid").HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property("Paymentid")
                .HasField("_paymentid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("paymentid");
            entity.Property("Amount")
                .HasField("_amount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("createdat");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Transactionid")
                .HasField("_transactionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("transactionid");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey("Orderid")
                .HasConstraintName("fk_payment_order");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Payments)
                .HasForeignKey("Transactionid")
                .HasConstraintName("fk_payment_transaction");
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey("TransportId").HasName("plane_pkey");

            entity.ToTable("plane");

            entity.Property("TransportId")
                .HasField("_transportId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property("PlaneCallsign")
                .HasField("_planeCallsign")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("plane_callsign");
            entity.Property("PlaneId")
                .HasField("_planeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("plane_id");
            entity.Property("PlaneType")
                .HasField("_planeType")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("plane_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Plane)
                .HasForeignKey<Plane>("TransportId")
                .HasConstraintName("fk_plane_transport");
        });

        modelBuilder.Entity<Polineitem>(entity =>
        {
            entity.HasKey("Polineid").HasName("polineitem_pkey");

            entity.ToTable("polineitem");

            entity.Property("Polineid")
                .HasField("_polineid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("polineid");
            entity.Property("Linetotal")
                .HasField("_linetotal")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("linetotal");
            entity.Property("Poid")
                .HasField("_poid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("poid");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Qty")
                .HasField("_qty")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("qty");
            entity.Property("Unitprice")
                .HasField("_unitprice")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Po).WithMany(p => p.Polineitems)
                .HasForeignKey("Poid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_polineitem_po");

            entity.HasOne(d => d.Product).WithMany(p => p.Polineitems)
                .HasForeignKey("Productid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_product_stock");
        });

        modelBuilder.Entity<PricingRule>(entity =>
        {
            entity.HasKey("RuleId").HasName("pricing_rule_pkey");

            entity.ToTable("pricing_rule");

            entity.Property("RuleId")
                .HasField("_ruleId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("rule_id");
            entity.Property("BaseRatePerKm")
                .HasField("_baseRatePerKm")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 4)
                .HasColumnName("base_rate_per_km");
            entity.Property("CarbonSurcharge")
                .HasField("_carbonSurcharge")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 4)
                .HasColumnName("carbon_surcharge");
            entity.Property("IsActive")
                .HasField("_isActive")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey("Productid").HasName("product_pkey");

            entity.ToTable("product");

            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("productid");
            entity.Property("Categoryid")
                .HasField("_categoryid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("categoryid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property("Sku")
                .HasField("_sku")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("sku");
            entity.Property("Threshold")
                .HasField("_threshold")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(5, 4)
                .HasColumnName("threshold");
            entity.Property("Updatedat")
                .HasField("_updatedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey("Categoryid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_product_category");
        });

        modelBuilder.Entity<ProductReturn>(entity =>
        {
            entity.HasKey("ReturnId").HasName("product_return_pkey");

            entity.ToTable("product_return");

            entity.Property("ReturnId")
                .HasField("_returnId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("return_id");
            entity.Property("DateIn")
                .HasField("_dateIn")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("date_in");
            entity.Property("DateOn")
                .HasField("_dateOn")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("date_on");
            entity.Property("ReturnStatus")
                .HasField("_returnStatus")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("return_status");
            entity.Property("TotalCarbon")
                .HasField("_totalCarbon")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("total_carbon");
        });

        modelBuilder.Entity<Productdetail>(entity =>
        {
            entity.HasKey("Detailsid").HasName("productdetails_pkey");

            entity.ToTable("productdetails");

            // entity.HasIndex("Productid", "productdetails_productid_key").IsUnique();
            entity.HasIndex("Productid").HasDatabaseName("productdetails_productid_key").IsUnique();

            entity.Property("Detailsid")
                .HasField("_detailsid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("detailsid");
            entity.Property("Depositrate")
                .HasField("_depositrate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("depositrate");
            entity.Property("Description")
                .HasField("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("description");
            entity.Property("Image")
                .HasField("_image")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property("Name")
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property("Price")
                .HasField("_price")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Totalquantity")
                .HasField("_totalquantity")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(0)
                .HasColumnName("totalquantity");
            entity.Property("Weight")
                .HasField("_weight")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("weight");

            entity.HasOne(d => d.Product).WithOne(p => p.Productdetail)
                .HasForeignKey<Productdetail>("Productid")
                .HasConstraintName("fk_productdetails_product");
        });

        modelBuilder.Entity<Productfootprint>(entity =>
        {
            entity.HasKey("Productcarbonfootprintid").HasName("productfootprint_pkey");

            entity.ToTable("productfootprint");

            entity.Property("Productcarbonfootprintid")
                .HasField("_productcarbonfootprintid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("productcarbonfootprintid");
            entity.Property("Badgeid")
                .HasField("_badgeid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("badgeid");
            entity.Property("Calculatedat")
                .HasField("_calculatedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("now()")
                .HasColumnName("calculatedat");
            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("productid");
            entity.Property("Producttoxicpercentage")
                .HasField("_producttoxicpercentage")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("producttoxicpercentage");
            entity.Property("Totalco2")
                .HasField("_totalco2")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("totalco2");

            entity.HasOne(d => d.Badge).WithMany(p => p.Productfootprints)
                .HasForeignKey("Badgeid")
                .HasConstraintName("fk_productfootprint_badge");

            entity.HasOne(d => d.Product).WithMany(p => p.Productfootprints)
                .HasForeignKey("Productid")
                .HasConstraintName("fk_productfootprint_product");
        });

        modelBuilder.Entity<Purchaseorder>(entity =>
        {
            entity.HasKey("Poid").HasName("purchaseorder_pkey");

            entity.ToTable("purchaseorder");

            entity.Property("Poid")
                .HasField("_poid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("poid");
            entity.Property("Expecteddeliverydate")
                .HasField("_expecteddeliverydate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("expecteddeliverydate");
            entity.Property("Podate")
                .HasField("_podate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("podate");
            entity.Property("Supplierid")
                .HasField("_supplierid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("supplierid");
            entity.Property("Totalamount")
                .HasField("_totalamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");
        });

        modelBuilder.Entity<Purchaseorderlog>(entity =>
        {
            entity.HasKey("Purchaseorderlogid").HasName("purchaseorderlog_pkey");

            entity.ToTable("purchaseorderlog");

            entity.Property("Purchaseorderlogid")
                .HasField("_purchaseorderlogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("purchaseorderlogid");
            entity.Property("Detailsjson")
                .HasField("_detailsjson")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("detailsjson");
            entity.Property("Expecteddeliverydate")
                .HasField("_expecteddeliverydate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("expecteddeliverydate");
            entity.Property("Podate")
                .HasField("_podate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("podate");
            entity.Property("Poid")
                .HasField("_poid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("poid");
            entity.Property("Supplierid")
                .HasField("_supplierid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("supplierid");
            entity.Property("Totalamount")
                .HasField("_totalamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Po).WithMany(p => p.Purchaseorderlogs)
                .HasForeignKey("Poid")
                .HasConstraintName("fk_po_log_po");

            entity.HasOne(d => d.PurchaseorderlogNavigation).WithOne(p => p.Purchaseorderlog)
                .HasForeignKey<Purchaseorderlog>("Purchaseorderlogid")
                .HasConstraintName("fk_po_transaction");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey("Refundid").HasName("refund_pkey");

            entity.ToTable("refund");

            entity.Property("Refundid")
                .HasField("_refundid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("refundid");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Depositrefundamount")
                .HasField("_depositrefundamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("depositrefundamount");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Penaltyamount")
                .HasField("_penaltyamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("penaltyamount");
            entity.Property("Returndate")
                .HasField("_returndate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returndate");
            entity.Property("Returnmethod")
                .HasField("_returnmethod")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("returnmethod");
            entity.Property("Returnrequestid")
                .HasField("_returnrequestid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returnrequestid");
            entity.Property("Transactionid")
                .HasField("_transactionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("transactionid");

            entity.HasOne(d => d.Customer).WithMany(p => p.Refunds)
                .HasForeignKey("Customerid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_refund_customer");

            entity.HasOne(d => d.Order).WithMany(p => p.Refunds)
                .HasForeignKey("Orderid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_refund_order");

            entity.HasOne(d => d.Returnrequest).WithMany(p => p.Refunds)
                .HasForeignKey("Returnrequestid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_refund_return");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Refunds)
                .HasForeignKey("Transactionid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_refund_transaction");
        });

        modelBuilder.Entity<Reliabilityrating>(entity =>
        {
            entity.HasKey("Ratingid").HasName("reliabilityrating_pkey");

            entity.ToTable("reliabilityrating");

            entity.Property("Ratingid")
                .HasField("_ratingid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ratingid");
            entity.Property("Calculatedat")
                .HasField("_calculatedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("calculatedat");
            entity.Property("Calculatedbyuserid")
                .HasField("_calculatedbyuserid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("calculatedbyuserid");
            entity.Property("Rationale")
                .HasField("_rationale")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rationale");
            entity.Property("Score")
                .HasField("_score")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(5, 2)
                .HasColumnName("score");
            entity.Property("Supplierid")
                .HasField("_supplierid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("supplierid");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Reliabilityratings)
                .HasForeignKey("Supplierid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reliabilityrating_supplier");
        });

        modelBuilder.Entity<Rentalorderlog>(entity =>
        {
            entity.HasKey("Rentalorderlogid").HasName("rentalorderlog_pkey");

            entity.ToTable("rentalorderlog");

            entity.Property("Rentalorderlogid")
                .HasField("_rentalorderlogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("rentalorderlogid");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Detailsjson")
                .HasField("_detailsjson")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("detailsjson");
            entity.Property("Orderdate")
                .HasField("_orderdate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderdate");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Totalamount")
                .HasField("_totalamount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Order).WithMany(p => p.Rentalorderlogs)
                .HasForeignKey("Orderid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_rental_order");

            entity.HasOne(d => d.RentalorderlogNavigation).WithOne(p => p.Rentalorderlog)
                .HasForeignKey<Rentalorderlog>("Rentalorderlogid")
                .HasConstraintName("fk_rental_transaction");
        });

        modelBuilder.Entity<Replenishmentrequest>(entity =>
        {
            entity.HasKey("Requestid").HasName("replenishmentrequest_pkey");

            entity.ToTable("replenishmentrequest");

            entity.Property("Requestid")
                .HasField("_requestid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestid");
            entity.Property("Completedat")
                .HasField("_completedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("completedat");
            entity.Property("Completedby")
                .HasField("_completedby")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("completedby");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("createdat");
            entity.Property("Remarks")
                .HasField("_remarks")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("remarks");
            entity.Property("Requestedby")
                .HasField("_requestedby")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("requestedby");
        });

        modelBuilder.Entity<Reportexport>(entity =>
        {
            entity.HasKey("Reportid").HasName("reportexport_pkey");

            entity.ToTable("reportexport");

            entity.Property("Reportid")
                .HasField("_reportid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("reportid");
            entity.Property("Refanalyticsid")
                .HasField("_refanalyticsid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("refanalyticsid");
            entity.Property("Title")
                .HasField("_title")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property("Url")
                .HasField("_url")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(500)
                .HasColumnName("url");

            entity.HasOne(d => d.Refanalytics).WithMany(p => p.Reportexports)
                .HasForeignKey("Refanalyticsid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reportexport_analytics");
        });

        modelBuilder.Entity<ReturnStage>(entity =>
        {
            entity.HasKey("StageId").HasName("return_stage_pkey");

            entity.ToTable("return_stage");

            entity.Property("StageId")
                .HasField("_stageId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("stage_id");
            entity.Property("CleaningSuppliesQty")
                .HasField("_cleaningSuppliesQty")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("cleaning_supplies_qty");
            entity.Property("EnergyKwh")
                .HasField("_energyKwh")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("energy_kwh");
            entity.Property("LabourHours")
                .HasField("_labourHours")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("labour_hours");
            entity.Property("MaterialsKg")
                .HasField("_materialsKg")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("materials_kg");
            entity.Property("PackagingKg")
                .HasField("_packagingKg")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("packaging_kg");
            entity.Property("ReturnId")
                .HasField("_returnId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("return_id");
            entity.Property("SurchargeRate")
                .HasField("_surchargeRate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 4)
                .HasColumnName("surcharge_rate");
            entity.Property("WaterLitres")
                .HasField("_waterLitres")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("water_litres");

            entity.HasOne(d => d.Return).WithMany(p => p.ReturnStages)
                .HasForeignKey("ReturnId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_return_stage_return_request");
        });

        modelBuilder.Entity<Returnitem>(entity =>
        {
            entity.HasKey("Returnitemid").HasName("returnitem_pkey");

            entity.ToTable("returnitem");

            entity.Property("Returnitemid")
                .HasField("_returnitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("returnitemid");
            entity.Property("Completiondate")
                .HasField("_completiondate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("completiondate");
            entity.Property("Image")
                .HasField("_image")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property("Inventoryitemid")
                .HasField("_inventoryitemid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("inventoryitemid");
            entity.Property("Returnrequestid")
                .HasField("_returnrequestid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returnrequestid");

            entity.HasOne(d => d.Inventoryitem).WithMany(p => p.Returnitems)
                .HasForeignKey("Inventoryitemid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_returnitem_inventory");

            entity.HasOne(d => d.Returnrequest).WithMany(p => p.Returnitems)
                .HasForeignKey("Returnrequestid")
                .HasConstraintName("fk_returnitem_request");
        });

        modelBuilder.Entity<Returnlog>(entity =>
        {
            entity.HasKey("Returnlogid").HasName("returnlog_pkey");

            entity.ToTable("returnlog");

            entity.Property("Returnlogid")
                .HasField("_returnlogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("returnlogid");
            entity.Property("Completiondate")
                .HasField("_completiondate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("completiondate");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("customerid");
            entity.Property("Detailsjson")
                .HasField("_detailsjson")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("detailsjson");
            entity.Property("Imageurl")
                .HasField("_imageurl")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(500)
                .HasColumnName("imageurl");
            entity.Property("Rentalorderlogid")
                .HasField("_rentalorderlogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("rentalorderlogid");
            entity.Property("Requestdate")
                .HasField("_requestdate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("requestdate");
            entity.Property("Returnrequestid")
                .HasField("_returnrequestid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("returnrequestid");

            entity.HasOne(d => d.Rentalorderlog).WithMany(p => p.Returnlogs)
                .HasForeignKey("Rentalorderlogid")
                .HasConstraintName("fk_return_rental");

            entity.HasOne(d => d.ReturnlogNavigation).WithOne(p => p.Returnlog)
                .HasForeignKey<Returnlog>("Returnlogid")
                .HasConstraintName("fk_return_transaction");

            entity.HasOne(d => d.Returnrequest).WithMany(p => p.Returnlogs)
                .HasForeignKey("Returnrequestid")
                .HasConstraintName("fk_return_request");
        });

        modelBuilder.Entity<Returnrequest>(entity =>
        {
            entity.HasKey("Returnrequestid").HasName("returnrequest_pkey");

            entity.ToTable("returnrequest");

            entity.Property("Returnrequestid")
                .HasField("_returnrequestid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("returnrequestid");
            entity.Property("Completiondate")
                .HasField("_completiondate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("completiondate");
            entity.Property("Customerid")
                .HasField("_customerid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("customerid");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Requestdate")
                .HasField("_requestdate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("requestdate");

            entity.HasOne(d => d.Customer).WithMany(p => p.Returnrequests)
                .HasForeignKey("Customerid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_returnrequest_customer");

            entity.HasOne(d => d.Order).WithMany(p => p.Returnrequests)
                .HasForeignKey("Orderid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_returnrequest_order");
        });

        modelBuilder.Entity<RouteLeg>(entity =>
        {
            entity.HasKey("LegId").HasName("route_leg_pkey");

            entity.ToTable("route_leg");

            entity.Property("LegId")
                .HasField("_legId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("leg_id");
            entity.Property("DistanceKm")
                .HasField("_distanceKm")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("distance_km");
            entity.Property("EndPoint")
                .HasField("_endPoint")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("end_point");
            entity.Property("IsFirstMile")
                .HasField("_isFirstMile")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(false)
                .HasColumnName("is_first_mile");
            entity.Property("IsLastMile")
                .HasField("_isLastMile")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(false)
                .HasColumnName("is_last_mile");
            entity.Property("RouteId")
                .HasField("_routeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("route_id");
            entity.Property("Sequence")
                .HasField("_sequence")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("sequence");
            entity.Property("StartPoint")
                .HasField("_startPoint")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("start_point");
            entity.Property("TransportId")
                .HasField("_transportId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("transport_id");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteLegs)
                .HasForeignKey("RouteId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_route_leg_route");

            entity.HasOne(d => d.Transport).WithMany(p => p.RouteLegs)
                .HasForeignKey("TransportId")
                .HasConstraintName("fk_route_leg_transport");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey("Sessionid").HasName("session_pkey");

            entity.ToTable("session");

            entity.Property("Sessionid")
                .HasField("_sessionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sessionid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("createdat");
            entity.Property("Expiresat")
                .HasField("_expiresat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("expiresat");
            entity.Property("Role")
                .HasField("_role")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property("Userid")
                .HasField("_userid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey("Userid")
                .HasConstraintName("fk_session_user");
        });

        modelBuilder.Entity<Ship>(entity =>
        {
            entity.HasKey("TransportId").HasName("ship_pkey");

            entity.ToTable("ship");

            entity.Property("TransportId")
                .HasField("_transportId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property("MaxVesselSize")
                .HasField("_maxVesselSize")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("max_vessel_size");
            entity.Property("ShipId")
                .HasField("_shipId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("ship_id");
            entity.Property("VesselNumber")
                .HasField("_vesselNumber")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("vessel_number");
            entity.Property("VesselType")
                .HasField("_vesselType")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("vessel_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Ship)
                .HasForeignKey<Ship>("TransportId")
                .HasConstraintName("fk_ship_transport");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey("Trackingid").HasName("shipment_pkey");

            entity.ToTable("shipment");

            entity.Property("Trackingid")
                .HasField("_trackingid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("trackingid");
            entity.Property("Batchid")
                .HasField("_batchid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("batchid");
            entity.Property("Destination")
                .HasField("_destination")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("destination");
            entity.Property("Orderid")
                .HasField("_orderid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("orderid");
            entity.Property("Weight")
                .HasField("_weight")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("weight");

            entity.HasOne(d => d.Batch).WithMany(p => p.Shipments)
                .HasForeignKey("Batchid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_shipment_batch");

            entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                .HasForeignKey("Orderid")
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_shipment_order");
        });

        modelBuilder.Entity<ShippingOption>(entity =>
        {
            entity.HasKey("OptionId").HasName("shipping_option_pkey");

            entity.ToTable("shipping_option");

            entity.Property("OptionId")
                .HasField("_optionId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("option_id");
            entity.Property("Carbonfootprintkg")
                .HasField("_carbonfootprintkg")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("carbonfootprintkg");
            entity.Property("Cost")
                .HasField("_cost")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property("DeliveryDays")
                .HasField("_deliveryDays")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("delivery_days");
            entity.Property("DisplayName")
                .HasField("_displayName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("display_name");
            entity.Property("OrderId")
                .HasField("_orderId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("order_id");
            entity.Property("RouteId")
                .HasField("_routeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("route_id");

            entity.HasOne(d => d.Order).WithMany(p => p.ShippingOptions)
                .HasForeignKey("OrderId")
                .HasConstraintName("fk_shipping_option_order");

            entity.HasOne(d => d.Route).WithMany(p => p.ShippingOptions)
                .HasForeignKey("RouteId")
                .HasConstraintName("fk_shipping_option_route");
        });

        modelBuilder.Entity<ShippingPort>(entity =>
        {
            entity.HasKey("HubId").HasName("shipping_port_pkey");

            entity.ToTable("shipping_port");

            entity.Property("HubId")
                .HasField("_hubId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("hub_id");
            entity.Property("PortCode")
                .HasField("_portCode")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(20)
                .HasColumnName("port_code");
            entity.Property("PortName")
                .HasField("_portName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("port_name");
            entity.Property("PortType")
                .HasField("_portType")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("port_type");
            entity.Property("VesselSize")
                .HasField("_vesselSize")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("vessel_size");

            entity.HasOne(d => d.Hub).WithOne(p => p.ShippingPort)
                .HasForeignKey<ShippingPort>("HubId")
                .HasConstraintName("fk_shipping_port_hub");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey("Staffid").HasName("staff_pkey");

            entity.ToTable("staff");

            // entity.HasIndex("Userid", "staff_userid_key").IsUnique();
            entity.HasIndex("Userid").HasDatabaseName("staff_userid_key").IsUnique();

            entity.Property("Staffid")
                .HasField("_staffid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("staffid");
            entity.Property("Department")
                .HasField("_department")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("department");
            entity.Property("Userid")
                .HasField("_userid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Staff)
                .HasForeignKey<Staff>("Userid")
                .HasConstraintName("fk_staff_user");
        });

        modelBuilder.Entity<Staffaccesslog>(entity =>
        {
            entity.HasKey("Accessid").HasName("staffaccesslog_pkey");

            entity.ToTable("staffaccesslog");

            entity.Property("Accessid")
                .HasField("_accessid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("accessid");
            entity.Property("Eventtime")
                .HasField("_eventtime")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("now()")
                .HasColumnName("eventtime");
            entity.Property("Staffid")
                .HasField("_staffid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("staffid");

            entity.HasOne(d => d.Staff).WithMany(p => p.Staffaccesslogs)
                .HasForeignKey("Staffid")
                .HasConstraintName("fk_staffaccesslog_staff");
        });

        modelBuilder.Entity<Stafffootprint>(entity =>
        {
            entity.HasKey("Staffcarbonfootprintid").HasName("stafffootprint_pkey");

            entity.ToTable("stafffootprint");

            entity.Property("Staffcarbonfootprintid")
                .HasField("_staffcarbonfootprintid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("staffcarbonfootprintid");
            entity.Property("Hoursworked")
                .HasField("_hoursworked")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("hoursworked");
            entity.Property("Staffid")
                .HasField("_staffid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("staffid");
            entity.Property("Time")
                .HasField("_time")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("now()")
                .HasColumnName("time");
            entity.Property("Totalstaffco2")
                .HasField("_totalstaffco2")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("totalstaffco2");

            entity.HasOne(d => d.Staff).WithMany(p => p.Stafffootprints)
                .HasForeignKey("Staffid")
                .HasConstraintName("fk_stafffootprint_staff");
        });

        modelBuilder.Entity<Stockitem>(entity =>
        {
            entity.HasKey("Productid").HasName("stockitem_pkey");

            entity.ToTable("stockitem");

            entity.Property("Productid")
                .HasField("_productid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("productid");
            entity.Property("Name")
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property("Sku")
                .HasField("_sku")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(100)
                .HasColumnName("sku");
            entity.Property("Uom")
                .HasField("_uom")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("uom");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey("Supplierid").HasName("supplier_pkey");

            entity.ToTable("supplier");

            entity.Property("Supplierid")
                .HasField("_supplierid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("supplierid");
            entity.Property("Avgturnaroundtime")
                .HasField("_avgturnaroundtime")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("avgturnaroundtime");
            entity.Property("Creditperiod")
                .HasField("_creditperiod")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("creditperiod");
            entity.Property("Details")
                .HasField("_details")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(500)
                .HasColumnName("details");
            entity.Property("Isverified")
                .HasField("_isverified")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("isverified");
            entity.Property("Name")
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Suppliercategorychangelog>(entity =>
        {
            entity.HasKey("Logid").HasName("suppliercategorychangelog_pkey");

            entity.ToTable("suppliercategorychangelog");

            entity.Property("Logid")
                .HasField("_logid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("logid");
            entity.Property("Changedat")
                .HasField("_changedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("changedat");
            entity.Property("Changereason")
                .HasField("_changereason")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("changereason");
            entity.Property("Supplierid")
                .HasField("_supplierid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("supplierid");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Suppliercategorychangelogs)
                .HasForeignKey("Supplierid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_suppliercatelog_supplier");
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.HasKey("TransportId").HasName("train_pkey");

            entity.ToTable("train");

            entity.Property("TransportId")
                .HasField("_transportId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property("TrainId")
                .HasField("_trainId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("train_id");
            entity.Property("TrainNumber")
                .HasField("_trainNumber")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("train_number");
            entity.Property("TrainType")
                .HasField("_trainType")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("train_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Train)
                .HasForeignKey<Train>("TransportId")
                .HasConstraintName("fk_train_transport");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey("Transactionid").HasName("transaction_pkey");

            entity.ToTable("transaction");

            entity.Property("Transactionid")
                .HasField("_transactionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("transactionid");
            entity.Property("Amount")
                .HasField("_amount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("createdat");
            entity.Property("Providertransactionid")
                .HasField("_providertransactionid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(100)
                .HasColumnName("providertransactionid");
        });

        modelBuilder.Entity<Transactionlog>(entity =>
        {
            entity.HasKey("Transactionlogid").HasName("transactionlog_pkey");

            entity.ToTable("transactionlog");

            entity.Property("Transactionlogid")
                .HasField("_transactionlogid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("transactionlogid");
            entity.Property("Createdat")
                .HasField("_createdat")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey("TransportId").HasName("transport_pkey");

            entity.ToTable("transport");

            entity.Property("TransportId")
                .HasField("_transportId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("transport_id");
            entity.Property("IsAvailable")
                .HasField("_isAvailable")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasDefaultValue(true)
                .HasColumnName("is_available");
            entity.Property("MaxLoadKg")
                .HasField("_maxLoadKg")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("max_load_kg");
            entity.Property("VehicleSizeM2")
                .HasField("_vehicleSizeM2")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("vehicle_size_m2");
        });

        modelBuilder.Entity<TransportationHub>(entity =>
        {
            entity.HasKey("HubId").HasName("transportation_hub_pkey");

            entity.ToTable("transportation_hub");

            entity.Property("HubId")
                .HasField("_hubId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("hub_id");
            entity.Property("Address")
                .HasField("_address")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property("CountryCode")
                .HasField("_countryCode")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(10)
                .HasColumnName("country_code");
            entity.Property("Latitude")
                .HasField("_latitude")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("latitude");
            entity.Property("Longitude")
                .HasField("_longitude")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("longitude");
            entity.Property("OperationTime")
                .HasField("_operationTime")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("operation_time");
            entity.Property("OperationalStatus")
                .HasField("_operationalStatus")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("operational_status");
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.HasKey("TransportId").HasName("truck_pkey");

            entity.ToTable("truck");

            entity.Property("TransportId")
                .HasField("_transportId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property("LicensePlate")
                .HasField("_licensePlate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("license_plate");
            entity.Property("TruckId")
                .HasField("_truckId")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("truck_id");
            entity.Property("TruckType")
                .HasField("_truckType")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(50)
                .HasColumnName("truck_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Truck)
                .HasForeignKey<Truck>("TransportId")
                .HasConstraintName("fk_truck_transport");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey("Userid").HasName("User_pkey");

            entity.ToTable("User");

            // entity.HasIndex("Email", "User_email_key").IsUnique();
            entity.HasIndex("Email").HasDatabaseName("User_email_key").IsUnique();

            entity.Property("Userid")
                .HasField("_userid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("userid");
            entity.Property("Email")
                .HasField("_email")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property("Name")
                .HasField("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property("Passwordhash")
                .HasField("_passwordhash")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property("Phonecountry")
                .HasField("_phonecountry")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("phonecountry");
            entity.Property("Phonenumber")
                .HasField("_phonenumber")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
        });

        modelBuilder.Entity<Vettingrecord>(entity =>
        {
            entity.HasKey("Vettingid").HasName("vettingrecord_pkey");

            entity.ToTable("vettingrecord");

            entity.Property("Vettingid")
                .HasField("_vettingid")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .UseIdentityAlwaysColumn()
                .HasColumnName("vettingid");
            entity.Property("Notes")
                .HasField("_notes")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("notes");
            entity.Property("Ratingid")
                .HasField("_ratingid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("ratingid");
            entity.Property("Supplierid")
                .HasField("_supplierid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("supplierid");
            entity.Property("Vettedat")
                .HasField("_vettedat")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("vettedat");
            entity.Property("Vettedbyuserid")
                .HasField("_vettedbyuserid")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("vettedbyuserid");

            entity.HasOne(d => d.Rating).WithMany(p => p.Vettingrecords)
                .HasForeignKey("Ratingid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_vettingrecord_rating");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Vettingrecords)
                .HasForeignKey("Supplierid")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_vettingrecord_supplier");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey("HubId").HasName("warehouse_pkey");

            entity.ToTable("warehouse");

            entity.Property("HubId")
                .HasField("_hubId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .ValueGeneratedNever()
                .HasColumnName("hub_id");
            entity.Property("ClimateControlEmissionRate")
                .HasField("_climateControlEmissionRate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("climate_control_emission_rate");
            entity.Property("LightingEmissionRate")
                .HasField("_lightingEmissionRate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("lighting_emission_rate");
            entity.Property("MaxProductCapacity")
                .HasField("_maxProductCapacity")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("max_product_capacity");
            entity.Property("SecuritySystemEmissionRate")
                .HasField("_securitySystemEmissionRate")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("security_system_emission_rate");
            entity.Property("TotalWarehouseVolume")
                .HasField("_totalWarehouseVolume")
                .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("total_warehouse_volume");
            entity.Property("WarehouseCode")
                .HasField("_warehouseCode")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(100)
                .HasColumnName("warehouse_code");

            entity.HasOne(d => d.Hub).WithOne(p => p.Warehouse)
                .HasForeignKey<Warehouse>("HubId")
                .HasConstraintName("fk_warehouse_hub");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
