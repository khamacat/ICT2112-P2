using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.UnitOfWork;

public partial class AppDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
		// ==========================================
		// PostgreSQL Enum Registrations
		// ==========================================
		modelBuilder.HasPostgresEnum<AccessEventType>("access_event_type");
		modelBuilder.HasPostgresEnum<AlertStatus>("alert_status");
		modelBuilder.HasPostgresEnum<AnalyticsType>("analytics_type_enum");
		modelBuilder.HasPostgresEnum<BatchStatus>("batch_status");
		modelBuilder.HasPostgresEnum<CarbonStageType>("carbon_stage_type");
		modelBuilder.HasPostgresEnum<CartStatus>("cart_status_enum");
		modelBuilder.HasPostgresEnum<CheckoutStatus>("checkout_status_enum");
		modelBuilder.HasPostgresEnum<ClearanceBatchStatus>("clearance_batch_status");
		modelBuilder.HasPostgresEnum<ClearanceStatus>("clearance_status");
		modelBuilder.HasPostgresEnum<DeliveryDuration>("delivery_duration_enum");
		modelBuilder.HasPostgresEnum<DeliveryType>("delivery_type_enum");
		modelBuilder.HasPostgresEnum<FileFormat>("file_format_enum");
		modelBuilder.HasPostgresEnum<HubType>("hub_type");
		modelBuilder.HasPostgresEnum<InventoryStatus>("inventory_status");
		modelBuilder.HasPostgresEnum<LoanStatus>("loan_status");
		modelBuilder.HasPostgresEnum<LogType>("log_type_enum");
		modelBuilder.HasPostgresEnum<NotificationFrequency>("notification_frequency_enum");
		modelBuilder.HasPostgresEnum<NotificationGranularity>("notification_granularity_enum");
		modelBuilder.HasPostgresEnum<NotificationType>("notification_type_enum");
		modelBuilder.HasPostgresEnum<OrderHistoryStatus>("order_history_status_enum");
		modelBuilder.HasPostgresEnum<OrderStatus>("order_status_enum");
		modelBuilder.HasPostgresEnum<PaymentMethod>("payment_method_enum");
		modelBuilder.HasPostgresEnum<PaymentPurpose>("payment_purpose_enum");
		modelBuilder.HasPostgresEnum<POStatus>("po_status_enum");
		modelBuilder.HasPostgresEnum<PreferenceType>("preference_type");
		modelBuilder.HasPostgresEnum<ProductStatus>("product_status");
		modelBuilder.HasPostgresEnum<PurchaseOrderStatus>("purchase_order_status_enum");
		modelBuilder.HasPostgresEnum<RatingBand>("rating_band_enum");
		modelBuilder.HasPostgresEnum<RentalStatus>("rental_status_enum");
		modelBuilder.HasPostgresEnum<ReplenishmentReason>("reason_code_enum");
		modelBuilder.HasPostgresEnum<ReplenishmentStatus>("replenishment_status_enum");
		modelBuilder.HasPostgresEnum<ReturnItemStatus>("return_item_status");
		modelBuilder.HasPostgresEnum<ReturnRequestStatus>("return_request_status");
		modelBuilder.HasPostgresEnum<ReturnStatus>("return_status_enum");
		modelBuilder.HasPostgresEnum<ShipmentStatus>("shipment_status_enum");
		modelBuilder.HasPostgresEnum<StageType>("stagetype");
		modelBuilder.HasPostgresEnum<SupplierCategory>("supplier_category_enum");
		modelBuilder.HasPostgresEnum<TransactionPurpose>("transaction_purpose_enum");
		modelBuilder.HasPostgresEnum<TransactionStatus>("transaction_status_enum");
		modelBuilder.HasPostgresEnum<TransactionType>("transaction_type_enum");
		modelBuilder.HasPostgresEnum<TransportMode>("transport_mode");
            modelBuilder.HasPostgresEnum<UserRole>("user_role_enum");
		modelBuilder.HasPostgresEnum<VettingDecision>("vetting_decision_enum");
		modelBuilder.HasPostgresEnum<VettingResult>("vetting_result_enum");
		modelBuilder.HasPostgresEnum<VisualType>("visual_type_enum");

        // ==========================================
        // A - C
        // ==========================================
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("alert_status");
        });

        modelBuilder.Entity<Analytic>(entity =>
        {
            entity.Property("type").HasField("_type").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("type").HasColumnType("analytics_type_enum");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("cart_status_enum");
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("checkout_status_enum");

            entity.Property("PaymentMethodType").HasField("_paymentMethodType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("paymentmethodtype").HasColumnType("payment_method_enum");
        });

        modelBuilder.Entity<Clearancebatch>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("clearance_batch_status");
        });

        modelBuilder.Entity<Clearanceitem>(entity =>
        {
            // Verify if your DB uses clearance_status or clearance_status_enum
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("clearance_status"); 
        });

        modelBuilder.Entity<CustomerChoice>(entity =>
        {
            entity.Property("PreferenceType").HasField("_preferenceType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("preferencetype").HasColumnType("preference_type");
        });

        // ==========================================
        // D - L
        // ==========================================
        modelBuilder.Entity<DeliveryBatch>(entity =>
        {
            entity.Property("DeliveryBatchStatus").HasField("_deliveryBatchStatus").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("deliverybatchstatus").HasColumnType("batch_status");
        });

        modelBuilder.Entity<Inventoryitem>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("inventory_status");
        });

        modelBuilder.Entity<LegCarbon>(entity =>
        {
            entity.Property("TransportMode").HasField("_transportMode").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("transportmode").HasColumnType("transport_mode");
        });

        modelBuilder.Entity<Lineitem>(entity =>
        {
            entity.Property("reason").HasField("_reason").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("reason").HasColumnType("reason_code_enum");
        });

        modelBuilder.Entity<Loanlist>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("loan_status");
        });

        // ==========================================
        // N - P
        // ==========================================
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property("NotificationType").HasField("_notificationType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("notificationtype").HasColumnType("notification_type_enum");
        });

        modelBuilder.Entity<Notificationpreference>(entity =>
        {
            entity.Property("Notificationfrequency").HasField("_notificationfrequency").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("notificationfrequency").HasColumnType("notification_frequency_enum");

            entity.Property("NotificationGranularity").HasField("_notificationGranularity").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("notificationgranularity").HasColumnType("notification_granularity_enum");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("order_status_enum");

            entity.Property("DeliveryType").HasField("_deliveryType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("deliverytype").HasColumnType("delivery_duration_enum");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property("Purpose").HasField("_purpose").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("purpose").HasColumnType("payment_purpose_enum");

            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("transaction_status_enum");
        });

        modelBuilder.Entity<PricingRule>(entity =>
        {
            entity.Property("TransportMode").HasField("_transportMode").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("transportmode").HasColumnType("transport_mode");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("product_status");
        });

        modelBuilder.Entity<Purchaseorder>(entity =>
        {
            entity.Property("status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("purchase_order_status_enum");
        });

        // ==========================================
        // R
        // ==========================================
        modelBuilder.Entity<Reliabilityrating>(entity =>
        {
            entity.Property("rating").HasField("_rating").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("rating").HasColumnType("rating_band_enum");
        });

        modelBuilder.Entity<Replenishmentrequest>(entity =>
        {
            entity.Property("status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("replenishment_status_enum");
        });

        modelBuilder.Entity<Reportexport>(entity =>
        {
            entity.Property("type").HasField("_type").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("type").HasColumnType("visual_type_enum");

            entity.Property("format").HasField("_format").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("format").HasColumnType("file_format_enum");
        });

        modelBuilder.Entity<Returnitem>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("return_item_status");
        });

        modelBuilder.Entity<Returnrequest>(entity =>
        {
            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("return_request_status");
        });

        modelBuilder.Entity<ReturnStage>(entity =>
        {
            entity.Property("StageType").HasField("_stageType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("stagetype").HasColumnType("carbon_stage_type");

            entity.Property("StageTypeAlt").HasField("_stageTypeAlt").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("stagetypealt").HasColumnType("stagetype");
        });

        modelBuilder.Entity<RouteLeg>(entity =>
        {
            entity.Property("TransportMode").HasField("_transportMode").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("transportmode").HasColumnType("transport_mode");
        });

        // ==========================================
        // S - V
        // ==========================================
        modelBuilder.Entity<ShippingOption>(entity =>
        {
            entity.Property("PreferenceType").HasField("_preferenceType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("preferencetype").HasColumnType("preference_type");

            entity.Property("TransportMode").HasField("_transportMode").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("transportmode").HasColumnType("transport_mode");
        });

        modelBuilder.Entity<Staffaccesslog>(entity =>
        {
            entity.Property("EventType").HasField("_eventType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("eventtype").HasColumnType("access_event_type");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property("category").HasField("_category").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("category").HasColumnType("supplier_category_enum");

            entity.Property("decision").HasField("_decision").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("decision").HasColumnType("vetting_decision_enum");
        });

        modelBuilder.Entity<Suppliercategorychangelog>(entity =>
        {
            entity.Property("category").HasField("_category").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("category").HasColumnType("supplier_category_enum");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property("Type").HasField("_type").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("type").HasColumnType("transaction_type_enum");

            entity.Property("Purpose").HasField("_purpose").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("purpose").HasColumnType("transaction_purpose_enum");

            entity.Property("Status").HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("status").HasColumnType("transaction_status_enum");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.Property("TransportMode").HasField("_transportMode").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("transportmode").HasColumnType("transport_mode");
        });

        modelBuilder.Entity<TransportationHub>(entity =>
        {
            entity.Property("HubType").HasField("_hubType").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("hubtype").HasColumnType("hub_type");
        });

        modelBuilder.Entity<User>(entity =>
      {
            entity.Property<UserRole>("UserRole").HasField("_userRole").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("userrole").HasColumnType("user_role_enum");
      });

        modelBuilder.Entity<Vettingrecord>(entity =>
        {
            entity.Property("decision").HasField("_decision").UsePropertyAccessMode(PropertyAccessMode.Field)
                  .HasColumnName("decision").HasColumnType("vetting_decision_enum");
        });
    }
}