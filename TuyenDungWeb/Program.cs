using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Notification;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.DataAccess.Services;
using TuyenDungWeb.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "183853191108679";
    option.AppSecret = "a618f63ab9c17c1fa5a9f947b38ef124";
});
builder.Services.AddAuthentication().AddGoogle(option =>
{
    option.ClientId = "14343377810-ictv6a6ddi5bs0k4v0rrm19bjdsleiov.apps.googleusercontent.com";
    option.ClientSecret = "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";
});
//builder.Services.AddAuthentication().AddMicrosoftAccount(option =>
//{
//    option.ClientId = "ec4d380d-d631-465d-b473-1e26ee706331";
//    option.ClientSecret = "qMW8Q~LlEEZST~SDxDgcEVx_45LJQF2cQ_rEKcSQ";
//});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<NotificationService>();
var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();
//builder.Services.AddScoped<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<AdminNotificationsHub>("/adminNotificationsHub");
app.MapControllers();
//SeedDatabase();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.Run();
//void SeedDatabase()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//        dbInitializer.Initialize();
//    }
//}