using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;
using SoruCevapPortali.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVÝS AYARLARI (BUILDER KISMI) ---

// Veritabaný baðlantýsý
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SoruCevapPortali.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository Tanýmlamalarý
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
builder.Services.AddScoped<IRepository<Answer>, AnswerRepository>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IRepository<Report>, ReportRepository>();

// Giriþ/Çýkýþ Ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Admin deðil, genel Account controller'a yönlendiriyoruz
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddControllersWithViews();

// --> SIGNALR SERVÝSÝNÝ BURAYA EKLÝYORUZ <--
builder.Services.AddSignalR();

// --- UYGULAMA OLUÞTURULUYOR ---
var app = builder.Build();

// --- 2. UYGULAMA AYARLARI (APP KISMI) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // CSS/JS dosyalarý için gerekli (MapStaticAssets yerine genelde bu kullanýlýr)
app.UseRouting();

app.UseAuthentication(); // Kimlik Doðrulama
app.UseAuthorization();  // Yetkilendirme

// --> SIGNALR HUB ROTASINI BURAYA EKLÝYORUZ <--
app.MapHub<SoruCevapPortali.Hubs.GeneralHub>("/general-hub");

// Rotalar (Routes)
app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();