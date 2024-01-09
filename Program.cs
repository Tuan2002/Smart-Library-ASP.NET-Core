using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using static Smart_Library.Config.SystemRules;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Middleware;
using Smart_Library.Services;
using Smart_Library.Utils;
using Microsoft.Extensions.FileProviders;
using AutoMapper;
using Smart_Library.Config.AutoMapper;

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
// Add services to configure session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "SmartLibrary.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to configure auto mapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
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
builder.Services.AddScoped<IPublisherManagerService, PublishManagerService>();
builder.Services.AddScoped<ICategoriesManagerService, CategoriesManagerService>();
builder.Services.AddScoped<IOrdersManagerService, OrdersManagerService>();
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IOrderServices, OrderServices>();

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
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = new PathString("/uploads")
}
);
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.UseSession();
app.UseMiddleware<ForceSignOutOnLockout>();
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();
app.MapControllerRoute(
    name: "areas",
    pattern: "{Area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");
app.Run();