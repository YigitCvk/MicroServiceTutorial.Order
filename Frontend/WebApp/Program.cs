using System.Configuration;
using WebApp.Models;
using WebApp.Services;
using WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IIdentityService,IdentityService>();
builder.Services.AddMvc();
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings")); // Change this line
builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings")); // Change this line
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
