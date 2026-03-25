using Microsoft.EntityFrameworkCore;
//using DeeplearningwithCapybara.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models; // Added for OpenApiInfo
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AI Study Planner API", Version = "v1" });
}); // Cực kỳ quan trọng để có Swagger!

// Giả sử dùng 1 Context duy nhất cho gọn
//builder.Services.AddDbContext<DeeplearningwithCapybaraContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DeeplearningwithCapybaraContextConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) // Tạm để false để test cho nhanh
//    .AddEntityFrameworkStores<DeeplearningwithCapybaraContext>();

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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();