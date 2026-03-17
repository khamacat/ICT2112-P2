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

    public virtual DbSet<CarbonResult> CarbonResults { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Cartitem> Cartitems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Checkout> Checkouts { get; set; }

    public virtual DbSet<Clearancebatch> Clearancebatches { get; set; }

    public virtual DbSet<Clearanceitem> Clearanceitems { get; set; }

    public virtual DbSet<Clearancelog> Clearancelogs { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerChoice> CustomerChoices { get; set; }

    public virtual DbSet<Customerreward> Customerrewards { get; set; }

    public virtual DbSet<Damagereport> Damagereports { get; set; }

    public virtual DbSet<DeliveryBatch> DeliveryBatches { get; set; }

    public virtual DbSet<DeliveryRoute> DeliveryRoutes { get; set; }

    public virtual DbSet<Deliverymethod> Deliverymethods { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Ecobadge> Ecobadges { get; set; }

    public virtual DbSet<Inventoryitem> Inventoryitems { get; set; }

    public virtual DbSet<LegCarbon> LegCarbons { get; set; }

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
            .HasPostgresEnum("vetting_decision_enum", new[] { "APPROVED", "REJECTED", "PENDING" })
            .HasPostgresEnum("vetting_result_enum", new[] { "APPROVED", "REJECTED", "PENDING" })
            .HasPostgresEnum("visual_type_enum", new[] { "TABLE", "BAR", "COLUMN", "LINE", "PIE", "AREA" });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.HubId).HasName("airport_pkey");

            entity.ToTable("airport");

            entity.Property(e => e.HubId)
                .ValueGeneratedNever()
                .HasColumnName("hub_id");
            entity.Property(e => e.AircraftSize).HasColumnName("aircraft_size");
            entity.Property(e => e.AirportCode)
                .HasMaxLength(10)
                .HasColumnName("airport_code");
            entity.Property(e => e.AirportName)
                .HasMaxLength(255)
                .HasColumnName("airport_name");
            entity.Property(e => e.Terminal).HasColumnName("terminal");

            entity.HasOne(d => d.Hub).WithOne(p => p.Airport)
                .HasForeignKey<Airport>(d => d.HubId)
                .HasConstraintName("fk_airport_hub");
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.Alertid).HasName("alert_pkey");

            entity.ToTable("alert");

            entity.Property(e => e.Alertid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("alertid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Currentstock).HasColumnName("currentstock");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.Minthreshold).HasColumnName("minthreshold");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Product).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk_alert_product");
        });

        modelBuilder.Entity<Analytic>(entity =>
        {
            entity.HasKey(e => e.Analyticsid).HasName("analytics_pkey");

            entity.ToTable("analytics");

            entity.Property(e => e.Analyticsid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("analyticsid");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Loanamt).HasColumnName("loanamt");
            entity.Property(e => e.Primaryitem)
                .HasMaxLength(255)
                .HasColumnName("primaryitem");
            entity.Property(e => e.Primarysupplier)
                .HasMaxLength(255)
                .HasColumnName("primarysupplier");
            entity.Property(e => e.Returnamt).HasColumnName("returnamt");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");
            entity.Property(e => e.Supplierreliability)
                .HasPrecision(10, 2)
                .HasColumnName("supplierreliability");
            entity.Property(e => e.Turnoverrate)
                .HasPrecision(10, 2)
                .HasColumnName("turnoverrate");

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
            entity.HasKey(e => new { e.BatchId, e.OrderId }).HasName("batch_order_pkey");

            entity.ToTable("batch_order");

            entity.HasIndex(e => e.OrderId, "batch_order_order_id_key").IsUnique();

            entity.Property(e => e.BatchId).HasColumnName("batch_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.AddedTimestamp)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("added_timestamp");

            entity.HasOne(d => d.Batch).WithMany(p => p.BatchOrders)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_batch_order_batch");

            entity.HasOne(d => d.Order).WithOne(p => p.BatchOrder)
                .HasForeignKey<BatchOrder>(d => d.OrderId)
                .HasConstraintName("fk_batch_order_order");
        });

        modelBuilder.Entity<Buildingfootprint>(entity =>
        {
            entity.HasKey(e => e.Buildingcarbonfootprintid).HasName("buildingfootprint_pkey");

            entity.ToTable("buildingfootprint");

            entity.Property(e => e.Buildingcarbonfootprintid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("buildingcarbonfootprintid");
            entity.Property(e => e.Block)
                .HasMaxLength(50)
                .HasColumnName("block");
            entity.Property(e => e.Floor)
                .HasMaxLength(50)
                .HasColumnName("floor");
            entity.Property(e => e.Room)
                .HasMaxLength(50)
                .HasColumnName("room");
            entity.Property(e => e.Timehourly)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timehourly");
            entity.Property(e => e.Totalroomco2).HasColumnName("totalroomco2");
            entity.Property(e => e.Zone)
                .HasMaxLength(50)
                .HasColumnName("zone");
        });

        modelBuilder.Entity<CarbonEmission>(entity =>
        {
            entity.HasKey(e => e.EmissionId).HasName("carbon_emission_pkey");

            entity.ToTable("carbon_emission");

            entity.Property(e => e.EmissionId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("emission_id");
            entity.Property(e => e.CarbonKg).HasColumnName("carbon_kg");
            entity.Property(e => e.StageId).HasColumnName("stage_id");

            entity.HasOne(d => d.Stage).WithMany(p => p.CarbonEmissions)
                .HasForeignKey(d => d.StageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_carbon_emission_stage");
        });

        modelBuilder.Entity<CarbonResult>(entity =>
        {
            entity.HasKey(e => e.CarbonResultId).HasName("carbon_result_pkey");

            entity.ToTable("carbon_result");

            entity.Property(e => e.CarbonResultId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("carbon_result_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.TotalCarbonKg).HasColumnName("total_carbon_kg");
            entity.Property(e => e.ValidationPassed)
                .HasDefaultValue(false)
                .HasColumnName("validation_passed");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Cartid).HasName("cart_pkey");

            entity.ToTable("cart");

            entity.Property(e => e.Cartid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("cartid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Rentalend)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("rentalend");
            entity.Property(e => e.Rentalstart)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("rentalstart");
            entity.Property(e => e.Sessionid).HasColumnName("sessionid");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cart_customer");

            entity.HasOne(d => d.Session).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Sessionid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cart_session");
        });

        modelBuilder.Entity<Cartitem>(entity =>
        {
            entity.HasKey(e => e.Cartitemid).HasName("cartitem_pkey");

            entity.ToTable("cartitem");

            entity.Property(e => e.Cartitemid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("cartitemid");
            entity.Property(e => e.Cartid).HasColumnName("cartid");
            entity.Property(e => e.Isselected)
                .HasDefaultValue(true)
                .HasColumnName("isselected");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.Cartid)
                .HasConstraintName("fk_cartitem_cart");

            entity.HasOne(d => d.Product).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_cartitem_product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Categoryid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("categoryid");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Updateddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updateddate");
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey(e => e.Checkoutid).HasName("checkout_pkey");

            entity.ToTable("checkout");

            entity.Property(e => e.Checkoutid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("checkoutid");
            entity.Property(e => e.Cartid).HasColumnName("cartid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Deliveryid).HasColumnName("deliveryid");
            entity.Property(e => e.Notifyoptin)
                .HasDefaultValue(false)
                .HasColumnName("notifyoptin");

            entity.HasOne(d => d.Cart).WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.Cartid)
                .HasConstraintName("fk_checkout_cart");

            entity.HasOne(d => d.Customer).WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_checkout_customer");

            entity.HasOne(d => d.Delivery).WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.Deliveryid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_checkout_delivery");
        });

        modelBuilder.Entity<Clearancebatch>(entity =>
        {
            entity.HasKey(e => e.Clearancebatchid).HasName("clearancebatch_pkey");

            entity.ToTable("clearancebatch");

            entity.Property(e => e.Clearancebatchid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("clearancebatchid");
            entity.Property(e => e.Batchname)
                .HasMaxLength(255)
                .HasColumnName("batchname");
            entity.Property(e => e.Clearancedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("clearancedate");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
        });

        modelBuilder.Entity<Clearanceitem>(entity =>
        {
            entity.HasKey(e => e.Clearanceitemid).HasName("clearanceitem_pkey");

            entity.ToTable("clearanceitem");

            entity.HasIndex(e => e.Inventoryitemid, "clearanceitem_inventoryitemid_key").IsUnique();

            entity.Property(e => e.Clearanceitemid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("clearanceitemid");
            entity.Property(e => e.Clearancebatchid).HasColumnName("clearancebatchid");
            entity.Property(e => e.Finalprice)
                .HasPrecision(10, 2)
                .HasColumnName("finalprice");
            entity.Property(e => e.Inventoryitemid).HasColumnName("inventoryitemid");
            entity.Property(e => e.Recommendedprice)
                .HasPrecision(10, 2)
                .HasColumnName("recommendedprice");
            entity.Property(e => e.Saledate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("saledate");

            entity.HasOne(d => d.Clearancebatch).WithMany(p => p.Clearanceitems)
                .HasForeignKey(d => d.Clearancebatchid)
                .HasConstraintName("fk_clearance_batch");

            entity.HasOne(d => d.Inventoryitem).WithOne(p => p.Clearanceitem)
                .HasForeignKey<Clearanceitem>(d => d.Inventoryitemid)
                .HasConstraintName("fk_clearance_inventory");
        });

        modelBuilder.Entity<Clearancelog>(entity =>
        {
            entity.HasKey(e => e.Clearancelogid).HasName("clearancelog_pkey");

            entity.ToTable("clearancelog");

            entity.Property(e => e.Clearancelogid)
                .ValueGeneratedNever()
                .HasColumnName("clearancelogid");
            entity.Property(e => e.Batchname)
                .HasMaxLength(255)
                .HasColumnName("batchname");
            entity.Property(e => e.Clearancebatchid).HasColumnName("clearancebatchid");
            entity.Property(e => e.Clearancedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("clearancedate");
            entity.Property(e => e.Detailsjson).HasColumnName("detailsjson");

            entity.HasOne(d => d.Clearancebatch).WithMany(p => p.Clearancelogs)
                .HasForeignKey(d => d.Clearancebatchid)
                .HasConstraintName("fk_clearance_batch");

            entity.HasOne(d => d.ClearancelogNavigation).WithOne(p => p.Clearancelog)
                .HasForeignKey<Clearancelog>(d => d.Clearancelogid)
                .HasConstraintName("fk_clearance_transaction");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Userid, "customer_userid_key").IsUnique();

            entity.Property(e => e.Customerid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("customerid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Customertype).HasColumnName("customertype");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.Userid)
                .HasConstraintName("fk_customer_user");
        });

        modelBuilder.Entity<CustomerChoice>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.OrderId }).HasName("customer_choice_pkey");

            entity.ToTable("customer_choice");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerChoices)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_customerchoice_customer");

            entity.HasOne(d => d.Order).WithMany(p => p.CustomerChoices)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_customerchoice_order");
        });

        modelBuilder.Entity<Customerreward>(entity =>
        {
            entity.HasKey(e => e.Rewardid).HasName("customerrewards_pkey");

            entity.ToTable("customerrewards");

            entity.Property(e => e.Rewardid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("rewardid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Ordercarbondataid).HasColumnName("ordercarbondataid");
            entity.Property(e => e.Rewardtype)
                .HasMaxLength(50)
                .HasColumnName("rewardtype");
            entity.Property(e => e.Rewardvalue).HasColumnName("rewardvalue");

            entity.HasOne(d => d.Customer).WithMany(p => p.Customerrewards)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("fk_customerrewards_customer");

            entity.HasOne(d => d.Ordercarbondata).WithMany(p => p.Customerrewards)
                .HasForeignKey(d => d.Ordercarbondataid)
                .HasConstraintName("fk_customerrewards_ordercarbondata");
        });

        modelBuilder.Entity<Damagereport>(entity =>
        {
            entity.HasKey(e => e.Damagereportid).HasName("damagereport_pkey");

            entity.ToTable("damagereport");

            entity.Property(e => e.Damagereportid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("damagereportid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Images)
                .HasMaxLength(255)
                .HasColumnName("images");
            entity.Property(e => e.Repaircost)
                .HasPrecision(10, 2)
                .HasColumnName("repaircost");
            entity.Property(e => e.Reportdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("reportdate");
            entity.Property(e => e.Returnitemid).HasColumnName("returnitemid");
            entity.Property(e => e.Severity)
                .HasMaxLength(255)
                .HasColumnName("severity");

            entity.HasOne(d => d.Returnitem).WithMany(p => p.Damagereports)
                .HasForeignKey(d => d.Returnitemid)
                .HasConstraintName("fk_damagereport_returnitem");
        });

        modelBuilder.Entity<DeliveryBatch>(entity =>
        {
            entity.HasKey(e => e.DeliveryBatchId).HasName("delivery_batch_pkey");

            entity.ToTable("delivery_batch");

            entity.Property(e => e.DeliveryBatchId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("delivery_batch_id");
            entity.Property(e => e.BatchWeightKg).HasColumnName("batch_weight_kg");
            entity.Property(e => e.CarbonSavings).HasColumnName("carbon_savings");
            entity.Property(e => e.DestinationAddress)
                .HasMaxLength(255)
                .HasColumnName("destination_address");
            entity.Property(e => e.HubId).HasColumnName("hub_id");
            entity.Property(e => e.TotalOrders)
                .HasDefaultValue(0)
                .HasColumnName("total_orders");

            entity.HasOne(d => d.Hub).WithMany(p => p.DeliveryBatches)
                .HasForeignKey(d => d.HubId)
                .HasConstraintName("fk_delivery_batch_hub");
        });

        modelBuilder.Entity<DeliveryRoute>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("delivery_route_pkey");

            entity.ToTable("delivery_route");

            entity.Property(e => e.RouteId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("route_id");
            entity.Property(e => e.DestinationAddress)
                .HasMaxLength(255)
                .HasColumnName("destination_address");
            entity.Property(e => e.DestinationHubId).HasColumnName("destination_hub_id");
            entity.Property(e => e.IsValid)
                .HasDefaultValue(true)
                .HasColumnName("is_valid");
            entity.Property(e => e.OriginAddress)
                .HasMaxLength(255)
                .HasColumnName("origin_address");
            entity.Property(e => e.OriginHubId).HasColumnName("origin_hub_id");
            entity.Property(e => e.TotalDistanceKm).HasColumnName("total_distance_km");

            entity.HasOne(d => d.DestinationHub).WithMany(p => p.DeliveryRouteDestinationHubs)
                .HasForeignKey(d => d.DestinationHubId)
                .HasConstraintName("fk_route_destination_hub");

            entity.HasOne(d => d.OriginHub).WithMany(p => p.DeliveryRouteOriginHubs)
                .HasForeignKey(d => d.OriginHubId)
                .HasConstraintName("fk_route_origin_hub");
        });

        modelBuilder.Entity<Deliverymethod>(entity =>
        {
            entity.HasKey(e => e.Deliveryid).HasName("deliverymethod_pkey");

            entity.ToTable("deliverymethod");

            entity.Property(e => e.Deliveryid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("deliveryid");
            entity.Property(e => e.Carrierid)
                .HasMaxLength(50)
                .HasColumnName("carrierid");
            entity.Property(e => e.Deliverycost)
                .HasPrecision(10, 2)
                .HasColumnName("deliverycost");
            entity.Property(e => e.Durationdays).HasColumnName("durationdays");
            entity.Property(e => e.Orderid).HasColumnName("orderid");

            entity.HasOne(d => d.Order).WithMany(p => p.Deliverymethods)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_deliverymethod_order");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Depositid).HasName("deposit_pkey");

            entity.ToTable("deposit");

            entity.Property(e => e.Depositid)
                .HasMaxLength(50)
                .HasColumnName("depositid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Forfeitedamount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("forfeitedamount");
            entity.Property(e => e.Heldamount)
                .HasPrecision(10, 2)
                .HasColumnName("heldamount");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Originalamount)
                .HasPrecision(10, 2)
                .HasColumnName("originalamount");
            entity.Property(e => e.Refundedamount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("refundedamount");
            entity.Property(e => e.Transactionid).HasColumnName("transactionid");

            entity.HasOne(d => d.Order).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_deposit_order");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("fk_deposit_transaction");
        });

        modelBuilder.Entity<Ecobadge>(entity =>
        {
            entity.HasKey(e => e.Badgeid).HasName("ecobadge_pkey");

            entity.ToTable("ecobadge");

            entity.Property(e => e.Badgeid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("badgeid");
            entity.Property(e => e.Badgename)
                .HasMaxLength(100)
                .HasColumnName("badgename");
            entity.Property(e => e.Criteriadescription)
                .HasMaxLength(255)
                .HasColumnName("criteriadescription");
            entity.Property(e => e.Maxcarbong).HasColumnName("maxcarbong");
        });

        modelBuilder.Entity<Inventoryitem>(entity =>
        {
            entity.HasKey(e => e.Inventoryid).HasName("inventoryitem_pkey");

            entity.ToTable("inventoryitem");

            entity.HasIndex(e => e.Serialnumber, "inventoryitem_serialnumber_key").IsUnique();

            entity.Property(e => e.Inventoryid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("inventoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Expirydate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expirydate");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Serialnumber)
                .HasMaxLength(255)
                .HasColumnName("serialnumber");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventoryitems)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk_inventory_product");
        });

        modelBuilder.Entity<LegCarbon>(entity =>
        {
            entity.HasKey(e => e.LegId).HasName("leg_carbon_pkey");

            entity.ToTable("leg_carbon");

            entity.Property(e => e.LegId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("leg_id");
            entity.Property(e => e.CarbonKg).HasColumnName("carbon_kg");
            entity.Property(e => e.CarbonRate).HasColumnName("carbon_rate");
            entity.Property(e => e.CarbonResultId).HasColumnName("carbon_result_id");
            entity.Property(e => e.DistanceKm).HasColumnName("distance_km");
            entity.Property(e => e.RouteLegId).HasColumnName("route_leg_id");
            entity.Property(e => e.WeightKg).HasColumnName("weight_kg");

            entity.HasOne(d => d.CarbonResult).WithMany(p => p.LegCarbons)
                .HasForeignKey(d => d.CarbonResultId)
                .HasConstraintName("fk_leg_carbon_result");

            entity.HasOne(d => d.RouteLeg).WithMany(p => p.LegCarbons)
                .HasForeignKey(d => d.RouteLegId)
                .HasConstraintName("fk_leg_carbon_leg");
        });

        modelBuilder.Entity<Lineitem>(entity =>
        {
            entity.HasKey(e => e.Lineitemid).HasName("lineitem_pkey");

            entity.ToTable("lineitem");

            entity.Property(e => e.Lineitemid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("lineitemid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantityrequest).HasColumnName("quantityrequest");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Product).WithMany(p => p.Lineitems)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_lineitem_product");

            entity.HasOne(d => d.Request).WithMany(p => p.Lineitems)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_lineitem_request");
        });

        modelBuilder.Entity<Loanitem>(entity =>
        {
            entity.HasKey(e => e.Loanitemid).HasName("loanitem_pkey");

            entity.ToTable("loanitem");

            entity.Property(e => e.Loanitemid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("loanitemid");
            entity.Property(e => e.Inventoryitemid).HasColumnName("inventoryitemid");
            entity.Property(e => e.Loanlistid).HasColumnName("loanlistid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");

            entity.HasOne(d => d.Inventoryitem).WithMany(p => p.Loanitems)
                .HasForeignKey(d => d.Inventoryitemid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_loanitem_inventory");

            entity.HasOne(d => d.Loanlist).WithMany(p => p.Loanitems)
                .HasForeignKey(d => d.Loanlistid)
                .HasConstraintName("fk_loanitem_loan");
        });

        modelBuilder.Entity<Loanlist>(entity =>
        {
            entity.HasKey(e => e.Loanlistid).HasName("loanlist_pkey");

            entity.ToTable("loanlist");

            entity.Property(e => e.Loanlistid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("loanlistid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Duedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("duedate");
            entity.Property(e => e.Loandate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("loandate");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Returndate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("returndate");

            entity.HasOne(d => d.Customer).WithMany(p => p.Loanlists)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_loan_customer");

            entity.HasOne(d => d.Order).WithMany(p => p.Loanlists)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_loan_order");
        });

        modelBuilder.Entity<Loanlog>(entity =>
        {
            entity.HasKey(e => e.Loanlogid).HasName("loanlog_pkey");

            entity.ToTable("loanlog");

            entity.Property(e => e.Loanlogid)
                .ValueGeneratedNever()
                .HasColumnName("loanlogid");
            entity.Property(e => e.Detailsjson).HasColumnName("detailsjson");
            entity.Property(e => e.Duedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("duedate");
            entity.Property(e => e.Loandate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("loandate");
            entity.Property(e => e.Loanlistid).HasColumnName("loanlistid");
            entity.Property(e => e.Rentalorderlogid).HasColumnName("rentalorderlogid");
            entity.Property(e => e.Returndate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("returndate");

            entity.HasOne(d => d.Loanlist).WithMany(p => p.Loanlogs)
                .HasForeignKey(d => d.Loanlistid)
                .HasConstraintName("fk_loan_list");

            entity.HasOne(d => d.LoanlogNavigation).WithOne(p => p.Loanlog)
                .HasForeignKey<Loanlog>(d => d.Loanlogid)
                .HasConstraintName("fk_loan_transaction");

            entity.HasOne(d => d.Rentalorderlog).WithMany(p => p.Loanlogs)
                .HasForeignKey(d => d.Rentalorderlogid)
                .HasConstraintName("fk_loan_rental");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Notificationid).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.Notificationid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("notificationid");
            entity.Property(e => e.Datesent)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datesent");
            entity.Property(e => e.Isread)
                .HasDefaultValue(false)
                .HasColumnName("isread");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_notification_user");
        });

        modelBuilder.Entity<Notificationpreference>(entity =>
        {
            entity.HasKey(e => e.Preferenceid).HasName("notificationpreference_pkey");

            entity.ToTable("notificationpreference");

            entity.Property(e => e.Preferenceid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("preferenceid");
            entity.Property(e => e.Emailenabled)
                .HasDefaultValue(true)
                .HasColumnName("emailenabled");
            entity.Property(e => e.Smsenabled)
                .HasDefaultValue(false)
                .HasColumnName("smsenabled");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Notificationpreferences)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_notificationpref_user");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Orderid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orderid");
            entity.Property(e => e.Checkoutid).HasColumnName("checkoutid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Orderdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdate");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");
            entity.Property(e => e.Transactionid).HasColumnName("transactionid");

            entity.HasOne(d => d.Checkout).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Checkoutid)
                .HasConstraintName("fk_order_checkout");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_order_customer");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Transactionid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_order_transaction");
        });

        modelBuilder.Entity<Ordercarbondatum>(entity =>
        {
            entity.HasKey(e => e.Ordercarbondataid).HasName("ordercarbondata_pkey");

            entity.ToTable("ordercarbondata");

            entity.Property(e => e.Ordercarbondataid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ordercarbondataid");
            entity.Property(e => e.Buildingcarbon).HasColumnName("buildingcarbon");
            entity.Property(e => e.Calculatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("calculatedat");
            entity.Property(e => e.Impactlevel)
                .HasMaxLength(20)
                .HasColumnName("impactlevel");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Packagingcarbon).HasColumnName("packagingcarbon");
            entity.Property(e => e.Productcarbon).HasColumnName("productcarbon");
            entity.Property(e => e.Staffcarbon).HasColumnName("staffcarbon");
            entity.Property(e => e.Totalcarbon).HasColumnName("totalcarbon");

            entity.HasOne(d => d.Order).WithMany(p => p.Ordercarbondata)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_ordercarbondata_order");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Orderitemid).HasName("orderitem_pkey");

            entity.ToTable("orderitem");

            entity.Property(e => e.Orderitemid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orderitemid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rentalenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("rentalenddate");
            entity.Property(e => e.Rentalstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("rentalstartdate");
            entity.Property(e => e.Unitprice)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_orderitem_order");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orderitem_product");
        });

        modelBuilder.Entity<Orderstatushistory>(entity =>
        {
            entity.HasKey(e => e.Historyid).HasName("orderstatushistory_pkey");

            entity.ToTable("orderstatushistory");

            entity.Property(e => e.Historyid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("historyid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("remark");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
            entity.Property(e => e.Updatedby)
                .HasMaxLength(50)
                .HasColumnName("updatedby");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderstatushistories)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_order_status_history_order");
        });

        modelBuilder.Entity<Packagingconfigmaterial>(entity =>
        {
            entity.HasKey(e => new { e.Configurationid, e.Materialid }).HasName("packagingconfigmaterials_pkey");

            entity.ToTable("packagingconfigmaterials");

            entity.Property(e => e.Configurationid).HasColumnName("configurationid");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Configuration).WithMany(p => p.Packagingconfigmaterials)
                .HasForeignKey(d => d.Configurationid)
                .HasConstraintName("fk_pcm_configuration");

            entity.HasOne(d => d.Material).WithMany(p => p.Packagingconfigmaterials)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("fk_pcm_material");
        });

        modelBuilder.Entity<Packagingconfiguration>(entity =>
        {
            entity.HasKey(e => e.Configurationid).HasName("packagingconfiguration_pkey");

            entity.ToTable("packagingconfiguration");

            entity.Property(e => e.Configurationid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("configurationid");
            entity.Property(e => e.Profileid).HasColumnName("profileid");

            entity.HasOne(d => d.Profile).WithMany(p => p.Packagingconfigurations)
                .HasForeignKey(d => d.Profileid)
                .HasConstraintName("fk_packagingconfiguration_profile");
        });

        modelBuilder.Entity<Packagingmaterial>(entity =>
        {
            entity.HasKey(e => e.Materialid).HasName("packagingmaterial_pkey");

            entity.ToTable("packagingmaterial");

            entity.Property(e => e.Materialid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("materialid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Recyclable)
                .HasDefaultValue(false)
                .HasColumnName("recyclable");
            entity.Property(e => e.Reusable)
                .HasDefaultValue(false)
                .HasColumnName("reusable");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Packagingprofile>(entity =>
        {
            entity.HasKey(e => e.Profileid).HasName("packagingprofile_pkey");

            entity.ToTable("packagingprofile");

            entity.Property(e => e.Profileid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("profileid");
            entity.Property(e => e.Fragilitylevel)
                .HasMaxLength(50)
                .HasColumnName("fragilitylevel");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Volume).HasColumnName("volume");

            entity.HasOne(d => d.Order).WithMany(p => p.Packagingprofiles)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_packagingprofile_order");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property(e => e.Paymentid)
                .HasMaxLength(50)
                .HasColumnName("paymentid");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Transactionid).HasColumnName("transactionid");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_payment_order");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("fk_payment_transaction");
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("plane_pkey");

            entity.ToTable("plane");

            entity.Property(e => e.TransportId)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property(e => e.PlaneCallsign)
                .HasMaxLength(50)
                .HasColumnName("plane_callsign");
            entity.Property(e => e.PlaneId).HasColumnName("plane_id");
            entity.Property(e => e.PlaneType)
                .HasMaxLength(50)
                .HasColumnName("plane_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Plane)
                .HasForeignKey<Plane>(d => d.TransportId)
                .HasConstraintName("fk_plane_transport");
        });

        modelBuilder.Entity<Polineitem>(entity =>
        {
            entity.HasKey(e => e.Polineid).HasName("polineitem_pkey");

            entity.ToTable("polineitem");

            entity.Property(e => e.Polineid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("polineid");
            entity.Property(e => e.Linetotal)
                .HasPrecision(10, 2)
                .HasColumnName("linetotal");
            entity.Property(e => e.Poid).HasColumnName("poid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Unitprice)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Po).WithMany(p => p.Polineitems)
                .HasForeignKey(d => d.Poid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_polineitem_po");

            entity.HasOne(d => d.Product).WithMany(p => p.Polineitems)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_product_stock");
        });

        modelBuilder.Entity<PricingRule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("pricing_rule_pkey");

            entity.ToTable("pricing_rule");

            entity.Property(e => e.RuleId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("rule_id");
            entity.Property(e => e.BaseRatePerKm)
                .HasPrecision(10, 4)
                .HasColumnName("base_rate_per_km");
            entity.Property(e => e.CarbonSurcharge)
                .HasPrecision(10, 4)
                .HasColumnName("carbon_surcharge");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Productid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("productid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Sku)
                .HasMaxLength(255)
                .HasColumnName("sku");
            entity.Property(e => e.Threshold)
                .HasPrecision(5, 4)
                .HasColumnName("threshold");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_product_category");
        });

        modelBuilder.Entity<ProductReturn>(entity =>
        {
            entity.HasKey(e => e.ReturnId).HasName("product_return_pkey");

            entity.ToTable("product_return");

            entity.Property(e => e.ReturnId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("return_id");
            entity.Property(e => e.DateIn).HasColumnName("date_in");
            entity.Property(e => e.DateOn).HasColumnName("date_on");
            entity.Property(e => e.ReturnStatus)
                .HasMaxLength(50)
                .HasColumnName("return_status");
            entity.Property(e => e.TotalCarbon).HasColumnName("total_carbon");
        });

        modelBuilder.Entity<Productdetail>(entity =>
        {
            entity.HasKey(e => e.Detailsid).HasName("productdetails_pkey");

            entity.ToTable("productdetails");

            entity.HasIndex(e => e.Productid, "productdetails_productid_key").IsUnique();

            entity.Property(e => e.Detailsid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("detailsid");
            entity.Property(e => e.Depositrate)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("depositrate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Totalquantity)
                .HasDefaultValue(0)
                .HasColumnName("totalquantity");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasColumnName("weight");

            entity.HasOne(d => d.Product).WithOne(p => p.Productdetail)
                .HasForeignKey<Productdetail>(d => d.Productid)
                .HasConstraintName("fk_productdetails_product");
        });

        modelBuilder.Entity<Productfootprint>(entity =>
        {
            entity.HasKey(e => e.Productcarbonfootprintid).HasName("productfootprint_pkey");

            entity.ToTable("productfootprint");

            entity.Property(e => e.Productcarbonfootprintid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("productcarbonfootprintid");
            entity.Property(e => e.Badgeid).HasColumnName("badgeid");
            entity.Property(e => e.Calculatedat)
                .HasDefaultValueSql("now()")
                .HasColumnName("calculatedat");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Producttoxicpercentage).HasColumnName("producttoxicpercentage");
            entity.Property(e => e.Totalco2).HasColumnName("totalco2");

            entity.HasOne(d => d.Badge).WithMany(p => p.Productfootprints)
                .HasForeignKey(d => d.Badgeid)
                .HasConstraintName("fk_productfootprint_badge");

            entity.HasOne(d => d.Product).WithMany(p => p.Productfootprints)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk_productfootprint_product");
        });

        modelBuilder.Entity<Purchaseorder>(entity =>
        {
            entity.HasKey(e => e.Poid).HasName("purchaseorder_pkey");

            entity.ToTable("purchaseorder");

            entity.Property(e => e.Poid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("poid");
            entity.Property(e => e.Expecteddeliverydate).HasColumnName("expecteddeliverydate");
            entity.Property(e => e.Podate).HasColumnName("podate");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");
        });

        modelBuilder.Entity<Purchaseorderlog>(entity =>
        {
            entity.HasKey(e => e.Purchaseorderlogid).HasName("purchaseorderlog_pkey");

            entity.ToTable("purchaseorderlog");

            entity.Property(e => e.Purchaseorderlogid)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("purchaseorderlogid");
            entity.Property(e => e.Detailsjson).HasColumnName("detailsjson");
            entity.Property(e => e.Expecteddeliverydate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expecteddeliverydate");
            entity.Property(e => e.Podate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("podate");
            entity.Property(e => e.Poid).HasColumnName("poid");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Po).WithMany(p => p.Purchaseorderlogs)
                .HasForeignKey(d => d.Poid)
                .HasConstraintName("fk_po_log_po");

            entity.HasOne(d => d.PurchaseorderlogNavigation).WithOne(p => p.Purchaseorderlog)
                .HasForeignKey<Purchaseorderlog>(d => d.Purchaseorderlogid)
                .HasConstraintName("fk_po_transaction");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Refundid).HasName("refund_pkey");

            entity.ToTable("refund");

            entity.Property(e => e.Refundid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("refundid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Depositrefundamount)
                .HasPrecision(10, 2)
                .HasColumnName("depositrefundamount");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Penaltyamount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("penaltyamount");
            entity.Property(e => e.Returndate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("returndate");
            entity.Property(e => e.Returnmethod)
                .HasMaxLength(50)
                .HasColumnName("returnmethod");
            entity.Property(e => e.Returnrequestid).HasColumnName("returnrequestid");
            entity.Property(e => e.Transactionid).HasColumnName("transactionid");

            entity.HasOne(d => d.Customer).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_refund_customer");

            entity.HasOne(d => d.Order).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_refund_order");

            entity.HasOne(d => d.Returnrequest).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.Returnrequestid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_refund_return");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.Transactionid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_refund_transaction");
        });

        modelBuilder.Entity<Reliabilityrating>(entity =>
        {
            entity.HasKey(e => e.Ratingid).HasName("reliabilityrating_pkey");

            entity.ToTable("reliabilityrating");

            entity.Property(e => e.Ratingid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ratingid");
            entity.Property(e => e.Calculatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("calculatedat");
            entity.Property(e => e.Calculatedbyuserid).HasColumnName("calculatedbyuserid");
            entity.Property(e => e.Rationale).HasColumnName("rationale");
            entity.Property(e => e.Score)
                .HasPrecision(5, 2)
                .HasColumnName("score");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Reliabilityratings)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reliabilityrating_supplier");
        });

        modelBuilder.Entity<Rentalorderlog>(entity =>
        {
            entity.HasKey(e => e.Rentalorderlogid).HasName("rentalorderlog_pkey");

            entity.ToTable("rentalorderlog");

            entity.Property(e => e.Rentalorderlogid)
                .ValueGeneratedNever()
                .HasColumnName("rentalorderlogid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Detailsjson).HasColumnName("detailsjson");
            entity.Property(e => e.Orderdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdate");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Order).WithMany(p => p.Rentalorderlogs)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_rental_order");

            entity.HasOne(d => d.RentalorderlogNavigation).WithOne(p => p.Rentalorderlog)
                .HasForeignKey<Rentalorderlog>(d => d.Rentalorderlogid)
                .HasConstraintName("fk_rental_transaction");
        });

        modelBuilder.Entity<Replenishmentrequest>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("replenishmentrequest_pkey");

            entity.ToTable("replenishmentrequest");

            entity.Property(e => e.Requestid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestid");
            entity.Property(e => e.Completedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completedat");
            entity.Property(e => e.Completedby)
                .HasMaxLength(255)
                .HasColumnName("completedby");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Requestedby)
                .HasMaxLength(255)
                .HasColumnName("requestedby");
        });

        modelBuilder.Entity<Reportexport>(entity =>
        {
            entity.HasKey(e => e.Reportid).HasName("reportexport_pkey");

            entity.ToTable("reportexport");

            entity.Property(e => e.Reportid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("reportid");
            entity.Property(e => e.Refanalyticsid).HasColumnName("refanalyticsid");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");

            entity.HasOne(d => d.Refanalytics).WithMany(p => p.Reportexports)
                .HasForeignKey(d => d.Refanalyticsid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reportexport_analytics");
        });

        modelBuilder.Entity<ReturnStage>(entity =>
        {
            entity.HasKey(e => e.StageId).HasName("return_stage_pkey");

            entity.ToTable("return_stage");

            entity.Property(e => e.StageId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("stage_id");
            entity.Property(e => e.CleaningSuppliesQty).HasColumnName("cleaning_supplies_qty");
            entity.Property(e => e.EnergyKwh).HasColumnName("energy_kwh");
            entity.Property(e => e.LabourHours).HasColumnName("labour_hours");
            entity.Property(e => e.MaterialsKg).HasColumnName("materials_kg");
            entity.Property(e => e.PackagingKg).HasColumnName("packaging_kg");
            entity.Property(e => e.ReturnId).HasColumnName("return_id");
            entity.Property(e => e.SurchargeRate)
                .HasPrecision(10, 4)
                .HasColumnName("surcharge_rate");
            entity.Property(e => e.WaterLitres).HasColumnName("water_litres");

            entity.HasOne(d => d.Return).WithMany(p => p.ReturnStages)
                .HasForeignKey(d => d.ReturnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_return_stage_return_request");
        });

        modelBuilder.Entity<Returnitem>(entity =>
        {
            entity.HasKey(e => e.Returnitemid).HasName("returnitem_pkey");

            entity.ToTable("returnitem");

            entity.Property(e => e.Returnitemid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("returnitemid");
            entity.Property(e => e.Completiondate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completiondate");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Inventoryitemid).HasColumnName("inventoryitemid");
            entity.Property(e => e.Returnrequestid).HasColumnName("returnrequestid");

            entity.HasOne(d => d.Inventoryitem).WithMany(p => p.Returnitems)
                .HasForeignKey(d => d.Inventoryitemid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_returnitem_inventory");

            entity.HasOne(d => d.Returnrequest).WithMany(p => p.Returnitems)
                .HasForeignKey(d => d.Returnrequestid)
                .HasConstraintName("fk_returnitem_request");
        });

        modelBuilder.Entity<Returnlog>(entity =>
        {
            entity.HasKey(e => e.Returnlogid).HasName("returnlog_pkey");

            entity.ToTable("returnlog");

            entity.Property(e => e.Returnlogid)
                .ValueGeneratedNever()
                .HasColumnName("returnlogid");
            entity.Property(e => e.Completiondate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completiondate");
            entity.Property(e => e.Customerid)
                .HasMaxLength(50)
                .HasColumnName("customerid");
            entity.Property(e => e.Detailsjson).HasColumnName("detailsjson");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(500)
                .HasColumnName("imageurl");
            entity.Property(e => e.Rentalorderlogid).HasColumnName("rentalorderlogid");
            entity.Property(e => e.Requestdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("requestdate");
            entity.Property(e => e.Returnrequestid).HasColumnName("returnrequestid");

            entity.HasOne(d => d.Rentalorderlog).WithMany(p => p.Returnlogs)
                .HasForeignKey(d => d.Rentalorderlogid)
                .HasConstraintName("fk_return_rental");

            entity.HasOne(d => d.ReturnlogNavigation).WithOne(p => p.Returnlog)
                .HasForeignKey<Returnlog>(d => d.Returnlogid)
                .HasConstraintName("fk_return_transaction");

            entity.HasOne(d => d.Returnrequest).WithMany(p => p.Returnlogs)
                .HasForeignKey(d => d.Returnrequestid)
                .HasConstraintName("fk_return_request");
        });

        modelBuilder.Entity<Returnrequest>(entity =>
        {
            entity.HasKey(e => e.Returnrequestid).HasName("returnrequest_pkey");

            entity.ToTable("returnrequest");

            entity.Property(e => e.Returnrequestid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("returnrequestid");
            entity.Property(e => e.Completiondate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completiondate");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Requestdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("requestdate");

            entity.HasOne(d => d.Customer).WithMany(p => p.Returnrequests)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_returnrequest_customer");

            entity.HasOne(d => d.Order).WithMany(p => p.Returnrequests)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_returnrequest_order");
        });

        modelBuilder.Entity<RouteLeg>(entity =>
        {
            entity.HasKey(e => e.LegId).HasName("route_leg_pkey");

            entity.ToTable("route_leg");

            entity.Property(e => e.LegId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("leg_id");
            entity.Property(e => e.DistanceKm).HasColumnName("distance_km");
            entity.Property(e => e.EndPoint)
                .HasMaxLength(255)
                .HasColumnName("end_point");
            entity.Property(e => e.IsFirstMile)
                .HasDefaultValue(false)
                .HasColumnName("is_first_mile");
            entity.Property(e => e.IsLastMile)
                .HasDefaultValue(false)
                .HasColumnName("is_last_mile");
            entity.Property(e => e.RouteId).HasColumnName("route_id");
            entity.Property(e => e.Sequence).HasColumnName("sequence");
            entity.Property(e => e.StartPoint)
                .HasMaxLength(255)
                .HasColumnName("start_point");
            entity.Property(e => e.TransportId).HasColumnName("transport_id");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteLegs)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_route_leg_route");

            entity.HasOne(d => d.Transport).WithMany(p => p.RouteLegs)
                .HasForeignKey(d => d.TransportId)
                .HasConstraintName("fk_route_leg_transport");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Sessionid).HasName("session_pkey");

            entity.ToTable("session");

            entity.Property(e => e.Sessionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sessionid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Expiresat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiresat");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_session_user");
        });

        modelBuilder.Entity<Ship>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("ship_pkey");

            entity.ToTable("ship");

            entity.Property(e => e.TransportId)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property(e => e.MaxVesselSize)
                .HasMaxLength(50)
                .HasColumnName("max_vessel_size");
            entity.Property(e => e.ShipId).HasColumnName("ship_id");
            entity.Property(e => e.VesselNumber)
                .HasMaxLength(50)
                .HasColumnName("vessel_number");
            entity.Property(e => e.VesselType)
                .HasMaxLength(50)
                .HasColumnName("vessel_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Ship)
                .HasForeignKey<Ship>(d => d.TransportId)
                .HasConstraintName("fk_ship_transport");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.Trackingid).HasName("shipment_pkey");

            entity.ToTable("shipment");

            entity.Property(e => e.Trackingid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("trackingid");
            entity.Property(e => e.Batchid).HasColumnName("batchid");
            entity.Property(e => e.Destination)
                .HasMaxLength(255)
                .HasColumnName("destination");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Batch).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.Batchid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_shipment_batch");

            entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_shipment_order");
        });

        modelBuilder.Entity<ShippingOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("shipping_option_pkey");

            entity.ToTable("shipping_option");

            entity.Property(e => e.OptionId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("option_id");
            entity.Property(e => e.Carbonfootprintkg).HasColumnName("carbonfootprintkg");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.DeliveryDays).HasColumnName("delivery_days");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(255)
                .HasColumnName("display_name");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.RouteId).HasColumnName("route_id");

            entity.HasOne(d => d.Order).WithMany(p => p.ShippingOptions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_shipping_option_order");

            entity.HasOne(d => d.Route).WithMany(p => p.ShippingOptions)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("fk_shipping_option_route");
        });

        modelBuilder.Entity<ShippingPort>(entity =>
        {
            entity.HasKey(e => e.HubId).HasName("shipping_port_pkey");

            entity.ToTable("shipping_port");

            entity.Property(e => e.HubId)
                .ValueGeneratedNever()
                .HasColumnName("hub_id");
            entity.Property(e => e.PortCode)
                .HasMaxLength(20)
                .HasColumnName("port_code");
            entity.Property(e => e.PortName)
                .HasMaxLength(255)
                .HasColumnName("port_name");
            entity.Property(e => e.PortType)
                .HasMaxLength(50)
                .HasColumnName("port_type");
            entity.Property(e => e.VesselSize).HasColumnName("vessel_size");

            entity.HasOne(d => d.Hub).WithOne(p => p.ShippingPort)
                .HasForeignKey<ShippingPort>(d => d.HubId)
                .HasConstraintName("fk_shipping_port_hub");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Staffid).HasName("staff_pkey");

            entity.ToTable("staff");

            entity.HasIndex(e => e.Userid, "staff_userid_key").IsUnique();

            entity.Property(e => e.Staffid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("staffid");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .HasColumnName("department");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.Userid)
                .HasConstraintName("fk_staff_user");
        });

        modelBuilder.Entity<Staffaccesslog>(entity =>
        {
            entity.HasKey(e => e.Accessid).HasName("staffaccesslog_pkey");

            entity.ToTable("staffaccesslog");

            entity.Property(e => e.Accessid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("accessid");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("now()")
                .HasColumnName("eventtime");
            entity.Property(e => e.Staffid).HasColumnName("staffid");

            entity.HasOne(d => d.Staff).WithMany(p => p.Staffaccesslogs)
                .HasForeignKey(d => d.Staffid)
                .HasConstraintName("fk_staffaccesslog_staff");
        });

        modelBuilder.Entity<Stafffootprint>(entity =>
        {
            entity.HasKey(e => e.Staffcarbonfootprintid).HasName("stafffootprint_pkey");

            entity.ToTable("stafffootprint");

            entity.Property(e => e.Staffcarbonfootprintid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("staffcarbonfootprintid");
            entity.Property(e => e.Hoursworked).HasColumnName("hoursworked");
            entity.Property(e => e.Staffid).HasColumnName("staffid");
            entity.Property(e => e.Time)
                .HasDefaultValueSql("now()")
                .HasColumnName("time");
            entity.Property(e => e.Totalstaffco2).HasColumnName("totalstaffco2");

            entity.HasOne(d => d.Staff).WithMany(p => p.Stafffootprints)
                .HasForeignKey(d => d.Staffid)
                .HasConstraintName("fk_stafffootprint_staff");
        });

        modelBuilder.Entity<Stockitem>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("stockitem_pkey");

            entity.ToTable("stockitem");

            entity.Property(e => e.Productid)
                .ValueGeneratedNever()
                .HasColumnName("productid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("sku");
            entity.Property(e => e.Uom)
                .HasMaxLength(50)
                .HasColumnName("uom");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Supplierid).HasName("supplier_pkey");

            entity.ToTable("supplier");

            entity.Property(e => e.Supplierid)
                .ValueGeneratedNever()
                .HasColumnName("supplierid");
            entity.Property(e => e.Avgturnaroundtime).HasColumnName("avgturnaroundtime");
            entity.Property(e => e.Creditperiod).HasColumnName("creditperiod");
            entity.Property(e => e.Details)
                .HasMaxLength(500)
                .HasColumnName("details");
            entity.Property(e => e.Isverified).HasColumnName("isverified");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Suppliercategorychangelog>(entity =>
        {
            entity.HasKey(e => e.Logid).HasName("suppliercategorychangelog_pkey");

            entity.ToTable("suppliercategorychangelog");

            entity.Property(e => e.Logid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("logid");
            entity.Property(e => e.Changedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changedat");
            entity.Property(e => e.Changereason)
                .HasMaxLength(255)
                .HasColumnName("changereason");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Suppliercategorychangelogs)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_suppliercatelog_supplier");
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("train_pkey");

            entity.ToTable("train");

            entity.Property(e => e.TransportId)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property(e => e.TrainId).HasColumnName("train_id");
            entity.Property(e => e.TrainNumber)
                .HasMaxLength(50)
                .HasColumnName("train_number");
            entity.Property(e => e.TrainType)
                .HasMaxLength(50)
                .HasColumnName("train_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Train)
                .HasForeignKey<Train>(d => d.TransportId)
                .HasConstraintName("fk_train_transport");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("transaction_pkey");

            entity.ToTable("transaction");

            entity.Property(e => e.Transactionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("transactionid");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Providertransactionid)
                .HasMaxLength(100)
                .HasColumnName("providertransactionid");
        });

        modelBuilder.Entity<Transactionlog>(entity =>
        {
            entity.HasKey(e => e.Transactionlogid).HasName("transactionlog_pkey");

            entity.ToTable("transactionlog");

            entity.Property(e => e.Transactionlogid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("transactionlogid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("transport_pkey");

            entity.ToTable("transport");

            entity.Property(e => e.TransportId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("transport_id");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("is_available");
            entity.Property(e => e.MaxLoadKg).HasColumnName("max_load_kg");
            entity.Property(e => e.VehicleSizeM2).HasColumnName("vehicle_size_m2");
        });

        modelBuilder.Entity<TransportationHub>(entity =>
        {
            entity.HasKey(e => e.HubId).HasName("transportation_hub_pkey");

            entity.ToTable("transportation_hub");

            entity.Property(e => e.HubId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("hub_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(10)
                .HasColumnName("country_code");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.OperationTime)
                .HasMaxLength(50)
                .HasColumnName("operation_time");
            entity.Property(e => e.OperationalStatus)
                .HasMaxLength(50)
                .HasColumnName("operational_status");
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("truck_pkey");

            entity.ToTable("truck");

            entity.Property(e => e.TransportId)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(50)
                .HasColumnName("license_plate");
            entity.Property(e => e.TruckId).HasColumnName("truck_id");
            entity.Property(e => e.TruckType)
                .HasMaxLength(50)
                .HasColumnName("truck_type");

            entity.HasOne(d => d.Transport).WithOne(p => p.Truck)
                .HasForeignKey<Truck>(d => d.TransportId)
                .HasConstraintName("fk_truck_transport");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "User_email_key").IsUnique();

            entity.Property(e => e.Userid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Phonecountry).HasColumnName("phonecountry");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
        });

        modelBuilder.Entity<Vettingrecord>(entity =>
        {
            entity.HasKey(e => e.Vettingid).HasName("vettingrecord_pkey");

            entity.ToTable("vettingrecord");

            entity.Property(e => e.Vettingid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("vettingid");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Ratingid).HasColumnName("ratingid");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Vettedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("vettedat");
            entity.Property(e => e.Vettedbyuserid).HasColumnName("vettedbyuserid");

            entity.HasOne(d => d.Rating).WithMany(p => p.Vettingrecords)
                .HasForeignKey(d => d.Ratingid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_vettingrecord_rating");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Vettingrecords)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_vettingrecord_supplier");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.HubId).HasName("warehouse_pkey");

            entity.ToTable("warehouse");

            entity.Property(e => e.HubId)
                .ValueGeneratedNever()
                .HasColumnName("hub_id");
            entity.Property(e => e.ClimateControlEmissionRate).HasColumnName("climate_control_emission_rate");
            entity.Property(e => e.LightingEmissionRate).HasColumnName("lighting_emission_rate");
            entity.Property(e => e.MaxProductCapacity).HasColumnName("max_product_capacity");
            entity.Property(e => e.SecuritySystemEmissionRate).HasColumnName("security_system_emission_rate");
            entity.Property(e => e.TotalWarehouseVolume).HasColumnName("total_warehouse_volume");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(100)
                .HasColumnName("warehouse_code");

            entity.HasOne(d => d.Hub).WithOne(p => p.Warehouse)
                .HasForeignKey<Warehouse>(d => d.HubId)
                .HasConstraintName("fk_warehouse_hub");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
