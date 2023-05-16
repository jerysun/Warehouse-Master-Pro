using Microsoft.EntityFrameworkCore;
using Core.IRepositories;
using Infrastructure.Data.EF;
using Core.Models;
using Core;
using System.Reflection;
using API.Profiles;
using Serilog;
using Infrastructure.Data.EF.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddMediatR(_ => _.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        typeof(NotEnoughStockException).Assembly
    ))
    .AddScoped<IRepository<int, Product>, Repository<int, Product, ProductContext>>()
    .AddDbContext<ProductContext>(options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("default")!))
    .AddAutoMapper(typeof(APIProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
      policy => policy
      .WithOrigins("http://localhost:4200", "http://localhost:82")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.CustomSchemaIds(x => x.FullName?.Replace("+", ".")));

var _loggrer = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.AddSerilog(_loggrer);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

using var seedScope = app.Services.CreateScope();
var services = seedScope.ServiceProvider;

try
{
    var db = services.GetRequiredService<ProductContext>();
    await db.Database.MigrateAsync();
    await Seed.SeedProductsAsync(db);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    if (logger == null)
    {
        throw;
    }
    logger.LogCritical(ex, "An critical error occured during migration!");
}

app.MapControllers();
app.Run();

public record class StockLevel(int QuantityInStock);

public partial class Program { }