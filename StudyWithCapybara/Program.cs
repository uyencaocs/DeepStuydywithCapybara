using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyWithCapybara.Models; // Đảm bảo gọi đúng namespace

var builder = WebApplication.CreateBuilder(args);

// 1. Kết nối Database
builder.Services.AddDbContext<StudyCabybaraDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Kích hoạt Identity Core
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<StudyCabybaraDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Bắt buộc để chạy UI của Identity

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// 3. Kích hoạt Middleware (Xác thực trước, Phân quyền sau)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Map các trang Login/Register

app.Run();