using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
//using OrderService.Data;
using ReportingService.Entities;
using ReportingService.Options;
using ReportingService.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container

builder.Services.AddScoped<ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Osemeke Minimal API", Version = "v1" });
});

builder.Services.AddCors();

var appsettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

builder.Services.Configure<AppSettings>(appsettings.GetSection("Settings"));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection(); // force use https. comment to use postman or File > Settings > Off the SSL certificate verification in General Tab

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Osemeke Minimal API");
    options.RoutePrefix = string.Empty; //"swagger/ui"; //string.Empty; // to arrest the root page use string.Empty
                                        // clear browser cache anytime this value is cahange
});

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// create
app.MapPost("/products", async (Product model, ProductService s) =>
{
    await s.Create(model);
    return Results.Ok(model);
});

app.Run();
