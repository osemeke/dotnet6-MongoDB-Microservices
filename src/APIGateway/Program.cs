using Ocelot.DependencyInjection;
using Ocelot.Middleware;
//using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.json", true, true);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
builder.Services.AddOcelot();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.UseOcelot().Wait();

app.Run();
