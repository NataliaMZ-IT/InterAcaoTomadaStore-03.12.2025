using TomadaStore.ProductAPI.Data;
using TomadaStore.ProductAPI.Repositories;
using TomadaStore.ProductAPI.Repositories.Interfaces;
using TomadaStore.ProductAPI.Services;
using TomadaStore.ProductAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddScoped<ConnectionDB>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
