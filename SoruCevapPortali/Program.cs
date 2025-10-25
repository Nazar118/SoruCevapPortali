using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;
using SoruCevapPortali.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Veritabaný baðlantý cümlesini appsettings.json'dan alýyoruz
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i ve SQL Server baðlantýsýný servislere ekliyoruz
builder.Services.AddDbContext<SoruCevapPortali.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IRepository<Kullanici>, KullaniciRepository>();
builder.Services.AddScoped<IRepository<Soru>, SoruRepository>();
builder.Services.AddScoped<IRepository<Cevap>, CevapRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Auth/Login"; // Giriþ yapýlmamýþsa yönlendirilecek sayfa
        options.LogoutPath = "/Admin/Auth/Logout";
        options.AccessDeniedPath = "/Admin/Auth/AccessDenied"; // Yetkisi yoksa 
    });
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Admin gibi alanlara (Area) giden yolu tarif eden kural
app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
