using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using PosWebApp.Context;
using PosWebAppBusinessLogic.Common;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppBusinessLogic.Repositories;
using PosWebAppBusinessLogic.Services;
using PosWebAppCommon.Interfaces;
using System.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Add MVC
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register a simple connection factory for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
// Register the DapperContext with its interface
builder.Services.AddSingleton<IDapperContext, DapperContext>();
builder.Services.AddScoped(typeof(IGenericDapperRepository<>), typeof(GenericDapperRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>(); // Add the new repository
builder.Services.AddScoped<DashboardRepository>(); // Add the new repository
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddScoped<ReportsService>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<LendingService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<PayrollService>();
builder.Services.AddScoped<InventoryService>();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession(); //

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
