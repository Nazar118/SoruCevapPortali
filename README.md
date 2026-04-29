\# 💬 Soru-Cevap Portalı



ASP.NET Core MVC ve SignalR kullanılarak geliştirilmiş, gerçek zamanlı veri güncellemeleri sağlayan interaktif bir soru-cevap platformudur.



\## 📸 Ekran Görüntüleri



\### 👨‍💼 Admin Paneli

!\[Admin Panel](images/admin.png)



\### 👤 Kullanıcı Arayüzü

!\[User Panel](images/user.png)



\## 🚀 Özellikler



\- SignalR ile gerçek zamanlı veri güncellemeleri

\- Kullanıcıların soru sorma ve cevaplama sistemi

\- Dinamik içerik güncelleme (AJAX)

\- Admin panel üzerinden içerik yönetimi

\- Responsive (mobil uyumlu) tasarım



\## 🛠️ Kullanılan Teknolojiler



\- Backend: ASP.NET Core MVC

\- Realtime: SignalR

\- Database: Microsoft SQL Server

\- ORM: Entity Framework Core

\- Frontend: HTML, CSS, Bootstrap, JavaScript, jQuery



\## ⚙️ Kurulum



Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyebilirsiniz:



1\. Repoyu klonlayın:

`git clone https://github.com/Nazar118/SoruCevapPortali.git`



2\. Veritabanını ayarlayın:

\- SQL Server'da yeni bir database oluşturun.

\- `appsettings.json` dosyasındaki connection string'i güncelleyin.



3\. Migration uygulayın:

`dotnet ef database update`



4\. Uygulamayı çalıştırın:

`dotnet run`



\## 📂 Proje Yapısı



\- `Controllers/` → İş mantığı

\- `Models/` → Veri modelleri

\- `Views/` → UI katmanı

\- `Data/` → Veritabanı işlemleri

\- `wwwroot/` → Statik dosyalar



\## 🔐 Kimlik Doğrulama



\- Kullanıcı giriş sistemi mevcuttur

\- Rol bazlı yetkilendirme uygulanmıştır (Admin / User)



\## 📌 Notlar



Bu proje eğitim amaçlı geliştirilmiştir ve aktif olarak geliştirilmeye devam edilmektedir.

