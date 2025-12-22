using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;
using SoruCevapPortali.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVÝS AYARLARI ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SoruCevapPortali.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole<int>>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;

    // Kullanýcý adý ve email eþsiz olsun
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<SoruCevapPortali.Data.ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});

// Repository Tanýmlamalarý
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
builder.Services.AddScoped<IRepository<Answer>, AnswerRepository>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IRepository<Report>, ReportRepository>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// --- 2. UYGULAMA AYARLARI ---

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

app.MapHub<SoruCevapPortali.Hubs.GeneralHub>("/general-hub");

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();