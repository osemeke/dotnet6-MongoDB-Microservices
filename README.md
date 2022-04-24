# dotnet6-MongoDB-Microservices
Sample implementation of microservice with MongoDB database and Redis Caching

## Ocelot API Gateway Implementation

- Empty project or minimal API
- Install-Package Ocelot
- add ocelot.json configuration
- add Logging
- Ensure CORS is enabled on the services 
- Ensure there is no 403 redirects to https when testing on http 

### Sample .net6 minimal API code

```csharp
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("API Gateway!");
    });
});

await app.UseOcelot(); 

app.Run();

```
