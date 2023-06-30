using Demo.Project2.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "ClientSchemes";
})
    .AddCookie("ClientSchemes", options =>
    {
        options.LoginPath = "/customer/login";
        options.LogoutPath = "/customer/logout";
        options.AccessDeniedPath = "/customer/forbidden";
    })
    .AddCookie("AdminSchemes", options =>
    {
        options.LoginPath = "/admin/auth/index";
        options.LogoutPath = "/admin/auth/logout";
        options.AccessDeniedPath = "/admin/auth/forbidden";
    });

builder.Services.AddSession();

builder.Services.AddDbContext<DemoProject2DbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    var principal = new ClaimsPrincipal();
    var result1 = await context.AuthenticateAsync("ClientSchemes");
    if (result1.Principal != null)
    {
        principal.AddIdentities(result1.Principal.Identities);
    }
    var result2 = await context.AuthenticateAsync("AdminSchemes");
    if (result2.Principal != null)
    {
        principal.AddIdentities(result2.Principal.Identities);
    }
    context.User = principal;
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
