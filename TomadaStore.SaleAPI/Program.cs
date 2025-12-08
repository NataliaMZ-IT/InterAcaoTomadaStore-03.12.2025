using TomadaStore.SaleAPI.Data;
using TomadaStore.SaleAPI.Repositories;
using TomadaStore.SaleAPI.Repositories.Interfaces;
using TomadaStore.SaleAPI.Services.Interfaces;
using TomadaStore.SaleAPI.Services.v1;
using TomadaStore.SaleAPI.Services.v2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddScoped<ConnectionDB>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleServiceV1>();
builder.Services.AddScoped<SaleServiceV2>();

builder.Services.AddHttpClient("Customer", client =>
    client.BaseAddress = new Uri("https://localhost:5001/api/v1/customer/"));

builder.Services.AddHttpClient("Product", client =>
    client.BaseAddress = new Uri("https://localhost:6001/api/v1/product/"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
