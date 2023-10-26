using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Data;
using Smart_Library.Entities;
using static Smart_Library.Config.AppRules;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TuanLocal") ?? throw new InvalidOperationException("Connection string not found.");
// Add services to connect to database
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString, x => x.UseDateOnlyTimeOnly()));
// Add services to authentication
builder.Services.AddIdentity<ApplicationUser, UserRole>(options => { options.SignIn.RequireConfirmedAccount = false; })
    .AddEntityFrameworkStores<ApplicationDBContext>();
// Add services to configure lockout settings
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});
// Add service to compile sass
#if DEBUG
builder.Services.AddSassCompiler();
#endif
// Add services to lower case url
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Add services to the container.
builder.Services.AddControllersWithViews();

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
// Redirect to login page if user is not authenticated
app.UseStatusCodePages(async context =>
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;
    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        response.Redirect("/account/login");
        await Task.CompletedTask;
        return;
    }
    // if (response.StatusCode == (int)HttpStatusCode.NotFound)
    // {
    //     response.Redirect("/error/notfound");
    //     await Task.CompletedTask;
    // }
    // if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
    // {
    //     response.Redirect("/error/internalservererror");
    //     await Task.CompletedTask;
    // }
});
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

