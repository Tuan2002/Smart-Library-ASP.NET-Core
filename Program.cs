using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Middleware;
using Smart_Library.Services;
using Smart_Library.Utils;
using static Smart_Library.Config.AppRules;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TuanLocal") ?? throw new InvalidOperationException("Connection string not found.");
// Add services to connect to database
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString, x => x.UseDateOnlyTimeOnly()));
// Add services to authentication
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => { options.SignIn.RequireConfirmedAccount = false; })
    .AddEntityFrameworkStores<ApplicationDBContext>();
// Add services to configure cookie 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/error/403";
});
// Add services to configure lockout settings
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});
// Add services to configure web socket
builder.Services.AddSingleton<WebSocketHandler>();
// Add service to compile sass
#if DEBUG
builder.Services.AddSassCompiler();
#endif
// Add services to lower case url
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDateOnlyTimeOnlyStringConverters();
// Add services to access HttpContext from custom service
builder.Services.AddHttpContextAccessor();
// Add custom application services.
builder.Services.AddScoped<IUsersManagerService, UsersManagerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBooksManagerService, BooksManagerService>();
builder.Services.AddScoped<IAuthorsManagerService, AuthorsManagerService>();
builder.Services.AddScoped<ICategoriesManagerService, CategoriesManagerService>();
builder.Services.AddScoped<IBooksService, BooksService>();

var app = builder.Build();
var options = new RewriteOptions().Add(new RedirectLowerCaseRule());
app.UseRewriter(options);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.UseMiddleware<ForceSignOutOnLockout>();
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();
app.MapControllerRoute(
    name: "areas",
    pattern: "{Area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();