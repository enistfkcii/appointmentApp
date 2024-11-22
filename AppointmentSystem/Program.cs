using AppointmentSystem.Models;
using AppointmentSystem.Models.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using AppointmentSystem.Areas.Identity.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IEmailSender, DefaultEmailSender>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorPages();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        string adminEmail = "admin@example.com";
        string roleName = "Admin";
        await RoleAssignment.AssignRole(services, adminEmail, roleName);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Hata: {ex.Message}");
    }
}
app.Run();
