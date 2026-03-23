using ProRental.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using ProRental.Domain.Enums;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Data;
using ProRental.Interfaces.Domain;
using ProRental.Domain.Controls;
using ProRental.Controllers.Module1;
using ProRental.Data.Services;


// uncomment when ready to code
// using ProRental.Data;
// using ProRental.Domain.Controls;
// //using ProRental.Domain.Entities;
// using ProRental.Interfaces.Domain;
// using ProRental.Interfaces.Data;
// using ProRental.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
// Data source

// Domain

// Presentation/Controllers

//Team P2-3
// Data source

// Domain

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
