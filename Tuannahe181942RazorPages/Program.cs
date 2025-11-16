using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation;
using Repositories.Interfaces;
using Services.Implementation;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm dịch vụ Razor Pages và SignalR
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// 2. Cấu hình Session
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Đăng ký DbContext (Quan trọng!)
// Đọc chuỗi kết nối
var connectionString = builder.Configuration.GetConnectionString("FUNewsManagementDB");
builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(connectionString));

// 4. Đăng ký các lớp Repositories và Services cho DI
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Thêm UseAuthentication nếu bạn dùng Identity, nhưng ở đây dùng Session nên chỉ cần UseAuthorization
app.UseAuthorization();
app.UseSession(); // Kích hoạt Session

// Map Razor Pages và cấu hình trang mặc định
app.MapRazorPages();

app.MapHub<Tuannahe181942RazorPages.Hubs.NewsHub>("/newsHub"); // Sẽ tạo ở Bước 8

// Điều hướng trang gốc (/) đến /Login
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Login");
        return;
    }
    await next();
});


app.Run();