using Microsoft.EntityFrameworkCore;
using DeeplearningwithCapybara.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Thêm dịch vụ Razor Pages cho Identity UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AI Study Planner API", Version = "v1" });
});


builder.Services.AddDbContext<DLWcapybara>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DLWcapybara")));

builder.Services.AddIdentity<Users, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<DLWcapybara>(); 

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "YOUR_GOOGLE_CLIENT_ID";
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "YOUR_GOOGLE_CLIENT_SECRET";
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "YOUR_FACEBOOK_APP_ID";
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "YOUR_FACEBOOK_APP_SECRET";
    }); 

var app = builder.Build();

// 2. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AI Study Planner API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages(); // Rất quan trọng: Ánh xạ các trang Identity (Login, Register...)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();