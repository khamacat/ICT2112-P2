using ProRental.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProRental.Domain.Enums;
using ProRental.Domain.Control;
using ProRental.Data.Gateways;
using ProRental.Interfaces;
using ProRental.Interfaces.Data;
using ProRental.Data;
using ProRental.Interfaces.Domain;
using ProRental.Domain.Controls;
using ProRental.Controllers;
using ProRental.Controllers.Module1;
using ProRental.Data.Services;
using ProRental.Data.Interfaces;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/Module2/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Module2/Shared/{0}.cshtml");
});

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var connectionString = builder.Configuration.GetConnectionString("Default");

// 2. Create the builder and map your strict PostgreSQL Enum
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<AccessEventType>("access_event_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<AlertStatus>("alert_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<AnalyticsType>("analytics_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<BatchStatus>("batch_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<CarbonStageType>("carbon_stage_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<CartStatus>("cart_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<CheckoutStatus>("checkout_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ClearanceBatchStatus>("clearance_batch_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ClearanceStatus>("clearance_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<DeliveryDuration>("delivery_duration_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<DeliveryType>("delivery_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<FileFormat>("file_format_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<HubType>("hub_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<InventoryStatus>("inventory_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<LoanStatus>("loan_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<LogType>("log_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<NotificationFrequency>("notification_frequency_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<NotificationGranularity>("notification_granularity_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<NotificationType>("notification_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<OrderHistoryStatus>("order_history_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<OrderStatus>("order_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PaymentMethod>("payment_method_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PaymentPurpose>("payment_purpose_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<POStatus>("po_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PreferenceType>("preference_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ProductStatus>("product_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PurchaseOrderStatus>("purchase_order_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<RatingBand>("rating_band_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<RentalStatus>("rental_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReplenishmentReason>("reason_code_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReplenishmentStatus>("replenishment_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReturnItemStatus>("return_item_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReturnRequestStatus>("return_request_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReturnStatus>("return_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ShipmentStatus>("shipment_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<StageType>("stagetype", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<SupplierCategory>("supplier_category_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransactionPurpose>("transaction_purpose_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransactionStatus>("transaction_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransactionType>("transaction_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransportMode>("transport_mode", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<UserRole>("user_role_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<VettingDecision>("vetting_decision_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<VettingResult>("vetting_result_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<VisualType>("visual_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());

// 3. Build the data source
var dataSource = dataSourceBuilder.Build();

// 4. Register the DbContext using the data source instead of a raw string
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(dataSource));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dataSource, o =>
    {
        o.MapEnum<AccessEventType>("access_event_type");
        o.MapEnum<AlertStatus>("alert_status");
        o.MapEnum<AnalyticsType>("analytics_type_enum");
        o.MapEnum<BatchStatus>("batch_status");
        o.MapEnum<CarbonStageType>("carbon_stage_type");
        o.MapEnum<CartStatus>("cart_status_enum");
        o.MapEnum<CheckoutStatus>("checkout_status_enum");
        o.MapEnum<ClearanceBatchStatus>("clearance_batch_status");
        o.MapEnum<ClearanceStatus>("clearance_status");
        o.MapEnum<DeliveryDuration>("delivery_duration_enum");
        o.MapEnum<DeliveryType>("delivery_type_enum");
        o.MapEnum<FileFormat>("file_format_enum");
        o.MapEnum<HubType>("hub_type");
        o.MapEnum<InventoryStatus>("inventory_status");
        o.MapEnum<LoanStatus>("loan_status");
        o.MapEnum<LogType>("log_type_enum");
        o.MapEnum<NotificationFrequency>("notification_frequency_enum");
        o.MapEnum<NotificationGranularity>("notification_granularity_enum");
        o.MapEnum<NotificationType>("notification_type_enum");
        o.MapEnum<OrderHistoryStatus>("order_history_status_enum");
        o.MapEnum<OrderStatus>("order_status_enum");
        o.MapEnum<PaymentMethod>("payment_method_enum");
        o.MapEnum<PaymentPurpose>("payment_purpose_enum");
        o.MapEnum<POStatus>("po_status_enum");
        o.MapEnum<PreferenceType>("preference_type");
        o.MapEnum<ProductStatus>("product_status");
        o.MapEnum<PurchaseOrderStatus>("purchase_order_status_enum");
        o.MapEnum<RatingBand>("rating_band_enum");
        o.MapEnum<RentalStatus>("rental_status_enum");
        o.MapEnum<ReplenishmentReason>("reason_code_enum");
        o.MapEnum<ReplenishmentStatus>("replenishment_status_enum");
        o.MapEnum<ReturnItemStatus>("return_item_status");
        o.MapEnum<ReturnRequestStatus>("return_request_status");
        o.MapEnum<ReturnStatus>("return_status_enum");
        o.MapEnum<ShipmentStatus>("shipment_status_enum");
        o.MapEnum<StageType>("stagetype");
        o.MapEnum<SupplierCategory>("supplier_category_enum");
        o.MapEnum<TransactionPurpose>("transaction_purpose_enum");
        o.MapEnum<TransactionStatus>("transaction_status_enum");
        o.MapEnum<TransactionType>("transaction_type_enum");
        o.MapEnum<TransportMode>("transport_mode");
        o.MapEnum<UserRole>("user_role_enum");
        o.MapEnum<VettingDecision>("vetting_decision_enum");
        o.MapEnum<VettingResult>("vetting_result_enum");
        o.MapEnum<VisualType>("visual_type_enum");
    }));

//Services builder(add your mappers/gateways, controllers, control and interface classes here)
//Team P2-1
// Data source

// Domain

// Presentation/Controllers


//Team P2-2
builder.Services.AddScoped<IPurchaseOrderMapper, PurchaseOrderMapper>();
builder.Services.AddScoped<IPOLineItemMapper, POLineItemMapper>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderControl>();
// Data source
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IReplenishmentRequestMapper, ProRental.Data.Module2.Gateways.ReplenishmentRequestMapper>();
builder.Services.AddScoped<ProRental.Interfaces.Module2.IReplenishmentRequestQuery, ProRental.Data.Module2.Gateways.ReplenishmentRequestMapper>();

// Domain
builder.Services.AddScoped<ProRental.Domain.Module2.P22.Controls.ReplenishmentRequestControl>();

builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IReliabilityRatingMapper, ProRental.Data.Module2.Gateways.ReliabilityRatingMapper>();
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Controls.SupplierScoringControl>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IVettingRecordMapper, ProRental.Data.Module2.Gateways.VettingRecordMapper>();
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Controls.VettingControl>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.ISupplierMapper, ProRental.Data.Module2.Gateways.SupplierMapper>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.ICategoryChangeLogMapper, ProRental.Data.Module2.Gateways.CategoryChangeLogMapper>();

// Domain
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Controls.SupplierControl>();
builder.Services.AddScoped<ProRental.Interfaces.Module2.ISupplier>(sp => sp.GetRequiredService<ProRental.Domain.Module2.P2_2.Controls.SupplierControl>());
builder.Services.AddScoped<ProRental.Interfaces.Module2.IVerifiedSupplierRegistry>(sp => sp.GetRequiredService<ProRental.Domain.Module2.P2_2.Controls.SupplierControl>());
builder.Services.AddScoped<ProRental.Interfaces.Module2.ISupplierVettingGateway>(sp => sp.GetRequiredService<ProRental.Domain.Module2.P2_2.Controls.SupplierControl>());
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Controls.SupplierCategoryChangeLogControl>();
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Factories.SupplierRegistryFactory>();


builder.Services.AddScoped<IAnalyticsData, AnalyticsControl>();
builder.Services.AddScoped<IAnalyticsMapper, AnalysisRecordMapper>();
builder.Services.AddScoped<IReportExportMapper, ReportMapper>();
builder.Services.AddScoped<ITransactionLogService, TransactionLogService>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IVettingRecordMapper, ProRental.Data.Module2.Gateways.VettingRecordMapper>();

// Domain
builder.Services.AddScoped<AnalyticsControl>();
builder.Services.AddScoped<ReportExportControl>();
builder.Services.AddScoped<AnalyticsFactory>();
builder.Services.AddScoped<IAnalyticsData, AnalyticsControl>();

builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.ITransactionLogGateway, ProRental.Data.Module2.Gateways.TransactionLogGateway>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IRentalOrderLogGateway, ProRental.Data.Module2.Gateways.RentalOrderLogGateway>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.ILoanLogGateway, ProRental.Data.Module2.Gateways.LoanLogGateway>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IReturnLogGateway, ProRental.Data.Module2.Gateways.ReturnLogGateway>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IClearanceLogGateway, ProRental.Data.Module2.Gateways.ClearanceLogGateway>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IPurchaseOrderLogGateway, ProRental.Data.Module2.Gateways.PurchaseOrderLogGateway>();
builder.Services.AddScoped<ProRental.Data.Module2.Interfaces.IReliabilityRatingMapper, ProRental.Data.Module2.Gateways.ReliabilityRatingMapper>();
// Domain
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Controls.TransactionLogControl>();
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Controls.TransactionFilterControl>();
builder.Services.AddScoped<ProRental.Interfaces.IPurchaseOrderService, ProRental.Domain.Control.PurchaseOrderControl>();
builder.Services.AddScoped<ProRental.Domain.Module2.P2_2.Controls.VettingControl>();
builder.Services.AddScoped<ProRental.Interfaces.Module2.IVerifiedSupplierRegistry>(sp => sp.GetRequiredService<ProRental.Domain.Module2.P2_2.Controls.VettingControl>());
// Presentation/Controllers
builder.Services.AddScoped<ProRental.Interfaces.IRentalOrderLogger>(sp => sp.GetRequiredService<ProRental.Domain.Module2.P2_2.Controls.TransactionLogControl>());
builder.Services.AddScoped<ProRental.Interfaces.IInventoryTransactionLogger>(sp => sp.GetRequiredService<ProRental.Domain.Module2.P2_2.Controls.TransactionLogControl>());
builder.Services.AddScoped<ProRental.Interfaces.ITransactionLoggingUI>(sp => sp.GetRequiredService<ProRental.Domain.Module2.P2_2.Controls.TransactionFilterControl>());

//Team P2-3
// Data source
builder.Services.AddScoped<IAlertMapper, AlertMapper>();
builder.Services.AddScoped<IInventoryItemMapper, InventoryItemMapper>();
builder.Services.AddScoped<ICategoryMapper, CategoryMapper>();
builder.Services.AddScoped<IDamageReportMapper, DamageReportMapper>();

// ── Core Mapper Implementations (Registered Once) ──
builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<LoanListMapper>();
builder.Services.AddScoped<LoanItemMapper>();
builder.Services.AddScoped<ReturnRequestMapper>();
builder.Services.AddScoped<ReturnItemMapper>();
builder.Services.AddScoped<ClearanceBatchMapper>();
builder.Services.AddScoped<ClearanceItemMapper>();

// ── Forwarding the Standard Interfaces ──
builder.Services.AddScoped<IProductMapper>(sp => sp.GetRequiredService<ProductMapper>());
builder.Services.AddScoped<ILoanListMapper>(sp => sp.GetRequiredService<LoanListMapper>());
builder.Services.AddScoped<ILoanItemMapper>(sp => sp.GetRequiredService<LoanItemMapper>());
builder.Services.AddScoped<IReturnRequestMapper>(sp => sp.GetRequiredService<ReturnRequestMapper>());
builder.Services.AddScoped<IReturnItemMapper>(sp => sp.GetRequiredService<ReturnItemMapper>());
builder.Services.AddScoped<IClearanceBatchMapper>(sp => sp.GetRequiredService<ClearanceBatchMapper>());
builder.Services.AddScoped<IClearanceItemMapper>(sp => sp.GetRequiredService<ClearanceItemMapper>());

// ── Forwarding the Read-Only Segregated Interfaces ──
builder.Services.AddScoped<IProductRead>(sp => sp.GetRequiredService<ProductMapper>());
builder.Services.AddScoped<ILoanListRead>(sp => sp.GetRequiredService<LoanListMapper>());
builder.Services.AddScoped<ILoanItemRead>(sp => sp.GetRequiredService<LoanItemMapper>());
builder.Services.AddScoped<IReturnRequestRead>(sp => sp.GetRequiredService<ReturnRequestMapper>());
builder.Services.AddScoped<IReturnItemRead>(sp => sp.GetRequiredService<ReturnItemMapper>());
builder.Services.AddScoped<IClearanceBatchRead>(sp => sp.GetRequiredService<ClearanceBatchMapper>());
builder.Services.AddScoped<IClearanceItemRead>(sp => sp.GetRequiredService<ClearanceItemMapper>());

// Domain - Control Classes
// category
builder.Services.AddScoped<CategoryControl>();
builder.Services.AddScoped<ICategoryCRUD>(sp => sp.GetRequiredService<CategoryControl>());
builder.Services.AddScoped<ICategoryQuery>(sp => sp.GetRequiredService<CategoryControl>());

//inventory
builder.Services.AddScoped<InventoryManagementControl>();
builder.Services.AddScoped<iInventoryCRUDControl>(sp => sp.GetRequiredService<InventoryManagementControl>());
builder.Services.AddScoped<iInventoryQueryControl>(sp => sp.GetRequiredService<InventoryManagementControl>());
builder.Services.AddScoped<iInventoryStatusControl>(sp => sp.GetRequiredService<InventoryManagementControl>());
builder.Services.AddScoped<iStockSubject>(sp => sp.GetRequiredService<InventoryManagementControl>());

builder.Services.AddScoped<LowStockAlertControl>();
builder.Services.AddScoped<iAlertControl>(sp => sp.GetRequiredService<LowStockAlertControl>());
builder.Services.AddScoped<iStockObserver>(sp => sp.GetRequiredService<LowStockAlertControl>());

//product
builder.Services.AddScoped<ProductCatalogControl>();
builder.Services.AddScoped<IProductQuery>(sp => sp.GetRequiredService<ProductCatalogControl>());
builder.Services.AddScoped<IProductCRUD>(sp => sp.GetRequiredService<ProductCatalogControl>());
builder.Services.AddScoped<IProductBulkCommand>(sp => sp.GetRequiredService<ProductCatalogControl>());
builder.Services.AddScoped<IProductActions>(sp => sp.GetRequiredService<ProductCatalogControl>());

//clearance
builder.Services.AddScoped<ClearanceBatchControl>();
builder.Services.AddScoped<iClearanceBatchControl>(sp => sp.GetRequiredService<ClearanceBatchControl>());
builder.Services.AddScoped<iClearanceBatchQuery>(sp => sp.GetRequiredService<ClearanceBatchControl>());

builder.Services.AddScoped<ClearanceItemControl>();
builder.Services.AddScoped<iClearanceItemControl>(sp => sp.GetRequiredService<ClearanceItemControl>());
builder.Services.AddScoped<iClearanceItemQuery>(sp => sp.GetRequiredService<ClearanceItemControl>());

//return
builder.Services.AddScoped<ReturnOrderControl>();
builder.Services.AddScoped<iReturnOrderQuery>(sp => sp.GetRequiredService<ReturnOrderControl>());
builder.Services.AddScoped<iReturnOrderCRUD>(sp => sp.GetRequiredService<ReturnOrderControl>());
builder.Services.AddScoped<iReturnProcess>(sp => sp.GetRequiredService<ReturnOrderControl>());
 
builder.Services.AddScoped<ReturnItemControl>();
builder.Services.AddScoped<iReturnItemQuery>(sp => sp.GetRequiredService<ReturnItemControl>());
builder.Services.AddScoped<iReturnItemCRUD>(sp => sp.GetRequiredService<ReturnItemControl>());
 
builder.Services.AddScoped<DamageReportControl>();
builder.Services.AddScoped<iDamageReportQuery>(sp => sp.GetRequiredService<DamageReportControl>());
builder.Services.AddScoped<iDamageReportCRUD>(sp => sp.GetRequiredService<DamageReportControl>());

//loan
builder.Services.AddScoped<LoanItemControl>();
builder.Services.AddScoped<ILoanItemCRUD>(sp => sp.GetRequiredService<LoanItemControl>());
builder.Services.AddScoped<ILoanItemQuery>(sp => sp.GetRequiredService<LoanItemControl>());

builder.Services.AddScoped<LoanListControl>();
builder.Services.AddScoped<ILoanActions>(sp => sp.GetRequiredService<LoanListControl>());
builder.Services.AddScoped<ILoanValidation>(sp => sp.GetRequiredService<LoanListControl>());
builder.Services.AddScoped<ILoanListQuery>(sp => sp.GetRequiredService<LoanListControl>());
builder.Services.AddScoped<ILoanListCRUD>(sp => sp.GetRequiredService<LoanListControl>());

//inventory services
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<IInventoryQueryFacade>(sp => sp.GetRequiredService<InventoryService>());
builder.Services.AddScoped<IResupplyService>(sp => sp.GetRequiredService<InventoryService>());
builder.Services.AddScoped<IInventoryService>(sp => sp.GetRequiredService<InventoryService>());

builder.Services.AddScoped<TransactionLogEnricher>();
builder.Services.AddScoped<ILoanLogEnricher>(sp => sp.GetRequiredService<TransactionLogEnricher>());
builder.Services.AddScoped<IReturnLogEnricher>(sp => sp.GetRequiredService<TransactionLogEnricher>());
builder.Services.AddScoped<IClearanceLogEnricher>(sp => sp.GetRequiredService<TransactionLogEnricher>());

// Presentation/Controllers


//Team P2-4
// Data source

// Domain

// Presentation/Controllers


//Team P2-5
// Data source

// Domain

// Presentation/Controllers


//Team P2-6
// Data source
// builder.Services.AddScoped<IOrderMapper, OrderMapper>();
// builder.Services.AddScoped<IOrderService, OrderManagementControl>();
// builder.Services.AddScoped<IInventoryService, FakeInventoryService>();
// // Domain

// // Presentation/Controllers
// builder.Services.AddScoped<IOrderService, OrderManagementControl>();

// Data source (mappers / DB-backed service implementations)
builder.Services.AddScoped<ISessionMapper, SessionMapper>();
builder.Services.AddScoped<IAuthenticationService, ProRentalAuthenticationService>();
builder.Services.AddScoped<ICustomerValidationService, CustomerValidationService>();

// Domain (controls — pure business logic, no DB dependency)
builder.Services.AddScoped<ISessionService, SessionControl>();
builder.Services.AddScoped<AuthenticationControl>();
builder.Services.AddScoped<CustomerIDValidationControl>();

// HTTP context accessor (required for session access in Razor layouts)
builder.Services.AddHttpContextAccessor();

// Session middleware (required for HttpContext.Session)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Presentation/Controllers
builder.Services.AddScoped<Module1Controller>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();      
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();