using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot()
                .AddConsul();
                //.AddCacheManager(x =>
                //{
                //    x.WithDictionaryHandle();
                //})
                //.AddPolly();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseOcelot();
app.Run();
