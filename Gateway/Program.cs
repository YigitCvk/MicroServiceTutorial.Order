using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

// Web uygulamasını oluşturmak için gerekli builder'ı oluşturuyoruz.
var builder = WebApplication.CreateBuilder(args);

// Konfigürasyon dosyalarını ekliyoruz.
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)    
    .AddJsonFile($"appsettings.development.{builder.Environment.EnvironmentName}.json", true, true)    
    .AddJsonFile($"appsettings.production.{builder.Environment.EnvironmentName}.json", true, true)    
    .AddJsonFile("ocelot.json", false, false)
    .AddEnvironmentVariables();

// Ocelot servislerini ekliyoruz, Consul kullanarak servis keşfi yapılandırması da ekleniyor.
builder.Services.AddOcelot()
    .AddConsul();
    //.AddCacheManager(x =>
    //{
    //    x.WithDictionaryHandle();
    //})
    //.AddPolly();

// Uygulamayı oluşturuyoruz.
var app = builder.Build();
  
// Uygulama geliştirme modunda çalışıyorsa, hata sayfasını kullanıyoruz.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Basit bir test için "/" endpoint'ini ekliyoruz.
app.MapGet("/", () => "Merhaba Dünya!");

// Ocelot middleware'ini uygulama pipeline'ına ekliyoruz.
app.UseOcelot();

// Uygulamayı çalıştırıyoruz.
app.Run();
