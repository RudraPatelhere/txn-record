using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionRecordApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Database connection
var connStr = builder.Configuration.GetConnectionString("TransactionsDb");
builder.Services.AddDbContext<TransactionContext>(options =>
    options.UseSqlServer(connStr));

// Identity services
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<TransactionContext>();

var app = builder.Build();

// Middleware configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // 🔐 Must come before authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed Admin user and role
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await TransactionContext.CreateAdminUser(services);
}

// Start the app
await app.RunAsync();
